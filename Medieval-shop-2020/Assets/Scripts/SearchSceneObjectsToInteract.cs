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
        private readonly float _drawRayDistance = 5f;         // Дальность прорисовки луча.
              
        private Camera _mainCamera;
        // Маска предметов для взаимодействия, остальные лучи игнорируют.
        private int _layerMask;

        public Transform HitComponent { get;set; }

        private void Awake()
        {
            string lm = "InteractObject";
            // Инициализация.
            _layerMask = LayerMask.GetMask(lm);
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            TryingToFindObjectsToInteractWith();
        }

        // Поиск объектов для взаимодействия.
        private void TryingToFindObjectsToInteractWith()
        {
            RaycastHit hit;
            MakeRaycast(out hit);

            // Если луч "нашел" нужный объект.
            if(hit.transform != null)
            {   
                // Когда нашли объект для взаимодействия, то передаем управление в GameManager.
                HitComponent = hit.transform.GetComponent <Transform> (); 
                GameManager.Instance.InteractWithHitObject<Transform>(HitComponent);
                return;
            }

            GameManager.Instance.InteractWithHitObject<Transform>(null);
        }

        
        private void MakeRaycast(out RaycastHit hit)
        {   
            // Направление лучей должно совпадать со взгядом игрока через камеру.
            Vector3 direction = Camera.main.transform.forward; 

            if (Physics.Raycast(Camera.main.transform.position, direction,
                out hit, _drawRayDistance, _layerMask))
            {
                #if UNITY_EDITOR
                DebugDrawRayInScene(direction, Color.red);
                #endif
            }
            else
            {
                #if UNITY_EDITOR
                DebugDrawRayInScene(direction, Color.white);
                #endif
            }
        }

        private void DebugDrawRayInScene(Vector3 direction, Color color)
        {
            Debug.DrawRay(
                new Vector3(Camera.main.transform.position.x, 
                            Camera.main.transform.position.y,
                            Camera.main.transform.position.z), 
                direction * _drawRayDistance, 
                color
            );
        }
    }
}