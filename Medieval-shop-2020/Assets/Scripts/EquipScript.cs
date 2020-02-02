using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteApp
{
    sealed internal class EquipScript : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.OnLookAtAnyEquip += onLookAtAnyEquip;
        }

        private void onLookAtAnyEquip(Transform t)
        {
            UserConsole.Instance.PrintIntoUserConsoleEquipInfo(t);
            // Активация шейдера(подсветки) при наведении raycast'ов на объект (оружие).
            // ...
        }
    }
}