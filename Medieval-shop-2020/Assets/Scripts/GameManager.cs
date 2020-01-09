using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //public delegate DefaultSword GetCurrentEquipInfo(); // зачем?
    //public GetCurrentEquipInfo currentEquip;

    [SerializeField]
    private List<DefaultSword> _meleeEquips;

    [SerializeField]
    private List<DefaultSword> _rangeEquips;

    private DefaultSword _currentEquip;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            print($"Warning:{instance} is unknown object.");
        }

        _meleeEquips = new List<DefaultSword>();
        _rangeEquips = new List<DefaultSword>();
    }

    private void SetEquipConfig()
    {
        // TO DO: 
        // 1. Берем имя объекта из PlayerEquipmentSearcher.HitComponent.name
        // оно должно строится по логике(не забыть try) тип боя_имя
        // 2. Ищем файлик с хар-ками в соответствующем списке.
        // 3. Сохраняем этот файлик в отдельную переменую.
    }
}
