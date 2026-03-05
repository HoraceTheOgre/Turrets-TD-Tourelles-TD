using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject Item_Prefab;

    public float DelayMin = 7;
    public float DelayMax = 20;

    private GameObject Item_Placed;
    private float m_Timer;
    private float m_Delay;
    private bool m_Active = false;
    void Start()
    {
        if (Random.Range(0, 2) != 0)
        {
            m_Active = true;
        }
        Item_Placed = null;
        m_Delay = Random.Range(DelayMin, DelayMax);
    }
    void Update()
    {
        if (m_Active)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Delay)
            {
                if (Item_Placed == null)
                {
                    Item_Placed = Spawn();
                }


                m_Timer = 0;
                m_Delay = Random.Range(DelayMin, DelayMax);
            }
        }
        
    }
    private GameObject Spawn()
    {
        GameObject t_Item = Instantiate(Item_Prefab, transform.position, Quaternion.identity);
        return t_Item;
    }
}
