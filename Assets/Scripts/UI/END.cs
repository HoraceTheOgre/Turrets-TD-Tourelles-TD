using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class END : MonoBehaviour
{
    //begin at 1
    public float CurrentWave = 1;
    [SerializeField]
    private TextMeshProUGUI m_TextStatus;
    [SerializeField]
    private TextMeshProUGUI m_TextPTSend;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        
    }
    public void TryAgain()
    {
        SceneManager.LoadScene("Level " + CurrentWave, LoadSceneMode.Single);
    }
    public void DiplayEndStatus(string a_EndStatus)
    {
        m_TextStatus.text = a_EndStatus;
    }
}
