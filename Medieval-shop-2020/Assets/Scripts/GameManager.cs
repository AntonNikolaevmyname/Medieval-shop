using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    TO DO: оптимизировать механизм поиска информации о текущем hitComponet из ScriptableObject'ов.
*/

namespace CompleteApp
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private Transform _hitComponent = null;

        // Режим отладки, рисует лучи, вывод дополнительной информации в консоль.
        [SerializeField]
        [Tooltip("Режим отладки")]
        private bool _debug;
        public bool Debug{get => _debug;}

        [SerializeField]
        [Tooltip("Имя маски слоя объектов для взаимодействия")]
        private string _layerMaskName = "Equipment"; 
        public string LayerMaskName{get => _layerMaskName;}

        [SerializeField]
        [Tooltip("Все ScriptableObject'ы должны быть здесь")]
        private DefaultEquipObject[]  _defaultEquipObjects;
        private string _userConsole;
        public string UserConsole
        {
            get => _userConsole;
            set
            {
                if(value != null)
                    _userConsole = value;
            }
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                print($"Warning:{instance} is unknown object.");
            }
        }

        private void Start()
        {
            _userConsole = string.Empty;
        }

        private void OnGUI()
        {
            if(_userConsole == string.Empty)
                return;

            // Пользовательская консоль.
            GUI.TextField(new Rect(10, 10, 200, 150), _userConsole);
        }
        private void Update()
        {
            if(_hitComponent == null)
            {
                _userConsole = string.Empty;
                return;
            }

            PurchaseEquip();
        }

        // Взаимодействие с объектом:
        // 1. Он подсвечивается.
        // 2. В консоль выводится информация о нем из ScriptableObject по его имени.
        // 3. Если нажать кнопку "Купить" (на клавиатуре),       -> в Update()
        // (*4.) то он добавится в инвентарь.                    -> в Update()
        public void InteractWithHitObject<T>(T hit) where T: Transform
        {
            if(hit == null)
            {
                print($"Class GameManager. Method 'InteractWithHitObject'. Exception: hit is null");
                return;
            }

            _hitComponent = hit;
            ActivatingShadersOnHover();
            UserConsoleOutput();
        }
        // Если raycast'ы ничего не нашли.
        public void HitComponentIsNull()
        {
            _hitComponent = null;
        }
        // Активация шейдера(подсветки) при наведении raycast'ов на объект (оружие).
        private void ActivatingShadersOnHover()
        {

        }
        // Вывод информации в польз.консоль о _hitComponent.
        // TO DO: Максимально неоптимизированный поиск, надо переделать.
        private void UserConsoleOutput()
        {
            for(int i = 0; i < _defaultEquipObjects.Length; i++)
            {
                if(_defaultEquipObjects[i].Name == _hitComponent.name)
                {
                    _userConsole = $"Name:{_defaultEquipObjects[i].Name}\n"+
                    $"Goldcost:{_defaultEquipObjects[i].GoldCost}\n"+
                    $"Damage:{_defaultEquipObjects[i].Damage}\n"+
                    $"AttackType:{_defaultEquipObjects[i].AttackType}\n"+
                    $"Description:{_defaultEquipObjects[i].Description}\n"+
                    $"Weight:{_defaultEquipObjects[i].Weight}";
                    break;
                }
            }
        }
        // Если нажата клавиша покупки и хватает денег, то совершаем покупку и добавляем в инвентарь, если он не переполнен.
        // *Пока без инвентаря.
        private void PurchaseEquip()
        {

        }

    }
}
