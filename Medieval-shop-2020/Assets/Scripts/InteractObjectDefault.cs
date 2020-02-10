using UnityEngine;
using System;
using System.Runtime.InteropServices;

/*
    Имя объекта на сцене, с которым будет взаимодействовать игрок, должно
    совпадать с имененем этого ScriptableObject'а, потому что по нему происходит 
    поиск информации для вывода.
*/
namespace CompleteApp
{
    [CreateAssetMenu(fileName = "New Interact Object", menuName = "Interact Object", order = 51)]
    sealed internal class InteractObjectDefault : ScriptableObject
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
        private float _goldCost;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _weight;
        [SerializeField]
        private string _id = System.Guid.NewGuid().ToString(); 
        [SerializeField]
        private float _heal;
        
        public enum AttackTypeEnum{
            melee = 0,
            range = 1,
        }
        public AttackTypeEnum AttackType{get => _attackType;}
        public string Name{get => _name;}
        public string Description{get => _description;}
        public Sprite Icon{get => _icon;}
        public float GoldCost{get => _goldCost;}
        public float Damage{get => _damage;}                        // Отрицательная атака == лечение.
        public float Weight{get => _weight;}
        public string Id { get => _id; }
        public float Heal{get => _heal;}
    }
}