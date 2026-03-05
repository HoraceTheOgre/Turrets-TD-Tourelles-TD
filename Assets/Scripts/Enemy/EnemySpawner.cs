using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Prefab_Enemy;
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid Grid;
    public Transform[] Objectives;

    private float EnemyHit = 1f;
    private float EnemySpeed = 5f;
    private float EnemyHeath = 2f;

    private float IntervalMin = 2f;
    private float IntervalMax = 3f;

    private float m_Interval;
    private float m_LastSpawn;

    private void Update()
    {
        m_Interval = Random.Range(IntervalMin, IntervalMax);
        if (Time.time - m_LastSpawn >= m_Interval)
        {
            m_LastSpawn = Time.time;

            // Spawn
            Vector3 t_SpawnPos = Grid.GridToWorld(Grid.WorldToGrid(transform.position));

            GameObject t_NewEnemy = Instantiate(Prefab_Enemy, t_SpawnPos, Quaternion.identity);

            t_NewEnemy.GetComponent<Enemy>().Player = Player;
            t_NewEnemy.GetComponent<Enemy>().Pathfinder = Pathfinder;
            t_NewEnemy.GetComponent<Enemy>().Grid = Grid;
            t_NewEnemy.GetComponent<Enemy>().Objective = Objectives[Random.Range(0, Objectives.Length)];
            t_NewEnemy.GetComponent<Enemy>().Hit = EnemyHit;
            t_NewEnemy.GetComponent<Enemy>().Speed = EnemySpeed;
            t_NewEnemy.GetComponent<Enemy>().Health = EnemyHeath;
        }
        
    }
    public void ScaleEnemy(float Scale, float ScaleHit, float ScaleSpeed, float ScaleHealth, float ScaleIntervalMin, float ScaleIntervalMax)
    {
        EnemyHit = ScaleHit * Scale;
        EnemySpeed = ScaleSpeed * Scale;
        EnemyHeath = ScaleHealth * Scale;
        IntervalMin = ScaleIntervalMin;
        IntervalMax = ScaleIntervalMax += 1f;
    }
}
