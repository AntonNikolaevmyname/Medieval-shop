using UnityEngine;

/*
    TO DO: оптимизировать механизм поиска информации о текущем hitComponet из ScriptableObject'ов.
*/

namespace CompleteApp
{
    internal sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance{get; private set;} 

        private Transform _hitComponent;
        private PlayerRecourseController _player;
        public PlayerRecourseController Player{get => _player; private set => _player = value;}

        [Tooltip("Покупка/действие на сцене")]
        public KeyCode action = KeyCode.E;
        [Tooltip("Инвентарь")]
        public KeyCode inventory = KeyCode.Q;
        // Режим отладки, рисует лучи, вывод дополнительной информации в консоль.
        [SerializeField]
        [Tooltip("Режим отладки")]
        private bool _debug;
        public bool Debug{get => _debug; private set => _debug = value;}
        public string LayerMaskName{get; private set;} = "Equipment";

        [SerializeField]
        [Tooltip("Все ScriptableObject'ы должны быть здесь")]
        private DefaultEquipObject[]  _defaultEquipObjects;
        private DefaultEquipObject _currentDEO;
        public DefaultEquipObject CurrentDefaultEquipObject
        {
            get => _currentDEO;
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
            Player = new PlayerRecourseController()
            {
                Money = 10000
            };
        }


        private void Update()
        {
            CheckInventoryClick();

            if(_hitComponent == null)
            {
                UserConsole.Instance.UserConsoleText = string.Empty;
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
        public void HitComponentMakeNull()
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
                    UserConsole.Instance.PrintIntoUserConsoleEquipInfo(CurrentDefaultEquipObject, action);
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
                if(Player.Money >= _currentDEO.GoldCost)
                {
                    Player.Money -= _currentDEO.GoldCost;
                    Player.AddEquipInInventory(_currentDEO);
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
                string[] inv = Player.InventoryObjectsNameEnumeration;
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
