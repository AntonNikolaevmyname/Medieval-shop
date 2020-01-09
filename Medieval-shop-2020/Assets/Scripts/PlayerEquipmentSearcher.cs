using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TO DO:
    1. Баг: когда оружие в руках сталкивается с чем-то, то оно улетает вверх. PS вроде исправил, надо тестить.
    2. Проблемы с названиями методов.
    3. Переделать вращение колесиком мыши на вращение мышью. 
*/

public class PlayerEquipmentSearcher : MonoBehaviour
{
    public static PlayerEquipmentSearcher instance = null;
    // Рисует raycast для поиска оружия.
    [SerializeField]
    private bool debug;
    private bool isMouseUp = false;

    [SerializeField]
    private Transform equipParent;
    private Transform hitComponent = null;
    // Углы кручения оружия.
    // C колесиком мыши крутится только по Y.
    private float hc_xRotate = 0f;
    private float hc_yRotate = 0f;
    private float hc_zRotate = 0f;

    private int layerMask;
    private float maxDistanceDrawRay = 10f;
    private float equipRotationSpeed = 10f;

    public Transform HitComponent
    {
        get
        {
            return hitComponent;
        }
    }
    private void Start()
    {
        if(instance == null)
        {
           instance = this;
        }
        else
        {
            print($"Warning:{instance} is unknown object.");
        }
        // Ищем с помощью raycast только по маске "оружия".
        layerMask = LayerMask.GetMask("Equipment");
    }

    private void Update()
    {
        PlayerDebug(debug);
        SearchEquip();
    }

    private void OnGUI()
    {
        if(hitComponent == null) 
            return;
              
        GUI.TextField(new Rect(10, 10, 150, 100), hitComponent.name);
    }
    private bool GetRaycastHit (out RaycastHit hit, float fixed_y)
    {
       return Physics.Raycast(new Vector3(transform.position.x, 
                              transform.position.y + fixed_y,
                              transform.position.z), 
                              transform.TransformDirection(Vector3.forward), 
                              out hit, maxDistanceDrawRay, layerMask);
    }

    // Пытаемся найти оружие возле себя.
    private void AttemptGetEquipment()
    {
        RaycastHit[] hit = new RaycastHit[3];
        bool canEquip0 = GetRaycastHit (out hit[0], 0f);
        bool canEquip1 = GetRaycastHit (out hit[1], 0.5f);
        bool canEquip2 = GetRaycastHit (out hit[2], 1f);
        
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

        //hc_xRotate += Input.mouseScrollDelta.x * equipRotationSpeed;
        // Крутится только по оси Y.
        hc_yRotate += Input.mouseScrollDelta.y * equipRotationSpeed; 
        hitComponent.rotation = Quaternion.Euler(hc_xRotate, hc_yRotate, hc_zRotate);
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
