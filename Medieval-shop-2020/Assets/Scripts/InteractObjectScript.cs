using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteApp
{
    internal sealed class InteractObjectScript : MonoBehaviour
    {
        [SerializeField]
        private string id;

        private void Start()
        {
            
            GameManager.Instance.OnLookAtAnyInteractObject += OnLookAtThisInteractObject;
        }

        private void OnLookAtThisInteractObject(Transform t)
        {
            if(t.Equals(this.gameObject.transform))
                GameManager.Instance.GetInteractObjectID(id);
        }
    }
}

