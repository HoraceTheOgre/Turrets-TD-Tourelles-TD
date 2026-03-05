using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_UIGroup;
    [SerializeField]
    private bool fadeIn = false;


    void Start()
    {
        Invoke("ShowUI", 6.7f);
    }
    public void Update()
    {
        if (fadeIn)
        {
            if (m_UIGroup.alpha < 1)
            {
                m_UIGroup.alpha += Time.deltaTime;
                if (m_UIGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

    }
    public void ShowUI()
    {
        fadeIn = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }

}