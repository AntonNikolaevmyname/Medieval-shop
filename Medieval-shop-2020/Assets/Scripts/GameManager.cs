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
        private PlayerRecourseController _player;

        [Tooltip("Покупка/действие на сцене")]
        public KeyCode action = KeyCode.E;
        [Tooltip("Инвентарь")]
        public KeyCode inventory = KeyCode.Q;
        private string _pressAction;
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
        private DefaultEquipObject _currentDEO;
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
            // Инициализация.
            _userConsole = string.Empty;
            _player = new PlayerRecourseController();
            _player.Money = 10000;
            _pressAction = $"Нажмите {action} для покупки";
        }

        private void OnGUI()
        {
            // Чеканные монеты игрока.
            GUI.TextField(new Rect(10, 10, 200, 50), $"Money: {_player.Money}");
            if(_userConsole == string.Empty)
                return;

            // Пользовательская консоль.
            GUI.TextField(new Rect(10, 100, 200, 150), _userConsole);
        }
        private void Update()
        {
            CheckInventoryClick();

            if(_hitComponent == null)
            {
                _userConsole = string.Empty;
                return;
            }

            PurchaseEquipClick();
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
        // TO DO: Максимально неоптимизированный поиск для большого количества объектов, надо переделать.
        private void UserConsoleOutput()
        {
            for(int i = 0; i < _defaultEquipObjects.Length; i++)
            {
                if(_defaultEquipObjects[i].Name == _hitComponent.name)
                {
                    _currentDEO = _defaultEquipObjects[i];

                    _userConsole = $"Name:{_defaultEquipObjects[i].Name}\n"+
                    $"Goldcost:{_defaultEquipObjects[i].GoldCost}\n"+
                    $"Damage:{_defaultEquipObjects[i].Damage}\n"+
                    $"AttackType:{_defaultEquipObjects[i].AttackType}\n"+
                    $"Description:{_defaultEquipObjects[i].Description}\n"+
                    $"Weight:{_defaultEquipObjects[i].Weight}\n\n\n"+
                    $"*****{_pressAction}*****";
                    return;
                }
            }
            // Если совпадений не найдено, то объекта с таким именем нет в массиве ScriptableObject'ов
            print($"Name:{_hitComponent.name} не найдено в массиве ScriptableObject'ов. Class 'GameManager'. Method 'UserConsoleOutput'");
        }
        // Если нажата клавиша покупки и хватает денег, то совершаем покупку и добавляем в инвентарь, 
        // если он не *переполнен.
        // *Пока без инвентаря.
        private void PurchaseEquipClick()
        {
            if(Input.GetKeyDown(action))
            {
                if(_player.Money >= _currentDEO.GoldCost)
                {
                    _player.Money -= _currentDEO.GoldCost;
                    _player.AddEquipInInventory(_currentDEO);
                    print($"{_hitComponent.name} удачно добавлен в инвентарь.");
                    _hitComponent = null;
                }
                else
                {
                    print($"Недостаточно золота.");
                }
            }
        }

        private void CheckInventoryClick()
        {
            if(Input.GetKeyDown(inventory))
            {
                string[] inv = _player.InventoryObjectsNameEnumeration;
                if(inv.Length == 0)
                {
                    print($"Инвентарь пуст.");
                    return;
                }

                for(int i = 0; i < inv.Length; i++)
                {
                    print($"Слот {inv[i]} в инвентаре.");
                }
            }
        }

    }
}
