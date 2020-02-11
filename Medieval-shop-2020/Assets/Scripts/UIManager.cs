using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteApp
{
    internal sealed class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Text text;
        void Start()
        {
            GameManager.Instance.PropertyChanged += UIUpdated;
        }

        void UIUpdated(object sender, PropertyChangedEventArgs eventArgs)
        {
            if(eventArgs.PropertyName == null)
            {
                text.text = "";
                return;
            }
            text.text = GameManager.Instance.GetCurrentInteractObject().Name;
        }
    }
}
