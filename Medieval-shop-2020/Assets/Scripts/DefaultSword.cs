using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Sword", menuName = "Default Sword", order = 51)]
public class DefaultSword : ScriptableObject
{
    [SerializeField]
    private string swordName;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int goldCost;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float weight;

    public string SwordName
    {
        get
        {
            return swordName;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int GoldCost
    {
        get
        {
            return goldCost;
        }
    }

    public float AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
    public float Weight
    {
        get
        {
            return weight;
        }
    }
}
