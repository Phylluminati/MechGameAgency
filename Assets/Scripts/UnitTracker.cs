using UnityEngine;
using System.Collections.Generic;
using CardRunner;
using UnityEngine.Rendering;

public class UnitTracker : MonoBehaviour
{
    public Unit unitData;
    public List<int> healthBarCurrentValues;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] 
    GameObject HealthBarField;
    public int activeHealthbar;
    // 0 = ARMOR /A/ 1 == SHIELD // Done this way to allow the possibility of extra active Healthbars
    private GameObject _gameManager;
    public List<Card.Effect> activeEffects;
    public List<int> activeEffectDuration;
    public bool isAttackPhy;
    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager");  
    }
    public void InitiateMech()
    {
        activeHealthbar = Random.Range(1,3);
        for(int i = 0; i < unitData.healthBarType.Count; i++)
        {
            if(unitData.healthBarType[i] == Unit.HpType.Hull)
            {
                healthBarCurrentValues[i] = unitData.healthBarMaxValues[i];
            }
            else if(unitData.healthBarType[i] == Unit.HpType.Armor)
            {
                healthBarCurrentValues[i] = unitData.healthBarMaxValues[i];
            }  
            else if(unitData.healthBarType[i] == Unit.HpType.Shield)
            {
                healthBarCurrentValues[i] = unitData.healthBarMaxValues[i];
            } 
            //print("Type: " + unitData.healthBarType[i] + "; Amount: " + healthBarCurrentValues[i]);
        }
        if(unitData.healthBarMaxValues[1] > 0 && unitData.healthBarMaxValues[2] > 0)
        {
            activeHealthbar = UnityEngine.Random.Range(0,2);
        }
        else
        {
            if(unitData.healthBarMaxValues[1] > 0)
            {
                activeHealthbar = 0;
            }
            else
            {
                activeHealthbar = 1;
            }
        }
        
        this.GetComponent<BattleHUD>().FullUpdateCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HealMech(int hp_type, float percentage)
    {
        percentage = percentage / 100;
        healthBarCurrentValues[hp_type]= (int)Mathf.Clamp((float)healthBarCurrentValues[hp_type]+((float)unitData.healthBarMaxValues[hp_type]*percentage),0 ,(float)unitData.healthBarMaxValues[hp_type]);
    }
}
