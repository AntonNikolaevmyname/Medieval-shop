using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteApp
{
    internal sealed class UserConsole : MonoBehaviour
    {
        public static UserConsole Instance{get; private set;} 

        private string _userConsoleText;
        public string UserConsoleText
        {
            get => _userConsoleText;
            // string.Empty не равно null
            set
            {
                if(value != null)
                    _userConsoleText = value;
                else 
                    print($"Нельзя задать null для UserConsoleText");
            }
        }

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
            UserConsoleText = string.Empty;
        }

        private void OnGUI()
        {
            // Чеканные монеты игрока.
            GUI.TextField(new Rect(10, 10, 200, 50), $"Money: {GameManager.Instance.Player.Money}");
            if(UserConsoleText == string.Empty)
                return;

            // Пользовательская консоль.
            GUI.TextField(new Rect(10, 100, 200, 150), UserConsoleText);
        }

        public void PrintIntoUserConsoleEquipInfo(DefaultEquipObject deo, KeyCode action)
        {
            if(deo == null)
            {
                print($"Нельзя вывести информацию о DefaultEquipObject равном null");
                return;
            }

            string pressAction = $"Нажмите {action} для покупки";
            UserConsoleText = $"Name:{deo.Name}\n"+
                    $"Goldcost:{deo.GoldCost}\n"+
                    $"Damage:{deo.Damage}\n"+
                    $"AttackType:{deo.AttackType}\n"+
                    $"Description:{deo.Description}\n"+
                    $"Weight:{deo.Weight}\n\n\n"+
                    $"*****{pressAction}*****";
        }
    }
}