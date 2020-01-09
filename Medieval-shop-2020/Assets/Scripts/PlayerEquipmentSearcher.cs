using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TO DO:
    Баг: когда оружие в руках сталкивается с чем-то, то оно улетает вверх. PS вроде исправил, надо тестить.
    Проблемы с названиями методов.
*/

public class PlayerEquipmentSearcher : MonoBehaviour
{
    // Рисует raycast для поиска оружия.
    public bool debug;
    private bool isMouseUp = false;

    [SerializeField]
    private Transform equipParent;
    private Transform hitComponent = null;
    float hc_xRotate = 0;
    float hc_yRotate = 0;
    private int layerMask;
    private float maxDistanceDrawRay = 10f;
    private void OnEnable()
    {
        // Ищем с помощью raycast только по маске "оружия".
        layerMask = LayerMask.GetMask("Equipment");
    }

    private void Update()
    {
        PlayerDebug(debug);
        SearchEquip();
    }

    private bool Equip (out RaycastHit hit, float fixed_y)
    {
       return Physics.Raycast(new Vector3(transform.position.x, 
                              transform.position.y + fixed_y,
                              transform.position.z), 
                              transform.TransformDirection(Vector3.forward), 
                              out hit, maxDistanceDrawRay, layerMask);
    }
    private void AttemptGetEquipment()
    {
        RaycastHit[] hit = new RaycastHit[3];
        bool canEquip0 = Equip (out hit[0], 0f);
        bool canEquip1 = Equip (out hit[1], 0.5f);
        bool canEquip2 = Equip (out hit[2], 1f);
        
        if( hit[0].transform == null && 
            hit[1].transform == null && 
            hit[2].transform == null)
            return;
        
        for(int i = 0; i < hit.Length; i++)
        {
            if(hit[i].transform != null)
            {
                hitComponent = hit[i].transform.GetComponent <Transform> (); 
                break;
            }
        }

        if((canEquip0 || canEquip1 || canEquip2) && isMouseUp && hitComponent != null)
        {
            if(equipParent.childCount == 0)
            {
                var hc_rigibody = hitComponent.GetComponent<Rigidbody>();

                hitComponent.transform.SetParent(equipParent);
                hitComponent.transform.localPosition = Vector3.zero;
                hc_rigibody.drag = 10;
                hc_rigibody.constraints = RigidbodyConstraints.FreezePositionY;
                hc_rigibody.useGravity = false;
            }
        }
    }
    private void SearchEquip()
    {
        RotateEquip();
        OnMouseOver();
    }

    // Дает возможность вращать оружие, если оно в "руках".
    private void RotateEquip()
    {
        // Если ничего в руках нет.
        if(hitComponent == null) 
            return;
        // Или колесо мыши не крутится.
        if(Input.mouseScrollDelta == Vector2.zero) 
            return;

        hc_xRotate += Input.mouseScrollDelta.x * 10;
        hc_yRotate += Input.mouseScrollDelta.y * 10;
        hitComponent.rotation = Quaternion.Euler(hc_xRotate, hc_yRotate, 0);
    }

    // TO DO: Переписать на кнопку по выбору.
    // Полностью переписать с событиями, чтобы без бесконечных if было...
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isMouseUp = true;
            AttemptGetEquipment();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isMouseUp = false;
            if(hitComponent != null)
            {
                var hc_rigibody = hitComponent.GetComponent<Rigidbody>();
                hc_rigibody.useGravity = true;
                hc_rigibody.drag = 1;
                hc_rigibody.constraints = RigidbodyConstraints.None;
                hitComponent = null;
            }
            equipParent.DetachChildren();
        }
    }
    private void PlayerDebug(bool debug)
    {
        if(!debug) return;

        float fixed_y = 0;
        for(int i = 0; i < 3;i++)
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fixed_y,
                                  transform.position.z), 
                                  transform.TransformDirection(Vector3.forward) * maxDistanceDrawRay, 
                                  Color.yellow);
            fixed_y += 0.5f;
        }
    }
}
