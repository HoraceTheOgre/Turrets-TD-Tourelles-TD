using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip Hover;
    public AudioClip Click;
    private AudioSource m_AS;
    private void Awake()
    {
        m_AS = GetComponent<AudioSource>();
    }
    public void PlayHover()
    {
        m_AS.PlayOneShot(Hover);
    }
    public void PlayClick()
    {
        m_AS.PlayOneShot(Click);
    }

}
