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
    //Like preferredDmgType, but a value, normally the damage preference will be 50-50, -0-50 is PHY and 51-100+ is HACK, the offset gets added
    //tldr -Value means +PHY, and +Value means +HACK
    public int PreferenceOffset;

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
