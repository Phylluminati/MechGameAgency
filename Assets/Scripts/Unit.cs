using System.Collections.Generic;
using CardRunner;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public List<HpType> healthBarType;
    public List<int> healthBarMaxValues;
    [SerializeField] Sprite personalSprite;
    [SerializeField] Sprite mechSprite;

    public int maxDamage;
    public int minDamage;

    public Card.DamageType preferredDmgType;

    public enum HpType
    {
        Hull,
        Armor,
        Shield
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
