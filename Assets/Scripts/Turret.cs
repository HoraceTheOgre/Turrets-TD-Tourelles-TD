using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float Range = 3f;
    public float Damage = 1f;
    public float Cooldown = 1f;
    private float m_LastBreak;

    public ParticleSystem Smoke;
    private Enemy m_Target;
    private Turret m_Turret;
    private float m_LastFire;
    [SerializeField]
    private Transform m_CanonTip;
    [SerializeField]
    private AudioClip m_Firesound;
    private AudioSource m_AS;
    private LineRenderer m_Laser;

    private float m_LazerCooldown = 0.05f;

    private void Awake()
    {
       
        m_AS = GetComponent<AudioSource>();
        m_Laser = GetComponent<LineRenderer>();
        m_Turret = GetComponent<Turret>();
        m_Laser.enabled = false;
    }

    private void Update()
    {
        if (m_Laser.enabled && Time.time >= m_LastFire + m_LazerCooldown)
        {
            m_Laser.enabled = false;
        }
        // Out of range?
        if (m_Target && Vector3.Distance(m_Target.transform.position, transform.position) > Range)
        {
            m_Target = null;
        }
        // No Target? Find one
        if (!m_Target)
        {
            m_Target = FindTarget();
        }
        // Do stuff when there is a target
        if (m_Target)
        {
            transform.up = m_Target.transform.position - transform.position;
            if (Time.time >= m_LastFire + Cooldown)
            {
                Fire();
                m_AS.PlayOneShot(m_Firesound);
            }

        }
    }
    private void Fire()
    {
        m_LastFire = Time.time;
        m_Target.GetComponent<Enemy>().TakeDamage(Damage);

        m_Laser.SetPositions(new Vector3[] { m_CanonTip.position, m_Target.GetComponent<Enemy>().transform.position });
        m_Laser.enabled = true;
        Smoke.Play();
    }
    private Enemy FindTarget()
    {
        var t_Enemies = GameObject.FindObjectsOfType<Enemy>();

        Enemy t_ClosestEnemy = null;
        float t_ClosestDistance = Range * Range;

        foreach (var t_Enemy in t_Enemies)
        {
            float t_Distance = (t_Enemy.transform.position - transform.position).sqrMagnitude;
            if (t_Distance < t_ClosestDistance)
            {
                t_ClosestEnemy = t_Enemy;
                t_ClosestDistance = t_Distance;
            }
        }
        return t_ClosestEnemy;
    }
}
