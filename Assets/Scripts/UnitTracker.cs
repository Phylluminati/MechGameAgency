using UnityEngine;
using System.Collections.Generic;
using CardRunner;

public class UnitTracker : MonoBehaviour
{
    public Unit unitData;
    public List<int> healthBarCurrentValues;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] 
    GameObject HealthBarField;
    public int activeHealthbar;
    // 0 = ARMOR // 1 == SHIELD // Done this way to allow the possibility of extra active Healthbars
    private GameObject _gameManager;
    public List<Card.Effect> activeEffects;
    public List<int> activeEffectDuration;
    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager");
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
            print("Type: " + unitData.healthBarType[i] + "; Amount: " + healthBarCurrentValues[i]);

            this.GetComponent<BattleHUD>().FullUpdateCheck();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
