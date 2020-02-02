using UnityEngine;
using System.Collections.Generic;

namespace CompleteApp
{
    sealed internal class Params : MonoBehaviour
    {
        public static Params Instance{get; private set;}
        public  KeyCode ActionButton {get; set;} = KeyCode.E;
        public  KeyCode InventoryButton {get;set;} = KeyCode.Q;
        public int PlayerMoney {get;set;} = 10000;
        public string EquipLayerMask {get;set;} = "Equipment";
        public List<DefaultEquipObject> defaultEquipObject;

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
        }
    }
}

