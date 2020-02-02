using UnityEngine;
using System;

namespace CompleteApp
{
    internal sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance{get; private set;} 

        public static event Action<Transform> OnLookAtAnyEquip = delegate{};


        private Transform _hitComponent;
        private PlayerRecourseController _player;
        public PlayerRecourseController Player {get => _player; private set => _player = value;}


        [SerializeField]
        [Tooltip("Режим отладки")]
        private bool _debug;
        public bool Debug {get => _debug; private set => _debug = value;}
        public Transform HitComponent { get => _hitComponent; set => _hitComponent = value; }

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
                Money = Params.Instance.PlayerMoney
            };
        }


        private void Update()
        {
            if(HitComponent == null)
            {
                UserConsole.Instance.UserConsoleText = null;
                return;
            }
        }

        public void InteractWithHitObject<T>(T hit) where T: Transform
        {
            if(hit == null)
            {
                print($"Class GameManager. Method 'InteractWithHitObject'. Exception: hit is null");
                return;
            }
            
            HitComponent = hit;
            OnLookAtAnyEquip(HitComponent);
        }
    }
}
