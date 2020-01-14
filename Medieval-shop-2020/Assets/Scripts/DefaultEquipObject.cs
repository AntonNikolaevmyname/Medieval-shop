using UnityEngine;

/*
    Имя объекта на сцене, с которым будет взаимодействовать игрок, должно
    совпадать с имененем этого ScriptableObject'а, потому что по нему происходит 
    поиск информации для вывода.
*/
namespace CompleteApp
{
    [CreateAssetMenu(fileName = "New Equip Object", menuName = "Equip Object", order = 51)]
    public class DefaultEquipObject : ScriptableObject
    {
        [SerializeField]
        private AttackTypeEnum _attackType; 
        [SerializeField]
        private string _name; 
        [SerializeField]
        private string _description;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private int _goldCost;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _weight;

        public enum AttackTypeEnum{
            melee = 0,
            range = 1,
        }
        public AttackTypeEnum AttackType{get => _attackType;}
        public string Name{get => _name;}
        public string Description{get => _description;}
        public Sprite Icon{get => _icon;}
        public int GoldCost{get => _goldCost;}
        public float Damage{get => _damage;}
        public float Weight{get => _weight;}
    }
}