using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public int NextWave = 0;
    public GameObject Icon;
    public AudioClip UpgradeSFX;
    public AudioSource AS;
    [SerializeField]
    private TextMeshProUGUI m_TextHP;
    [SerializeField]
    private TextMeshProUGUI m_TextPTS;
    [SerializeField]
    private TextMeshProUGUI m_TextWave;
    [SerializeField]
    private TextMeshProUGUI m_TextStatus;

    public void DiplayHP(float a_HP)
    {
        int t_HP = (int)a_HP;
        m_TextHP.text = t_HP.ToString();
    }
    public void DiplayPTS(int a_PTS)
    {
        m_TextPTS.text = "PTS : " + a_PTS.ToString();
    }
    public void DiplayWave(int a_Wave)
    {
        m_TextWave.text = a_Wave.ToString();
    }
    public void DiplayStatus(string a_Status)
    {
        m_TextStatus.text = a_Status.ToString();
    }
    public void StartWave()
    {
        if (NextWave == 2)
        {
            SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
        } else if (NextWave == 3)
        {
            SceneManager.LoadScene("Level 3", LoadSceneMode.Single);
        } else if (NextWave == 4)
        {
            SceneManager.LoadScene("Level 4", LoadSceneMode.Single);
        } 
    }
    public void Upgrade()
    {
        Icon.GetComponent<Icon>().UpgradeAnim();
        // Needs Interval
        AS.Stop();
        AS.PlayOneShot(UpgradeSFX);

        if (this.gameObject.CompareTag("SOLDIER"))
        {
            Debug.Log("SOLDIER");
        }
        else if (this.gameObject.CompareTag("SMG"))
        {
            Debug.Log("SMG");
        }
        else if (this.gameObject.CompareTag("SNIPER"))
        {
            Debug.Log("SNIPER");
        }

    }

}
