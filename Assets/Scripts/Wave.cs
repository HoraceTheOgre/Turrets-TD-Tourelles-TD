using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Wave : MonoBehaviour
{
    public int CurrentWave;

    public float Interval = 120f;
    [SerializeField]
    private float[] BasePlayerHP;
    [SerializeField] 
    private float[] BaseHit;
    [SerializeField]
    private float[] BaseSpeed;
    [SerializeField]
    private float[] BaseHealth;
    [SerializeField]
    private float[] IntervalScaleMin;
    [SerializeField]
    private float[] IntervalScaleMax;
    [SerializeField]
    private float m_ScaleMin;
    [SerializeField]
    private float m_ScaleMax;
    private float m_Scale;
    public GameObject Spawner1;
    public GameObject Spawner2;
    public GameObject Player;


    private float m_LastWave;

    [SerializeField]
    private AudioClip[] m_Tracks;
    public AudioSource AS;
    void Start()
    {
        m_LastWave = Time.time;
        //Play Track
        AS.PlayOneShot(m_Tracks[CurrentWave]);
        //Show Current Wave Stats
        MenuManager.Instance.HUD.DiplayWave(CurrentWave);
        m_Scale = Random.Range(m_ScaleMin, m_ScaleMin);
        MenuManager.Instance.HUD.DiplayStatus("Enemy Scale : " + m_Scale);
        Player.GetComponent<Player>().ScaleHP(BasePlayerHP[CurrentWave - 1]);
        Spawner1.GetComponent<EnemySpawner>().ScaleEnemy(m_Scale, BaseHit[CurrentWave - 1], BaseSpeed[CurrentWave - 1], BaseHealth[CurrentWave - 1], IntervalScaleMin[CurrentWave - 1], IntervalScaleMax[CurrentWave - 1]);
        Spawner2.GetComponent<EnemySpawner>().ScaleEnemy(m_Scale, BaseHit[CurrentWave - 1], BaseSpeed[CurrentWave - 1], BaseHealth[CurrentWave - 1], IntervalScaleMin[CurrentWave - 1], IntervalScaleMax[CurrentWave - 1]);
    }


    void Update()
    {

        if (Time.time - m_LastWave >= Interval)
        {
            m_LastWave = Time.time;
            if (CurrentWave == 4)
            {
                Player.GetComponent<Player>().Win();
            }
            AS.Stop();

            // Go to InBetween
            SceneManager.LoadScene("Lobby "+ CurrentWave, LoadSceneMode.Single);
        }

    }
}
