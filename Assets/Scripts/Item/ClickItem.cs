using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class ClickItem : MonoBehaviour
{
    public int Healing = 1;
    public int Points = 10;

    public GameObject Prefab_Item;

    private GameObject target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetClickedObject())
            {
                target = GetClickedObject();
                if (target.CompareTag(Prefab_Item.tag))
                {
                    print("clicked/touched!");
                    gameObject.GetComponent<Player>().TakeDamage(-1 * Healing);
                    gameObject.GetComponent<Player>().AddPoints(Points);
                    Destroy(target);
                }

            }
            
        }
    }

    GameObject GetClickedObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics2D.Raycast(ray.origin, ray.direction * 10))
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction * 10);
            return raycastHit2D.collider.gameObject;
        }
        else
        {
            return null;
        }




    }
}

