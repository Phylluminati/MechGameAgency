using UnityEngine;
using CardRunner;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using UnityEditor.ShaderGraph;
using Mono.Cecil;
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
    
    
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //drainSprite = Resources.Load("DRAINSprite") as Sprite;
        // transform.root.GetComponent<UnitTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject tempIcon;
    public void UpdateIcons()
    {
        if(transform.root.GetComponent<UnitTracker>().activeEffects.Count != 0)
            {

                for(int i = 0; i < transform.root.GetComponent<UnitTracker>().activeEffects.Count; i++)
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

                    switch(transform.root.GetComponent<UnitTracker>().activeEffects[i])
                    {
                        
                        case Card.Effect.MELT:

                            //isTickDownEffect[i] = true;
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = meltIcon;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = transform.root.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                            tempIcon.GetComponentInChildren<TMP_Text>().color = Color.red;
                            
                        break;


                        case Card.Effect.DRAIN:
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = drainSprite;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = transform.root.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                            tempIcon.GetComponentInChildren<TMP_Text>().color = Color.blue;
                        break;

                        case Card.Effect.SELFSWITCH:

                        break;

                        case Card.Effect.ENEMYSWITCH:
                        
                        break;

                        case Card.Effect.SELFDESTRUCT:
                            tempIcon.GetComponent<UnityEngine.UI.Image>().sprite = selfDestructIcon;
                            tempIcon.GetComponentInChildren<TMP_Text>().text = " ";
                        break;

                        default:
                            print("Something went wrong with Icon sprite application, deleting Icon");
                            Destroy(tempIcon);
                        break;
                    }
                    //this.transform.GetChild(i).GetComponentInChildren<TMP_Text>().text = transform.root.GetComponent<UnitTracker>().activeEffectDuration[i].ToString();
                    print("Effect Duration: " + transform.root.GetComponent<UnitTracker>().activeEffectDuration[i]);
                    //if(0 >= transform.root.GetComponent<UnitTracker>().activeEffectDuration[i])
                    if(i >= transform.root.GetComponent<UnitTracker>().activeEffectDuration.Count)
                        {
                            print("Empty destroying ICON");
                            Destroy(this.transform.GetChild(i).gameObject);
                        }
                    
                }
                //Double check at the end of each assignment if there is one icon too much
                // STILLL DOESN TWORK!!!!
                
            }

            print(transform.root.gameObject.name + "'s Children: " + this.transform.childCount);

            for(int i = 0; i <= this.transform.childCount; i++)
                {
                    if(transform.root.GetComponent<UnitTracker>().activeEffects.Count < i)
                        {
                            print("Empty destroying ICON: " + i);
                            Destroy(this.transform.GetChild(i-1).gameObject);
                        }
                }
        
    }
}
