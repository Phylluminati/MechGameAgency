using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using CardRunner;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    public int RoundsSinceAttackingBoss = 0;
    [SerializeField]
    GameObject DialogueImage;
    [SerializeField]
    UnityEngine.UI.Image PilotSprite;
    [SerializeField]
    private bool fadeIn = false;
    private bool fadedone = false;
    private CanvasGroup ImageGroup;
    private GameManagerScript GameManager;
    [SerializeField]
    Run run;
    [SerializeField]
    GameObject CardPickPrefab;
    [SerializeField]
    GameObject CardDiscardPrefab;
    [SerializeField] GameObject dialoguerunner;
    [SerializeField] GameObject MissionCompletePanel;
    

[YarnCommand("FadeIn")]
    public void FadeIn()
    {   
        DialogueImage.SetActive(true);
        ClearBackgroundImage(); 
        fadeIn = true;
        fadedone = false;
    }
[YarnCommand("FadeOut")]
    public void FadeOut()
    {
        DialogueImage.SetActive(true);
        ClearBackgroundImage();
        fadeIn = false;
        fadedone = false;
    }
[YarnCommand("ChangeBackgroundImage")]
    public void ChangeBackgroundImage(string spritename)
    {
        Sprite sprite = Resources.Load<Sprite>("DialogueSprite/"+ spritename) as Sprite;
        print("sprite");
        DialogueImage.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        DialogueImage.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
    [YarnCommand("ClearBackgroundImage")]
    public void ClearBackgroundImage()
    {
        DialogueImage.GetComponent<UnityEngine.UI.Image>().sprite = null;
        DialogueImage.GetComponent<UnityEngine.UI.Image>().color = Color.black;
    }

    public bool PlayerAttackCheck()
    {
        if(GameManager.roundHackDmgOutput + GameManager.roundPhyDmgOutput == 0) //Add Applied effects to this, either check if there are hostile effects on the enemy mech or check if they have been applied
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    [YarnCommand("PilotSprite")]
    public void ChangePilotSprite(string pilotstring)
    {
        if(pilotstring == null)
        {
            print("Pilotstring is null, resetting and turning black");
            PilotSprite.sprite = null;
            PilotSprite.color = Color.black;
        }
        else
        {
            PilotSprite.color = Color.white;
        }
        Sprite sprite = Resources.Load<Sprite>("PilotSprites/"+ pilotstring) as Sprite;
        PilotSprite.sprite = sprite;
    }
    
    void Awake()
    {
        ImageGroup = DialogueImage.GetComponent<CanvasGroup>();
        GameManager = this.gameObject.GetComponent<GameManagerScript>();
        gameCanvas = GameManager.MainMechLocation.transform.parent.gameObject;
        OutOfGameCanvas = GameObject.Find("OutsideGameCanvas");
        
    }
    void Start()
    {
        
    }
    [YarnCommand("RewardCards")]
    public void RewardCards(int card_amount, int cards_to_choose, int card_type = 10000)
    { //card_type = 0 is PHY, 1 is HACK, 2 is MULT, 3 is RUST, 4 is NULL
        GameObject temp_card_pick = Instantiate(CardPickPrefab, OutOfGameCanvas.transform);
        dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 20;
        if(card_type < 500)
        {
            Card.CardType temp_type = (Card.CardType)card_type;
            temp_card_pick.GetComponentInChildren<NewCardSelector>().typeChoice = temp_type;
        }
        temp_card_pick.GetComponentInChildren<NewCardSelector>().cardsToChoose = cards_to_choose;
        temp_card_pick.GetComponentInChildren<NewCardSelector>().cardAmount = card_amount;
    }
    [YarnCommand("DiscardCards")]
    public void DiscardCards(int card_amount, int cards_to_choose)
    {       
        dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 20;
        if(GameManager.currentDeck.Count > card_amount)
        {
            GameObject temp_card_pick = Instantiate(CardDiscardPrefab, OutOfGameCanvas.transform);
            temp_card_pick.GetComponentInChildren<SelectCardsToDiscard>().cardsToChoose = cards_to_choose;
            temp_card_pick.GetComponentInChildren<SelectCardsToDiscard>().cardAmount = card_amount;
            dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 0;
        }
        else
        {
            Debug.LogWarning("ERROR WITH CARD DISCARD, NOT ENOUGH CARDS IN DECK TO MAKE A FULL DISCARD");
            return;
        }
    }
    public void ResetDialogueCanvas()
    {
        dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 30;
    }
    /*public void FadeIn()
    {
        Color b = DialogueImage.GetComponent<UnityEngine.UI.Image>().color;
        float _progress = 1;
    
        while(_progress > 0)
        {
            //DialogueImage.GetComponent<UnityEngine.UI.Image>().color = 0f;
            b.a = Mathf.MoveTowards(_progress, 0f, 0.05f * Time.deltaTime);
            DialogueImage.GetComponent<UnityEngine.UI.Image>().color = b;
            _progress = b.a;
        }
        print("End of while loop exiting method");
        
        Color tmp = DialogueImage.GetComponent<UnityEngine.UI.Image> ().color;
		tmp.a = 0f;
		DialogueImage.GetComponent<UnityEngine.UI.Image> ().color = tmp;
		float _progress = 0.0f;

		while (_progress < 1) 
		{
			Color _tmpColor = DialogueImage.GetComponent<UnityEngine.UI.Image> ().color;
			DialogueImage.GetComponent<UnityEngine.UI.Image> ().color = new Color (_tmpColor.r, _tmpColor.g, _tmpColor.b, Mathf.Lerp (tmp.a, 255, _progress)); //startAlpha = 0 <-- value is in tmp.a
			_progress += Time.deltaTime * 0.5f;
		}
    }
    
    void fadeOut()
    {
        
    }
    */
    void Update()
    {
        if(fadedone == false)
        {
            if(fadeIn == true)
            {
                if(ImageGroup.alpha < 1)
                {
                    ImageGroup.alpha += 2 * Time.deltaTime;
                    if(ImageGroup.alpha >= 255)
                    {
                        fadedone = true;
                        
                    }
                }
            }
            else
            {
                if(ImageGroup.alpha > 0)
                {
                    ImageGroup.alpha -= 2 * Time.deltaTime;
                    if(ImageGroup.alpha == 0)
                    {
                        fadedone = true;
                        DialogueImage.SetActive(false);
                    }
                }
            }
        }
        
    }
    GameObject OutOfGameCanvas;
    GameObject gameCanvas;
    public void PauseGameStartDialogue()
    {
        gameCanvas.SetActive(false);
        dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 30;
    }

    
    public void ContinueGameStopDialogue()
    {
        dialoguerunner.GetComponentInChildren<Canvas>().sortingOrder = 0;
        gameCanvas.SetActive(true);
        dialoguerunner.GetComponent<InMemoryVariableStorage>().TryGetValue<bool>("$MapSwitchFlag", out bool MapSwitchFlag);
        if(MapSwitchFlag == true)
        {
            dialoguerunner.GetComponent<InMemoryVariableStorage>().SetValue("$MapSwitchFlag", false);
            run.SwitchScene(0);
        }
    }

    public void RewardCredit(float coinAmount)
    {
        float CompanyCredit;
        dialoguerunner.GetComponent<InMemoryVariableStorage>().TryGetValue<float>("$CompanyCredit", out CompanyCredit);
        print("Company Credit before adjustment: " + CompanyCredit);
        dialoguerunner.GetComponent<InMemoryVariableStorage>().SetValue("$CompanyCredit", CompanyCredit + coinAmount);
    }

    public void StartDialogue(string SceneName)
    {
        dialoguerunner.GetComponent<DialogueRunner>().StartDialogue(SceneName);
    }
    public bool CheckTalkingFlag()
    {
        bool keepstalking;
        dialoguerunner.GetComponent<InMemoryVariableStorage>().TryGetValue<bool>("$keepstalking", out keepstalking);
       return keepstalking;
    }
    [YarnCommand("MissionComplete")]
    public void MissionComplete()
    {
        MissionCompletePanel.SetActive(true);
    }
}
