using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public GameObject Icon;
    public float MaxHP = 10f;
    private float HP;
    private int m_Points = 0;
    public AudioSource MUSIC_AS;
    public AudioSource SFX_AS;
    public AudioClip HitSFX;
    public AudioClip FX_WIN;
    public AudioClip FX_LOSE;
    private void Start()
    {
        HP = MaxHP;
    }
    private void Update()
    {
        // Damage Tester
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
        }

    }
    public void ScaleHP(float new_HP)
    {
        MaxHP = new_HP;
        HP = new_HP;
        MenuManager.Instance.HUD.DiplayHP(HP);
    }
    public void TakeDamage(float a_Damage)
    {
        HP -= a_Damage;
        if (HP >= MaxHP) 
        {
            HP = MaxHP; 
        }
        if (a_Damage >= 0) 
        {
            SFX_AS.Stop();
            SFX_AS.PlayOneShot(HitSFX);
            Icon.GetComponent<Icon>().HurtAnim();
            
        }
        MenuManager.Instance.HUD.DiplayHP(HP);
        if (HP <= 0)
        {
            Die();
        }
    }
    public void AddPoints(int a_Points)
    {
        m_Points += a_Points;
        MenuManager.Instance.HUD.DiplayPTS(m_Points);
    }
    private void Die()
    {
        MUSIC_AS.Stop();
        SFX_AS.loop = true;
        SFX_AS.PlayOneShot(FX_LOSE);
        MenuManager.Instance.END.gameObject.SetActive(!MenuManager.Instance.END.gameObject.activeSelf);
        MenuManager.Instance.END.DiplayEndStatus("YOU DIED");
        Time.timeScale = 0;
    }
    public void Win()
    {
        MUSIC_AS.Stop();
        SFX_AS.loop = true;
        SFX_AS.PlayOneShot(FX_WIN);
        MenuManager.Instance.END.gameObject.SetActive(!MenuManager.Instance.END.gameObject.activeSelf);
        MenuManager.Instance.END.DiplayEndStatus("YOU LIVED");
        Time.timeScale = 0;
    }
}
