using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CompleteApp
{
    internal sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 

        public event Action<Transform> OnLookAtAnyInteractObject = delegate{};      

        public Transform InteractObject { get; set; }
        public string InteractObjectId { get; private set;}

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                print($"Warning:{Instance} is unknown object.");
            }
            //List<InteractObjectDefault> a = FindAllScriptableObject<InteractObjectDefault>();
        }

        private void Update()
        {

        }


        public void InteractWithHitObject<T>(T hit) where T: Transform
        {
            if(hit == null)
            {
                InteractObject = null;
                return;
            }
            
            if(hit.Equals(InteractObject))
            return;

            InteractObject = hit;
            OnLookAtAnyInteractObject(InteractObject);
        }

        public void GetInteractObjectID(string id)
        {
            if(id == null)
            return;

            InteractObjectId = id;
            // Сообщить UIManager'y для вывода инфы.
        }

        private List<T> FindAllScriptableObject<T>()
        {           
            return Resources.LoadAll("ScriptableObjectes", typeof(T)).Cast<T>().ToList();
        }
    }
}
