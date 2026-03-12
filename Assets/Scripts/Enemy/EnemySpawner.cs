using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid       Grid;
    public Transform[] Objectives;

    private float EnemyHit   = 1f;
    private float EnemySpeed = 5f;
    private float EnemyHeath = 2f;
    private float IntervalMin = 2f;
    private float IntervalMax = 3f;

    private float        m_Interval;
    private float        m_LastSpawn;
    private SpawnEntry[] m_Pool   = null;
    private bool         m_Paused = false;

    public void SetSpawnPool(SpawnEntry[] pool) { m_Pool   = pool;   }
    public void SetPaused   (bool paused)       { m_Paused = paused; }

    private void Update()
    {
        if (m_Paused) return;

        m_Interval = Random.Range(IntervalMin, IntervalMax);

        if (Time.time - m_LastSpawn >= m_Interval)
        {
            m_LastSpawn = Time.time;
            SpawnEnemy(null);
        }
    }

    // Called by Wave to spawn the boss through the normal spawner pipeline
    public void SpawnSpecific(GameObject prefab)
    {
        SpawnEnemy(prefab);
    }

   private void SpawnEnemy(GameObject overridePrefab)
{
    GameObject prefab = overridePrefab;

    if (prefab == null)
    {
        if (m_Pool == null || m_Pool.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] No pool assigned by Wave!");
            return;
        }
        prefab = Wave.PickFromPool(m_Pool);
    }

    if (prefab == null) return;

    Vector3    spawnPos = Grid.GridToWorld(Grid.WorldToGrid(transform.position));
    GameObject obj      = Instantiate(prefab, spawnPos, Quaternion.identity);

    Enemy e = obj.GetComponent<Enemy>();
    if (e == null) return;

    e.Player     = Player;
    e.Pathfinder = Pathfinder;
    e.Grid       = Grid;
    e.Objective  = Objectives[Random.Range(0, Objectives.Length)];
    e.ScaleStats(1f, EnemyHit, EnemySpeed, EnemyHeath);
}

    public void ScaleEnemy(float Scale, float ScaleHit, float ScaleSpeed,
                           float ScaleHealth, float ScaleIntervalMin, float ScaleIntervalMax)
    {
        EnemyHit   = ScaleHit    * Scale;
        EnemySpeed = ScaleSpeed  * Scale;
        EnemyHeath = ScaleHealth * Scale;
        IntervalMin = ScaleIntervalMin;
        IntervalMax = ScaleIntervalMax + 1f;
    }
}