using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private Transform _hitComponent = null;

    // Режим отладки, рисует лучи, вывод дополнительной информации в консоль.
    [SerializeField]
    private bool _debug;
    public bool Debug{get => _debug;}

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
        _userConsole = string.Empty;
    }

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
    }

    private void OnGUI()
    {
        if(_userConsole == string.Empty)
            return;

        // Пользовательская консоль.
        GUI.TextField(new Rect(10, 10, 150, 100), _userConsole);
    }
    private void Update()
    {
        if(_hitComponent == null)
        {
            return;
        }

        PurchaseEquip();
    }

    // Взаимодействие с объектом:
    // 1. Он подсвечивается.
    // 2. В консоль выводится информация о нем из ScriptableObject по его имени.
    // 3. Если нажать кнопку "Купить" (на клавиатуре),       -> в Update()
    // (*4.) то он добавится в инвентарь.                    -> в Update()
    public void InteractWithHitObject(Transform hit)
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
    
    // Активация шейдера(подсветки) при наведении raycast'ов на объект (оружие).
    private void ActivatingShadersOnHover()
    {

    }
    // Вывод информации в польз.консоль о _hitComponent.
    private void UserConsoleOutput()
    {

    }
    // Если нажата клавиша покупки и хватает денег, то совершаем покупку и добавляем в инвентарь, если он не переполнен.
    // *Пока без инвентаря.
    private void PurchaseEquip()
    {

    }
}
