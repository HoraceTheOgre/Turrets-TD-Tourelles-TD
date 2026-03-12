using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpawnEntry
{
    public GameObject EnemyPrefab;
    [Range(0f, 1f)]
    public float Weight = 1f;
}

[System.Serializable]
public class WaveSpawnPool
{
    public SpawnEntry[] Entries;
}

public class Wave : MonoBehaviour
{
    public int   CurrentWave;
    public float Interval = 120f;

    [SerializeField] private float[] BasePlayerHP;
    [SerializeField] private float[] BaseHit;
    [SerializeField] private float[] BaseSpeed;
    [SerializeField] private float[] BaseHealth;
    [SerializeField] private float[] IntervalScaleMin;
    [SerializeField] private float[] IntervalScaleMax;
    [SerializeField] private float   m_ScaleMin;
    [SerializeField] private float   m_ScaleMax;
    private float m_Scale;

    [Header("Spawn Pools (one per wave)")]
    [SerializeField] private WaveSpawnPool[] SpawnPools;

    [Header("Boss Event")]
    [SerializeField] private GameObject BossPrefab;
    [SerializeField] private Transform  BossSpawnPoint;
    [SerializeField] private float      BossSpawnTime = 60f;
    [SerializeField] private AudioClip  BossTrack;
    private bool m_BossSpawned = false;
    private bool m_BossActive  = false;

    public GameObject Spawner1;
    public GameObject Spawner2;
    public GameObject Player;

    [SerializeField] private AudioClip[] m_Tracks;
    public AudioSource AS;

    private float m_LastWave;
    private float WaveElapsed => Time.time - m_LastWave;

    void Start()
    {
        m_LastWave    = Time.time;
        m_BossSpawned = false;
        m_BossActive  = false;

        AS.clip = m_Tracks[CurrentWave - 1];
        AS.Play();

        MenuManager.Instance.HUD.DiplayWave(CurrentWave);

        m_Scale = Random.Range(m_ScaleMin, m_ScaleMax);
        MenuManager.Instance.HUD.DiplayStatus("Enemy Scale : " + m_Scale);

        Player.GetComponent<Player>().ScaleHP(BasePlayerHP[CurrentWave - 1]);

        SpawnEntry[] pool = GetCurrentPool();
        int idx = CurrentWave - 1;

        Spawner1.GetComponent<EnemySpawner>().SetSpawnPool(pool);
        Spawner1.GetComponent<EnemySpawner>().ScaleEnemy(
            m_Scale, BaseHit[idx], BaseSpeed[idx], BaseHealth[idx],
            IntervalScaleMin[idx], IntervalScaleMax[idx]);

        Spawner2.GetComponent<EnemySpawner>().SetSpawnPool(pool);
        Spawner2.GetComponent<EnemySpawner>().ScaleEnemy(
            m_Scale, BaseHit[idx], BaseSpeed[idx], BaseHealth[idx],
            IntervalScaleMin[idx], IntervalScaleMax[idx]);
    }

    void Update()
    {
        // Boss spawn trigger
        if (!m_BossSpawned && WaveElapsed >= BossSpawnTime)
            TriggerBossEvent();

        // Resume normal music after boss dies
        if (m_BossActive && !IsBossAlive())
        {
            m_BossActive = false;
            Spawner1.GetComponent<EnemySpawner>().SetPaused(false);
            Spawner2.GetComponent<EnemySpawner>().SetPaused(false);
            AS.Stop();
            AS.clip = m_Tracks[CurrentWave - 1];
            AS.Play();
            MenuManager.Instance.HUD.DiplayStatus("Boss defeated!");
        }

        // Wave end
        if (WaveElapsed >= Interval)
        {
            m_LastWave = Time.time;

            if (CurrentWave == 4)
            {
                Player.GetComponent<Player>().Win();
                return;
            }

            AS.Stop();
            SceneManager.LoadScene("Lobby " + CurrentWave, LoadSceneMode.Single);
        }
    }

    private void TriggerBossEvent()
    {
        m_BossSpawned = true;
        m_BossActive  = true;

        Spawner1.GetComponent<EnemySpawner>().SetPaused(true);
        Spawner2.GetComponent<EnemySpawner>().SetPaused(true);

        // Spawn boss through the spawner so pathfinding refs are set automatically
        Spawner1.GetComponent<EnemySpawner>().SpawnSpecific(BossPrefab);

        AS.Stop();
        AS.clip = BossTrack;
        AS.Play();

        MenuManager.Instance.HUD.DiplayStatus("BOSS INCOMING");
    }

    private bool IsBossAlive()
    {
        return FindObjectOfType<Boss>() != null;
    }

    private SpawnEntry[] GetCurrentPool()
    {
        if (SpawnPools == null || SpawnPools.Length == 0) return new SpawnEntry[0];
        int i = Mathf.Clamp(CurrentWave - 1, 0, SpawnPools.Length - 1);
        return SpawnPools[i].Entries;
    }

    public static GameObject PickFromPool(SpawnEntry[] pool)
    {
        if (pool == null || pool.Length == 0) return null;

        float total = 0f;
        foreach (var e in pool) total += Mathf.Max(0f, e.Weight);

        float roll = Random.Range(0f, total);
        float acc  = 0f;

        foreach (var e in pool)
        {
            acc += Mathf.Max(0f, e.Weight);
            if (roll <= acc) return e.EnemyPrefab;
        }

        return pool[pool.Length - 1].EnemyPrefab;
    }
}