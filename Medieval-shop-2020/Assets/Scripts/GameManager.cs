using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CompleteApp
{
    internal sealed class GameManager : MonoBehaviour, INotifyPropertyChanged
    {
        public static GameManager Instance { get; private set; } 

        public event Action<Transform> OnLookAtAnyInteractObject = delegate{};
        public event PropertyChangedEventHandler PropertyChanged = delegate{}; // Обновление данных для UI.

        public Transform InteractObject { get; set; }
        // Текущий id объекта взаимодействия.
        public string InteractObjectId { get; private set;}
        // { Id : InteractObject }
        public Dictionary<string , InteractObjectDefault> IDInteractObjectsDict { get; private set;}

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

            // Инициализация.
            var collection = FindAllScriptableObjects<InteractObjectDefault>("ScriptableObjects");
            print(collection.Length);
            print(collection.GetType());
            // Храним данные в словаре для доступа по ключу к объектам.
            foreach (var item in collection)
            {
                IDInteractObjectsDict.Add(item.Id, item);
            }
        }

        private void Update()
        {

        }


        public void InteractWithHitObject<T>(T hit) where T: Transform
        {
            if(hit == null)
            {
                InteractObject = null;
                NotifyPropertyChanged(null);
                return;
            }
            
            if(hit.Equals(InteractObject))
            return;

            InteractObject = hit;
            OnLookAtAnyInteractObject(InteractObject);
        }

        public void SetInteractObjectID(string id)
        {
            if(id == null)
            return;

            InteractObjectId = id;
            // Сообщить UIManager'y для вывода инфы.
            NotifyPropertyChanged();
        }
        
        public InteractObjectDefault GetCurrentInteractObject()
        {
            return IDInteractObjectsDict[InteractObjectId];
        }

        private T[] FindAllScriptableObjects<T>(string path)
        {           
            return Resources.LoadAll(path, typeof(T)).Cast<T>().ToArray();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")  
        {  
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
    }
}
