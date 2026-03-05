using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    private Animator m_Animator;
    public AudioSource AS;
    [SerializeField]
    private AudioClip[] m_SFXs;

    private void Awake()
    {
        m_Animator = this.gameObject.GetComponent<Animator>();
    }
    public void UpgradeAnim()
    {
        m_Animator.SetTrigger("Upgrade");
        AS.PlayOneShot(m_SFXs[0]);
    }
    public void HurtAnim()
    {
        m_Animator.SetTrigger("Hurt");
        AS.PlayOneShot(m_SFXs[1]);
    }
}
