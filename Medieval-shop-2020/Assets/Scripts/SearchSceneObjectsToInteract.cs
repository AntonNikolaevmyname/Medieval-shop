using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
В этом скрипте происходит поиск объектов на сцене через рисование и пересечение raycast'ов
с объектами, у которых Layer выставлен Equipment.
После нахождения объекта взаимодействия все дальнейшее управление переходит в GameManager.
*/
public class SearchSceneObjectsToInteract : MonoBehaviour
{
    private const int _hitsCount = 5;                   // Количество испускаемых лучей.
    private const float _distanceBetweenRays = 0.4f;    // Расстояние по Y между лучами Raycast'ов.
    private const float _drawRayDistance = 10f;         // Дальность прорисовки лучей.

    private Camera _mainCamera;
    private Transform _hitComponent = null;
    // Маска предметов для взаимодействия, остальные лучи игнорируют.
    private int _layerMask;
    private float _maxDistanceDrawRay = 10f;    

    private void Awake()
    {
        // Инициализация.
        _layerMask = LayerMask.GetMask("Equipment");
        _mainCamera = Camera.main;
    }

    private void Start()
    {

    }

    private void Update()
    {
        TryingToFindObjectsToInteractWith();
    }

    // Поиск объектов для взаимодействия.
    private void TryingToFindObjectsToInteractWith()
    {
        RaycastHit[] _hits  = new RaycastHit[_hitsCount];

        for(int numberRay = 0; numberRay < _hitsCount; numberRay++)
        {
            MakeRaycast(out _hits[numberRay], numberRay);
            if(_hits[numberRay].transform != null)
            {   
                print($"Нашли объект {_hits[numberRay].transform.name}");
                
                _hitComponent = _hits[numberRay].transform.GetComponent <Transform> (); 
                InteractWithHitObject();
                break;
            }
        }
    }

    // Когда нашли объект для взаимодействия, то передаем управление в GameManager.
    private void InteractWithHitObject()
    {
        GameManager.instance.InteractWithHitObject(_hitComponent);
    }

    // Метод рисования 1 луча.
    private void MakeRaycast(out RaycastHit hit, int numberRay)
    {   
        Vector3 direction = new Vector3(Camera.main.transform.forward.x, Camera.main.transform.forward.y, Vector3.forward.z);

        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), 
            out hit, _drawRayDistance, _layerMask))
        {
            Debug.DrawRay(
                new Vector3(Camera.main.transform.position.x, 
                            Camera.main.transform.position.y + _distanceBetweenRays * numberRay,
                            Camera.main.transform.position.z), 
                transform.TransformDirection(direction) * _drawRayDistance, 
                Color.yellow
                );
        }
        else
        {
            Debug.DrawRay(
                new Vector3(Camera.main.transform.position.x, 
                            Camera.main.transform.position.y + _distanceBetweenRays * numberRay,
                            Camera.main.transform.position.z), 
                transform.TransformDirection(direction) * _drawRayDistance, 
                Color.white
            );
        }
    }
}
