using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float Range    = 3f;
    public float Damage   = 1f;
    public float Cooldown = 1f;

    public ParticleSystem Smoke;

    [SerializeField] private Transform  m_CanonTip;
    [SerializeField] private AudioClip  m_Firesound;
    [SerializeField] private GameObject SelectionHighlight;

    private AudioSource  m_AS;
    private LineRenderer m_Laser;
    private Enemy        m_Target;

    private float m_LastFire;
    private float m_LazerCooldown = 0.05f;

    [HideInInspector] public bool IsControlled = false;

    private void Awake()
    {
        m_AS    = GetComponent<AudioSource>();
        m_Laser = GetComponent<LineRenderer>();
        m_Laser.enabled = false;

        if (SelectionHighlight) SelectionHighlight.SetActive(false);
    }

    private void Update()
    {
        if (m_Laser.enabled && Time.time >= m_LastFire + m_LazerCooldown)
            m_Laser.enabled = false;

        if (IsControlled)
            HandleManualControl();
        else
            HandleAutoControl();
    }

    private void HandleAutoControl()
    {
        if (m_Target && Vector3.Distance(m_Target.transform.position, transform.position) > Range)
            m_Target = null;

        if (!m_Target)
            m_Target = FindTarget();

        if (m_Target)
        {
            transform.up = m_Target.transform.position - transform.position;

            if (Time.time >= m_LastFire + Cooldown)
            {
                Fire(m_Target.transform.position);
                m_AS.PlayOneShot(m_Firesound);
            }
        }
    }

    private void HandleManualControl()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        transform.up = mouseWorld - transform.position;

        if (Input.GetMouseButtonDown(1) && Time.time >= m_LastFire + Cooldown)
        {
            Fire(mouseWorld);
            m_AS.PlayOneShot(m_Firesound);
        }
    }

    private void Fire(Vector3 targetPos)
    {
        m_LastFire = Time.time;

        Vector2 dir      = (targetPos - m_CanonTip.position).normalized;
        var     hit      = Physics2D.Raycast(m_CanonTip.position, dir, Range);
        Vector3 endPoint = targetPos;

        if (hit.collider != null)
        {
            Enemy e = hit.collider.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(Damage);
                endPoint = hit.collider.transform.position;
            }
        }

        m_Laser.SetPositions(new Vector3[] { m_CanonTip.position, endPoint });
        m_Laser.enabled = true;
        Smoke.Play();
    }

    public void SetControlled(bool controlled)
    {
        IsControlled = controlled;
        m_Target     = null;
        if (SelectionHighlight) SelectionHighlight.SetActive(controlled);
    }

    private Enemy FindTarget()
    {
        var   enemies        = FindObjectsOfType<Enemy>();
        Enemy closestEnemy   = null;
        float closestDistSqr = Range * Range;

        foreach (var e in enemies)
        {
            float d = (e.transform.position - transform.position).sqrMagnitude;
            if (d < closestDistSqr)
            {
                closestEnemy   = e;
                closestDistSqr = d;
            }
        }

        return closestEnemy;
    }
}