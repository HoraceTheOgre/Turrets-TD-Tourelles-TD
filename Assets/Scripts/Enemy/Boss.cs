using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Boss : MonoBehaviour
{
    [Header("Melee Attack")]
    public float MeleeRange    = 2f;
    public float MeleeDamage   = 5f;
    public float MeleeCooldown = 2f;

    [Header("References")]
    public GameObject  Player;
    public Animator    BossAnimator;

    [Header("Audio")]
    public AudioSource BossAudioSource;
    public AudioClip   SFX_Attack;
    public AudioClip   SFX_Death;

    // Match these to your Animator Controller bool parameter names
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_DEATH  = "Death";

    private float     m_LastMelee;
    private bool      m_IsDead = false;
    private Enemy     m_Enemy;
    private Transform m_PlayerTF;

    private void Awake()
    {
        m_Enemy    = GetComponent<Enemy>();
        m_PlayerTF = Player != null ? Player.transform : null;
    }

    private void Update()
    {
        if (m_IsDead || m_PlayerTF == null) return;

        // Sync Player ref from Enemy component in case it was assigned there
        if (m_PlayerTF == null && m_Enemy.Player != null)
            m_PlayerTF = m_Enemy.Player.transform;

        float dist = Vector3.Distance(transform.position, m_PlayerTF.position);

        if (dist <= MeleeRange && Time.time - m_LastMelee >= MeleeCooldown)
            StartCoroutine(MeleeAttack());
    }

    private IEnumerator MeleeAttack()
    {
        m_LastMelee = Time.time;

        if (BossAnimator) BossAnimator.SetBool(ANIM_ATTACK, true);
        if (BossAudioSource && SFX_Attack) BossAudioSource.PlayOneShot(SFX_Attack);

        // Wait for swing — match this to your attack animation length
        yield return new WaitForSeconds(0.4f);

        if (Vector3.Distance(transform.position, m_PlayerTF.position) <= MeleeRange + 0.5f)
            Player.GetComponent<Player>().TakeDamage(MeleeDamage);

        yield return new WaitForSeconds(0.2f);
        if (BossAnimator) BossAnimator.SetBool(ANIM_ATTACK, false);
    }

    // Call this to damage the boss
    public void TakeDamage(float damage)
    {
        if (m_IsDead) return;

        m_Enemy.TakeDamage(damage);

        if (m_Enemy.Health <= 0f)
            Die();
    }

    private void Die()
    {
        if (m_IsDead) return;
        m_IsDead = true;

        StopAllCoroutines();

        if (BossAnimator)
        {
            BossAnimator.SetBool(ANIM_ATTACK, false);
            BossAnimator.SetBool(ANIM_DEATH,  true);
        }

        if (BossAudioSource && SFX_Death) BossAudioSource.PlayOneShot(SFX_Death);

        m_Enemy.enabled = false;

        // Match this delay to your death animation length
        Destroy(gameObject, 1.5f);
    }
}