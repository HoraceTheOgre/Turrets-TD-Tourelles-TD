using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid       Grid;
    public Transform  Objective;

    public float Hit    = 1f;
    public float Speed  = 5f;
    public float Health = 2f;

    private Animator    m_Animator;
    private Path        m_Path;
    private AudioSource m_AS;

    [SerializeField] private AudioClip[] m_SpawnSounds;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_AS       = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_Path == null) CalculatePath();
        if (m_Path == null) return;

        Vector3 targetPos = m_Path.Checkpoints[1].transform.position;
        float   step      = Speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos)
        {
            CalculatePath();
            if (m_Path == null) return;

            if (m_Path.Checkpoints.Count == 1)
            {
                Player.GetComponent<Player>().TakeDamage(Hit);
                Die();
            }
        }
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        if (moveX < 0 && transform.localScale.x > 0 ||
            moveX > 0 && transform.localScale.x < 0)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnDestroy() { }

    public void ScaleStats(float scale, float baseHit, float baseSpeed, float baseHealth)
    {
        Hit    = baseHit    * scale;
        Speed  = baseSpeed  * scale;
        Health = baseHealth * scale;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        Debug.Log($"[Enemy: {gameObject.name}] HIT ──────────────────────────");
        Debug.Log($"  Damage Received : {damage}");
        Debug.Log($"  Health Remaining: {Health}");
        Debug.Log($"  Hit (Damage Out): {Hit}");
        Debug.Log($"  Speed           : {Speed}");
        Debug.Log($"  Position        : {transform.position}");
        Debug.Log($"  Alive           : {Health > 0}");

        if (Health <= 0)
        {
            Debug.Log($"[Enemy: {gameObject.name}] DIED");
            Destroy(gameObject);
        }
    }

    private void CalculatePath()
    {
        if (Grid == null || Pathfinder == null || Objective == null)
        {
            Debug.LogWarning($"[Enemy: {gameObject.name}] Missing Grid/Pathfinder/Objective!");
            return;
        }

        Tile startTile = Grid.GetTile(Grid.WorldToGrid(transform.position));
        Tile endTile   = Grid.GetTile(Grid.WorldToGrid(Objective.position));
        m_Path = Pathfinder.GetPath(startTile, endTile, false);
        Debug.Log(m_Path);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Path == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < m_Path.Checkpoints.Count - 1; i++)
        {
            Gizmos.DrawLine(
                m_Path.Checkpoints[i].transform.position,
                m_Path.Checkpoints[i + 1].transform.position);
        }
    }
}