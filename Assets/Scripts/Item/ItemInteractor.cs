using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteractor : MonoBehaviour
{
    public int        Healing     = 1;
    public int        Points      = 10;
    public GameObject Prefab_Item;

    private GameObject m_Target;
    private Turret     m_ControlledTurret = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Target = GetClickedObject();

            if (m_Target == null)
            {
                ReleaseTurret();
                return;
            }

            // Clicked a Turret → take manual control
            Turret clickedTurret = m_Target.GetComponentInParent<Turret>();
            if (clickedTurret != null)
            {
                if (m_ControlledTurret != null && m_ControlledTurret != clickedTurret)
                    m_ControlledTurret.SetControlled(false);

                if (m_ControlledTurret == clickedTurret)
                    ReleaseTurret();
                else
                {
                    m_ControlledTurret = clickedTurret;
                    m_ControlledTurret.SetControlled(true);
                }

                return;
            }

            // Clicked a pickup item
            if (Prefab_Item != null && m_Target.CompareTag(Prefab_Item.tag))
            {
                print("clicked/touched!");
                GetComponent<Player>().TakeDamage(-1 * Healing);
                GetComponent<Player>().AddPoints(Points);
                Destroy(m_Target);
            }
        }
    }

    private void ReleaseTurret()
    {
        if (m_ControlledTurret != null)
        {
            m_ControlledTurret.SetControlled(false);
            m_ControlledTurret = null;
        }
    }

    private GameObject GetClickedObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction * 10);
        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        return null;
    }
}