using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
В этом скрипте происходит поиск объектов на сцене через рисование и пересечение raycast'ов
с объектами, у которых Layer выставлен Equipment.
После нахождения объекта взаимодействия все дальнейшее управление переходит в GameManager.
*/
namespace CompleteApp
{
    internal sealed class SearchSceneObjectsToInteract : MonoBehaviour
    {  
        private const int _hitsCount = 3;                   // Количество испускаемых лучей.
        private const float _distanceBetweenRays = 0.5f;    // Расстояние по Y между лучами Raycast'ов.
        private const float _drawRayDistance = 5f;         // Дальность прорисовки лучей.
              
        private Camera _mainCamera;
        private Transform _hitComponent = null;
        // Маска предметов для взаимодействия, остальные лучи игнорируют.
        private int _layerMask;
        private float _maxDistanceDrawRay = 10f;    

        private void Start()
        {
            string lmn = GameManager.Instance.LayerMaskName;
            
            // Инициализация.
            _layerMask = LayerMask.GetMask(lmn);
            _mainCamera = Camera.main;
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
                // Если любой из лучей "нашел" нужный объект.
                if(_hits[numberRay].transform != null)
                {   
                    _hitComponent = _hits[numberRay].transform.GetComponent <Transform> (); 
                    InteractWithHitObject();
                    return;
                }
            }

            GameManager.Instance.HitComponentIsNull();
        }

        // Когда нашли объект для взаимодействия, то передаем управление в GameManager.
        private void InteractWithHitObject()
        {
            GameManager.Instance.InteractWithHitObject<Transform>(_hitComponent);
        }

        // Метод рисования 1 луча.
        private void MakeRaycast(out RaycastHit hit, int numberRay)
        {   
            // Направление лучей должно совпадать со взгядом игрока через камеру.
            Vector3 direction = Camera.main.transform.forward; 

            if (Physics.Raycast(Camera.main.transform.position, direction,
                out hit, _drawRayDistance, _layerMask))
            {
                DebugDrawRayInScene(direction, numberRay, Color.yellow);
            }
            else
            {
                DebugDrawRayInScene(direction, numberRay, Color.white);
            }
        }

        private void DebugDrawRayInScene(Vector3 direction, int numberRay, Color color)
        {
            if(GameManager.Instance.Debug == false)
                    return;

            Debug.DrawRay(
                new Vector3(Camera.main.transform.position.x, 
                            Camera.main.transform.position.y + _distanceBetweenRays * numberRay,
                            Camera.main.transform.position.z), 
                direction * _drawRayDistance, 
                color
            );
        }
    }
}