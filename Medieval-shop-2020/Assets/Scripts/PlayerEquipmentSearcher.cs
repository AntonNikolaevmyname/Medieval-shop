using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEquipmentSearcher : MonoBehaviour
{
    // Рисует raycast для поиска оружия.
    public bool debug;
    private bool isMouseUp = false;

    [SerializeField]
    private Transform equipParent;
    private Transform hitComponent = null;
    private int layerMask;
    private float maxDistanceDrawRay = 10f;
    private void OnEnable()
    {
        layerMask = LayerMask.GetMask("Equipment");
    }
    private void Update()
    {
        PlayerDebug(debug);
        OnMouseOver();
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
                hitComponent.transform.SetParent(equipParent);
                hitComponent.transform.localPosition = Vector3.zero;
                hitComponent.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
    
    // TO DO: Переписать на кнопку по выбору.
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
                hitComponent.GetComponent<Rigidbody>().useGravity = true;
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
