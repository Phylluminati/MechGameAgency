using UnityEngine;
using CardRunner;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
public class EffectIconLogic : MonoBehaviour
{
    public bool isPlayerEffect = false; 
    public bool[] isTickDownEffect;
    [SerializeField]
    Sprite shieldIcon;
    [SerializeField]
    Sprite selfDestructIcon;
    [SerializeField]
    Sprite meltIcon;
    [SerializeField]
    Sprite armorIcon;
    [SerializeField]
    Sprite HACKifPHYIcon;
    [SerializeField]
    Sprite PHYifHACKIcon;
    [SerializeField]
    GameObject IconPrefab;
    [SerializeField]
    Sprite drainSprite;
    [SerializeField] Sprite phyAttack;
    [SerializeField] Sprite hackAttack;
    [SerializeField] Sprite shieldSwitch;
    [SerializeField] Sprite armorSwitch;
    UnitTracker TrackedUnit;
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //shieldSwitch = Resources.Load("Sprites/ShieldSwitchSprite") as Sprite;
        //armorSwitch = Resources.Load("Sprites/ArmorSwitchSprite") as Sprite;
        //drainSprite = Resources.Load("DRAINSprite") as Sprite;
        /*phyAttack = (Sprite)Resources.Load("Sprites/PhyAttack");
        hackAttack =(Sprite)Resources.Load("Sprites/HackAttack");
        if(hackAttack == null)
        {
            Debug.LogError("No Hack Sprite");
        }*/
        // transform.root.GetComponent<UnitTracker>();
        TrackedUnit = transform.parent.parent.GetComponent<UnitTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject tempIcon;
    public void UpdateIcons()
    {
        if(TrackedUnit.activeEffects.Count != 0)
            {

                for(int i = 0; i < transform.parent.parent.GetComponent<UnitTracker>().activeEffects.Count; i++)
                {
                    print("ChildCount: " + this.transform.childCount);
                    if(this.transform.childCount <= i)
                        {
                            tempIcon = Instantiate(IconPrefab, this.transform);
                        }
                    else
                        {
                            tempIcon = this.transform.GetChild(i).gameObject;
                        }

                    switch(transform.parent.parent.GetComponent<UnitTracker>().activeEffects[i])
                    {
                        
                        case Card.Effect.MELT:

                            //isTickDownEffect[i] = true;
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = meltIcon;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                            tempIcon.GetComponentInChildren<TMP_Text>().color = Color.red;
                            tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Deals 40 PHY damage per stack each turn";
                            
                        break;


                        case Card.Effect.DRAIN:
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = drainSprite;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                            tempIcon.GetComponentInChildren<TMP_Text>().color = Color.blue;
                            tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Deals 40 HACK damage per stack each turn";
                        break;

                        case Card.Effect.SELFSWITCH:

                        break;

                        case Card.Effect.ENEMYSWITCH:
                        //TODO: Check EnemySwitchType and adjust Icon accordingly
                            if(TrackedUnit.activeHealthbar == 0)
                            {
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = shieldSwitch;
                                tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                tempIcon.GetComponentInChildren<TMP_Text>().color = Color.blue;
                                tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Is switching their active Defense to SHIELD";
                            }
                        else
                            {
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = armorSwitch;
                                    tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                    tempIcon.GetComponentInChildren<TMP_Text>().color = Color.red;
                                    tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Is switching their active Defense to ARMOR";
                            }
                        break;

                        case Card.Effect.ENEMYATTACK:
                            if(TrackedUnit.isAttackPhy)
                            {
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = phyAttack;
                                tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                tempIcon.GetComponentInChildren<TMP_Text>().color = Color.red;
                                tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Is preparing to attack with PHY";
                            }
                            else
                            {
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = hackAttack;
                                tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                tempIcon.GetComponentInChildren<TMP_Text>().color = Color.green;
                                tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Is preparing to attack with HACK";
                            }
                        //TODO: Check EnemyAttackType and adjust Icon accordingly
                        break;
                        case Card.Effect.PILEDRIVER:
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = null;
                                tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                tempIcon.GetComponentInChildren<TMP_Text>().color = Color.red;
                                tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Played PHY cards turn into Piledrivers";                  
                        break;
                        case Card.Effect.ODAY:
                                tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = null;
                                tempIcon.GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                                tempIcon.GetComponentInChildren<TMP_Text>().color = Color.green;
                                tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Played HACK cards turn into Zer0 Days";   
                        break;
                        case Card.Effect.SELFDESTRUCT:
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = selfDestructIcon;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = " ";
                            tempIcon.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = "Explodes at the End of Turn";   
                        break;

                        default:
                            print("Something went wrong with Icon sprite application, deleting Icon");
                            Destroy(tempIcon);
                        break;
                    }
                    //this.transform.GetChild(i).GetComponentInChildren<TMP_Text>().text = transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                    print("Effect Duration: " + transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i]);
                    //if(0 >= transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration[i])
                    if(i >= transform.parent.parent.GetComponent<UnitTracker>().activeEffectDuration.Count)
                        {
                            print("Empty destroying ICON");
                            Destroy(this.transform.GetChild(i).gameObject);
                        }
                    
                }
                //Double check at the end of each assignment if there is one icon too much
                // STILLL DOESN TWORK!!!!
                
            }

            //print(transform.parent.parent.gameObject.name + "'s Children: " + this.transform.childCount);

            for(int i = 0; i <= this.transform.childCount; i++)
                {
                    if(transform.parent.parent.GetComponent<UnitTracker>().activeEffects.Count < i)
                        {
                            print("Empty destroying ICON: " + i);
                            Destroy(this.transform.GetChild(i-1).gameObject);
                        }
                }
        
    }
}
