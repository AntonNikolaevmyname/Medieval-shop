using System.Collections.Generic;

namespace CompleteApp
{
    // Класс PlayerRecourseController управляет всеми русурсами игрока
    // такими как: инвентарь, деньги и тд.
    // Данное представление примитивное отображение инвентаря и механики его работы.
    internal sealed class PlayerRecourseController
    {
        public int Money{get;set;}

        // Слоты инвентаря. 
        private List<DefaultEquipObject> _inventory = new List<DefaultEquipObject>();

        public void AddEquipInInventory(DefaultEquipObject deo)
        {
            _inventory.Add(deo);
        }
        public void RemoveEquipFromInventory(DefaultEquipObject deo)
        {
            _inventory.Remove(deo);
        }
        public void ClearInventory()
        {
            _inventory.Clear();
        }

        public string[] InventoryObjectsNameEnumeration
        {
            get
            {
                string[] collection = new string[_inventory.Count];
                for (int i = 0; i < _inventory.Count; i++)
                {
                    collection[i] = _inventory[i].Name;
                }
                return collection;
            }
        }
    }
}
