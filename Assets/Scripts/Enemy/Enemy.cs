using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid Grid;
    public Transform Objective;

    public float Hit = 1f;
    public float Speed = 5f;
    public float Health = 2f;

    private Animator m_Animator;
    private Path m_Path;
    [SerializeField]
    private AudioClip[] m_SpawnSounds;
    private AudioSource m_AS;
    private void Awake()
    {
        m_Animator = this.gameObject.GetComponent<Animator>();
        m_AS = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //m_AS.PlayOneShot(m_SpawnSounds[Random.Range(0, m_SpawnSounds.Length)]);
    }
    private void Update()
    {
        if (m_Path == null)
        {
            CalculatePath();
        }
        if (m_Path == null)
        {
            return;
        }

        // Find/Go to next CheckPoint
        Vector3 t_TargetPos = m_Path.Checkpoints[1].transform.position;
        float t_Step = Speed * Time.deltaTime;
        //Vector3 currentDirection = (Vector3.MoveTowards(transform.position, t_TargetPos, t_Step) - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, t_TargetPos, t_Step);
        


        //Arrived at checkpoint
        if (transform.position == t_TargetPos)
        {
            CalculatePath();

            if (m_Path == null)
            {
                return;
            }

            if (m_Path.Checkpoints.Count == 1)
            {
                Player.GetComponent<Player>().TakeDamage(Hit);
                Die();
            }
        }


    }
    private void FixedUpdate()
    {
        float t_MoveX = Input.GetAxis("Horizontal");

        if (t_MoveX < 0 && transform.localScale.x > 0 || t_MoveX > 0 && transform.localScale.x < 0)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));

        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < m_Path.Checkpoints.Count - 1; i++)
        {
            Gizmos.DrawLine(m_Path.Checkpoints[i].transform.position, m_Path.Checkpoints[i + 1].transform.position);
        }
    }

    private void CalculatePath()
    {
        Tile t_StartTile = Grid.GetTile(Grid.WorldToGrid(transform.position));
        Tile t_EndTile = Grid.GetTile(Grid.WorldToGrid(Objective.position));
        m_Path = Pathfinder.GetPath(t_StartTile, t_EndTile, false);
        Debug.Log(m_Path);
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

    }
    void OnDestroy()
    {
        Destroy(gameObject);
    }

}
