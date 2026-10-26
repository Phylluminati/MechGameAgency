using System;
using System.Collections.Generic;
using UnityEngine;
using CardRunner;
using TMPro;
using System.Data;
using UnityEditor.PackageManager;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using NUnit.Framework;
using Yarn.Unity;
using System.Collections;
using System.Numerics;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    //This GameManager is designated for multiple things, both in-scene and inter-scenes
        //in scene
            // it should keep track the HP or Armor of a player as well as the conclusion of the turn order
            //It should manage the calling and initiation of Dialogue between each turn
            //It should manage (or at least call upon) the enemy data-script
        //inter-scene
            //it should keep track of the deck if vague elements of deckbuilding get implemented into this game
            //it should keep track of narrative decisions that will be relevant in future dialogue
            //it should keep track of any other relevant data, up to including setting preferences

    public GameObject currentEnemy;
    public GameObject MainMech;
    public List<Card> currentDeck;
    public int RoundEnergy;
    public int DiscardsThisTurn;
    public List<Card> cardsPlayedThisTurn;
    public List<Card> drawPile;
    public List<Card> discardPile;

    public List<Card> AllCardsInPlay;
    
    [SerializeField] GameObject GameOver;
    [SerializeField] GameObject BattleWonPanel;
    [SerializeField] GameObject CardEmpty;
    [SerializeField] TMP_Text DrawNumber;
    [SerializeField] TMP_Text DiscardNumber;
    [SerializeField] TMP_Text TurnDiscards;

    [SerializeField] Button EndTurnButton;
    //DMGNr is the PreFab for the Damage Numbers that pop up when inflicting damage of any kind
    [SerializeField] GameObject DMGNr;
     Run run;

    //the EnemyMech and Mainmech locations are empty gameObjects to determine the Mechs location on the canvas so UI effects can be instantiated in the right positions
    [SerializeField] GameObject EnemyMechLocation;
    public GameObject MainMechLocation;

    private GameObject _PHYDMGCounter;
    private GameObject _HACKDMGCounter;

    public bool mainShieldActive;
    public bool enemyShieldActive;


    public GameObject cardPrefab;
    [SerializeField] GameObject SelectNewCardPrefab;
    [SerializeField] GameObject ParticleExplosionPrefab;
    public int roundHackDmgOutput;
    public int roundPhyDmgOutput;
    public int RoundlyEnergy = 4;
    public int TurnlyDiscards = 2;

void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        run = GameObject.Find("SceneSwitcher").GetComponent<Run>();
        _PHYDMGCounter = GameObject.Find("PHYDMGcounter");
        _HACKDMGCounter = GameObject.Find("HACKDMGcounter");
        _PHYDMGCounter.SetActive(false);
        _HACKDMGCounter.SetActive(false);
        
        
    }

public void AddCardToDeck(Card cardData)
    {
        //This will add a new card to the currentDeck List
        currentDeck.Add(cardData);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        MainMech.GetComponent<UnitTracker>().InitiateMech();
    }

    // Update is called once per frame
void Update()
    {
        
    }
void StartofCombat()
    {

        GenerateDrawPile();
        BattleWonPanel.SetActive(false);
        if(currentEnemy.GetComponent<UnitTracker>().unitData != null)
        {
            currentEnemy.GetComponent<UnitTracker>().InitiateMech();
        }
        else
        {
            Debug.LogError("Tried to access Enemies Unit Data, which isn't assigned somehow, did something go wrong with assigning Enemy Data in the Run Script?");
        }
        this.GetComponent<DialogueManager>().ContinueGameStopDialogue();
        this.GetComponent<DialogueManager>().ClearBackgroundImage();
        this.GetComponent<DialogueManager>().FadeOut();
        NewRoundCheck();
    }

public void NewRoundCheck()
    {
        //check off Effects on player (MELT does dmg at the beginning of Players turn)
        if(MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[0] <= 0)
        {
            StartCoroutine(LoseRoutine());
        }
       if(drawPile.Count == 0)
        {
            GenerateDrawPile(); 
        }
        // if I wanna add an effect that carries over energy between rounds I need to implement it right before the round energy gets reset to 4
        RoundEnergy = RoundlyEnergy;
        DiscardsThisTurn = TurnlyDiscards;
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
        shuffleDrawPile();
        int newrounddraw = 7 - CardEmpty.transform.childCount;
        if(newrounddraw > 0)
        {
            DrawRandomCard(newrounddraw);
        }
        

        UpdateUIEffects();
        EndTurnButton.interactable = true;
    }
public void EndOfRoundCheck()
    {
        if(GameOver.activeSelf == true || BattleWonPanel.activeSelf == true)
        {
            return;
        }
        for(int i = CardEmpty.transform.childCount - 1; i >= 0; i--)
        {
            CardEmpty.transform.GetChild(i).gameObject.GetComponent<CardPlayScript>().DiscardCard(true);
        }
        
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
        //Apply Round damage
        //Check off Enemy Effects
        //Put Animator Hit attack here vvv
        Animator HitAnimator = MainMech.transform.GetChild(0).GetComponentInChildren<Animator>();
            if(HitAnimator != null)
            {
                HitAnimator.SetTrigger("Hit");
            }
        /*switch(currentEnemy.GetComponent<UnitTracker>().activeHealthbar)
        {
            case 0:
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundHackDmgOutput/2;
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundPhyDmgOutput;
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundPhyDmgOutput;
                if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] < 0)
                     {
                        currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1];

                        if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] < 0)
                        {
                            GameObject tempDmgNumber = Instantiate(DMGNr, EnemyMechLocation.transform);
                            tempDmgNumber.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1]+currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]).ToString();
                            tempDmgNumber.GetComponent<TMP_Text>().color = Color.red;

                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2];

                            GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;

                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] = 0;
                        }
                        else
                    {
                        GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1]).ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                    }


                        currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] = 0;
                    }
                else
                {
                    GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = (roundHackDmgOutput/2).ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;

                        tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = roundPhyDmgOutput.ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                }
            break;

            case 1:
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] -= roundHackDmgOutput;
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] -= roundPhyDmgOutput/2;
                if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] < 0)
                     {
                        currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2];
                        if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] < 0)
                        {
                            GameObject tempDmgNumber = Instantiate(DMGNr, EnemyMechLocation.transform);
                            tempDmgNumber.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]+currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1]).ToString();
                            tempDmgNumber.GetComponent<TMP_Text>().color = Color.green;

                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1];
                            GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;

                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] = 0;
                        }
                        else
                    {
                        GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]).ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;
                    }


                        currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] = 0;
                    }
                else
                {
                    GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = roundHackDmgOutput.ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;

                        tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = (roundPhyDmgOutput/2).ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                }
            break;
        }
        */

        if(currentEnemy.GetComponent<UnitTracker>().activeHealthbar == 0)
        {
            DealDamage(currentEnemy, roundPhyDmgOutput, Card.DamageType.PHY);
            DealDamage(currentEnemy, roundHackDmgOutput, Card.DamageType.HACK);
        }
        else
        {
            DealDamage(currentEnemy, roundHackDmgOutput, Card.DamageType.HACK);
            DealDamage(currentEnemy, roundPhyDmgOutput, Card.DamageType.PHY);
        }

        if(currentEnemy.GetComponent<UnitTracker>().activeEffects.Count != 0)
        {
            for(int i = 0; i < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; i++)
            {
                switch(currentEnemy.GetComponent<UnitTracker>().activeEffects[i])
                {
                    case Card.Effect.MELT:
                        //Deals XX Damage per stack of MELT to the target, Circumvents SHIELD, only Damages ARMOR and then HULL
                        int MELTStackDmg = 20;
                        int MELTFullDmg = MELTStackDmg * currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i];
                        GameObject tempDmgNumber = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber.GetComponent<TMP_Text>().text = MELTFullDmg.ToString();
                        tempDmgNumber.GetComponent<TMP_Text>().color = Color.red;
                        if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] > 0)
                            {
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= MELTFullDmg;
                                if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] < 0)
                                {
                                    GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                                    tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1]).ToString();
                                    tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                                    currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1];
                                    currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] = 0;
                                }
                            }
                        else
                            {
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] -= MELTFullDmg;
                            }
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                    break;

                    case Card.Effect.DRAIN:
                    //basically MELT but in HACK
                        //Beep
                        int DrainStackDMG = 20;
                        int DrainFullDmg = DrainStackDMG * currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i];
                        tempDmgNumber = Instantiate(DMGNr, EnemyMechLocation.transform);
                        tempDmgNumber.GetComponent<TMP_Text>().text = DrainFullDmg.ToString();
                        tempDmgNumber.GetComponent<TMP_Text>().color = Color.blue;
                        if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] > 0)
                            {
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] -= DrainFullDmg;
                                //leftover damage gets applied to HULL vvv
                                if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] < 0)
                                {
                                    GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                                    tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2]).ToString();
                                    tempDmgNumber2.GetComponent<TMP_Text>().color = Color.blue;
                                    currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2];
                                    currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] = 0;
                                }
                            }
                        else
                            {
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] -= DrainFullDmg;
                            }
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;

                    break;

                    case Card.Effect.ENEMYSWITCH:
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 0;
                        if(currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] == 0)
                        {
                            HealthbarSwitch(currentEnemy);
                        }
                    break;

                    case Card.Effect.STUN:

                    break;

                    case Card.Effect.AAA:

                    break;

                    case Card.Effect.RECURSIVE:

                    break;

                }
             
            }
            //I rather put this in a seperate for Loop, all this does is go over all the effects at end of turn and check if any of them have run out, if they have run out
            //it will remove the effect from the activeEffects List and aE-Duration List and then go back a step to account for any later effects that slip into the place of
            //the just checked List instance
            for(int i = 0; i < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; i++)
            {
                if(currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] == 0)
                {
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i);
                    currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                    i--;
                }
            }
            
            
        
        roundHackDmgOutput = 0;
        roundPhyDmgOutput = 0;
        UpdateDMGCounters();
        }
        if(MainMech.GetComponent<UnitTracker>().activeEffects.Count != 0)
        {
            //Win Check here before Self-Destruct Can fire
            if (currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] <= 0)
                    {
                        print("Battle Won :: Starting Coroutine");
                        StartCoroutine(EndBattle());
                        return;
                    }
            for(int i = 0; i < MainMech.GetComponent<UnitTracker>().activeEffects.Count; i++)
                {
                    // ---
                    switch(MainMech.GetComponent<UnitTracker>().activeEffects[i])
                    {
                        case Card.Effect.SELFDESTRUCT:
                            //ADD EXPLOSIVE SHAKEY EFFECTS HERE!
                            print("Selfdestruct triggered");
                            GameObject tempDmgNumber5 = Instantiate(DMGNr, MainMechLocation.transform);
                            tempDmgNumber5.GetComponent<TMP_Text>().text = MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[0].ToString();
                            tempDmgNumber5.GetComponent<TMP_Text>().color = Color.red;
                            GameObject tempDmgNumber3 = Instantiate(DMGNr, MainMechLocation.transform);
                            tempDmgNumber3.GetComponent<TMP_Text>().text = MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[1].ToString();
                            tempDmgNumber3.GetComponent<TMP_Text>().color = Color.orange;
                            GameObject tempDmgNumber4 = Instantiate(DMGNr, MainMechLocation.transform);
                            tempDmgNumber4.GetComponent<TMP_Text>().text = MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[2].ToString();
                            tempDmgNumber4.GetComponent<TMP_Text>().color = Color.green;
                            MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[0] = 0;
                            MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[1] = 0;
                            MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[2] = 0;
                            UpdateUIEffects();
                        break;
                    
                    
                        case Card.Effect.NON:
                        break;

                        case Card.Effect.DRAW:
                            
                        break;
                        
                        case Card.Effect.DOUBLEDMG:

                        break;  

                        case Card.Effect.DRAWPHYIFSHIELD:

                        break;

                        case Card.Effect.DRAWHACKIFPHY:

                        break;
                        case Card.Effect.PILEDRIVER:

                        break;
                    }
                    MainMech.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                    if(MainMech.GetComponent<UnitTracker>().activeEffectDuration[i] >= 0)
                    {
                        MainMech.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i);
                        MainMech.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                    }
                }
                //Same as with Enemy Mech above, Removing things from Lists in For Loops is a bit scuffed, most versions I've tried with multiple removals in a for loop blew up in my face
                //Since I keep the necessary removals as small as possible this should be fine
            for(int i = 0; i < MainMech.GetComponent<UnitTracker>().activeEffects.Count; i++)
                {
                    if(MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.PILEDRIVER)
                        {
                            currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i);
                            currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                        }
                }
            for(int i = 0; i < MainMech.GetComponent<UnitTracker>().activeEffects.Count; i++)
                {
                    if(MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.ODAY)
                {
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i);
                    currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                }
                }
        }

        


        if (currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] <= 0)
                {
                    //print("Battle Won :: Starting Coroutine");
                    StartCoroutine(EndBattle());
                    return;
                }
        roundHackDmgOutput = 0;
        roundPhyDmgOutput = 0;
        UpdateDMGCounters();
        MainMech.GetComponent<BattleHUD>().FullUpdateCheck();
        currentEnemy.GetComponent<BattleHUD>().FullUpdateCheck();
        UpdateUIEffects();

        EnemyTurn();
        //New round begins after End of Round and Enemy turn
        StartCoroutine(WaitBetweenEndRoundAndNewRound());
}

    

public void UpdateDMGCounters()
    {
        print("HACK dmg: " + roundHackDmgOutput);
        print("PHY dmg: " + roundPhyDmgOutput);
        //roundHackDmgOutput
        //roundPhyDmgOutput
        if(roundHackDmgOutput != 0)
        {
            _HACKDMGCounter.SetActive(true);
            _HACKDMGCounter.GetComponent<TMP_Text>().text = roundHackDmgOutput.ToString();
        }
        else
        {
            _HACKDMGCounter.SetActive(false);
        }
        if(roundPhyDmgOutput != 0)
        {
            _PHYDMGCounter.SetActive(true);
            _PHYDMGCounter.GetComponent<TMP_Text>().text = roundPhyDmgOutput.ToString();
        }
        else
        {
            _PHYDMGCounter.SetActive(false);
        }
        
    }

    void GenerateDrawPile()
    {
        if(drawPile.Count > 0)
        {
            for(int i = drawPile.Count -1; i > -1; i--)
            {
                drawPile.RemoveAt(i);
            }
        }
        for(int i = currentDeck.Count -1; i > -1; i--)
        {
            drawPile.Add(currentDeck[i]);
        }
        shuffleDrawPile();
    }

    public void DrawRandomCard(int drawAmount)
    {
        //int randomDeterminator;
        for(int i = 0; i < drawAmount; i++)
        {
            if(drawPile.Count > 0)
            {
                //randomDeterminator = UnityEngine.Random.Range(0, drawPile.Count);
                //GameObject card = Instantiate(cardPrefab, CardEmpty.transform);
                //card.GetComponent<CardDisplayScript>().cardData = drawPile[randomDeterminator];
                //drawPile.RemoveAt(randomDeterminator);  

                GameObject card = Instantiate(cardPrefab, CardEmpty.transform);
                card.GetComponent<CardDisplayScript>().cardData = drawPile[0];
                card.GetComponent<CardPlayScript>().cardData = drawPile[0];
                drawPile.RemoveAt(0);
            }
            else
            {
                if(discardPile.Count > 0)
                {
                    DiscardToDraw();
                    i--;
                }
                else
                {
                    print("drawpile and discardpile empty, aborting draw");
                }
            }
        }
        UpdateDrawDiscard();

    }

    public void shuffleDrawPile()
    {
        // fun fact this method of shuffling is known as the Fisher-Yates shuffle algorithm
        System.Random rng = new System.Random();
        int n = drawPile.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = drawPile[k];
            drawPile[k] = drawPile[n];
            drawPile[n] = value;
    }
    }
    
    public void DiscardToDraw()
    {
        for(int i = discardPile.Count - 1; i >= 0; i--)
        {
            drawPile.Add(discardPile[i]);
            discardPile.RemoveAt(i);
        }
        shuffleDrawPile();
        UpdateDrawDiscard();
    }

    public void DiscardCard(Card cardData)
    {

    }

    public void UpdateDrawDiscard() 
    {
        print("updating Draw and Discard");
        DrawNumber.text = drawPile.Count.ToString();
        DiscardNumber.text = discardPile.Count.ToString();
        TurnDiscards.text = DiscardsThisTurn.ToString();
    }
    public void DrawSpecificCardType(Card.CardType cardType, bool secondTry = false)
    {
        bool specificCardFound = false;
        for(int i = 0; i < drawPile.Count; i++)
        {
            if(drawPile.Count > 0 )
            {
                if(drawPile[i].cardType == cardType)
                {
                GameObject card = Instantiate(cardPrefab, CardEmpty.transform);
                card.GetComponent<CardDisplayScript>().cardData = drawPile[i];
                card.GetComponent<CardPlayScript>().cardData = drawPile[i];
                drawPile.RemoveAt(i);
                specificCardFound = true;
                break;
                }
            }
            else
            {
                if(discardPile.Count > 0)
                {
                    DiscardToDraw();
                    i--;
                }
                else
                {
                    print("drawpile and discardpile empty, aborting draw");
                }
            }
        }
        if(specificCardFound == false)
        {
            if(secondTry == true)
            {
                print("this function has fired twice already without result, there are none of the specific cardType in the Deck of the player");
                return;
            }
            DiscardToDraw();
            DrawSpecificCardType(cardType, true);
        }
        UpdateDrawDiscard();
    }
    //Variables for playing cards and applying effects
    bool foundEffectTrue = false;
    Card pileDriver;
    Card oDay;
    public void PlayCardEffect(Card cardData)

    {
        if(pileDriver == null || oDay == null)
        {
            for(int i = 0; i < AllCardsInPlay.Count; i++)
            {
                if(AllCardsInPlay[i].cardName == "Piledriver Waltz")
                {
                    pileDriver = AllCardsInPlay[i];
                }
                if(AllCardsInPlay[i].cardName == "Zero Day")
                {
                    oDay = AllCardsInPlay[i];
                }
            }
        }
        //this method is responsible for causing card effects to happen, 
        // it will read Carddata from played/discarded cards, that got played with the DiscardCard() method in the carddisplayscript
        //print("Card: " + cardData.cardName + "/n CardType: " + cardData.cardType);


        //following snippet of code is for the effects DrawHackifPHY and DrawPhyifHack which specialise in drawing specific cards when playing a certain card type
        for(int i = 0; i < MainMech.GetComponent<UnitTracker>().activeEffects.Count; i++)
        {
            if(MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.DRAWPHYIFSHIELD)
            {
                if(cardData.cardType == Card.CardType.HACK)
                {
                    DrawSpecificCardType(Card.CardType.PHY);
                }
            }
            if(MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.DRAWHACKIFPHY)
            {
                if(cardData.cardType == Card.CardType.PHY)
                {
                    DrawSpecificCardType(Card.CardType.HACK);
                }
            }
            if((MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.PILEDRIVER) && (cardData.cardType == Card.CardType.PHY) && (cardData != pileDriver)) 
            {
                int tempnumber = MainMech.GetComponent<UnitTracker>().activeEffectDuration[i];
                
                //cardData = pileDriver;
                //FOR EVERY PILEDRIVER PLAYED THIS TURN
                for(int j = 0; j < tempnumber; j++)
                {
                    PlayCardEffect(pileDriver);
                }
                //End This Instance of PlayCardEffect() after instantiating all the necessary piledrivers
                return;
            }
            if((MainMech.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.ODAY) && (cardData.cardType == Card.CardType.HACK) && (cardData!=oDay))
            {
                int tempnumber2 = MainMech.GetComponent<UnitTracker>().activeEffectDuration[i];

                //cardData = oDay;
                //FOR EVERY oDay PLAYED THIS TURN
                for(int j = 0; j < tempnumber2; j++)
                {
                    PlayCardEffect(oDay);
                }
                //End This Instance of PlayCardEffect() after instantiating all the necessary ODAYs
                return;
            }
        }
        

        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
            if(cardData.dmg != 0)
            {
                switch(cardData.cardType)
                {
                    case Card.CardType.PHY:
                        //ResolveDamageOutput here? or somewhere else?
                        roundPhyDmgOutput += cardData.dmg;
                        UpdateDMGCounters();
                    break;

                    case Card.CardType.HACK:
                        roundHackDmgOutput += cardData.dmg;
                        UpdateDMGCounters();
                    break;
                }

            }
            if(cardData.applyEffect.Count != 0)
            {
                for(int i = 0; i < cardData.applyEffect.Count; i++)
                {
                    switch(cardData.applyEffect[i])
                    {
                        case Card.Effect.MELT:
                        foundEffectTrue = false;
                        for(int b = 0; b < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; b++)
                        {
                            if(currentEnemy.GetComponent<UnitTracker>().activeEffects[b] == Card.Effect.MELT)
                            {
                                currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[b] += cardData.effectSeverity[i];
                                foundEffectTrue = true;
                                break;
                            }
                        }
                        if(foundEffectTrue == false)
                        {
                            currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.MELT);
                            currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(cardData.effectSeverity[i]);
                        }
                        break;

                        case Card.Effect.DRAIN:
                            foundEffectTrue = false;
                            for(int b = 0; b < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; b++)
                            {
                                if(currentEnemy.GetComponent<UnitTracker>().activeEffects[b] == Card.Effect.DRAIN)
                                {
                                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[b] += cardData.effectSeverity[i];
                                    foundEffectTrue = true;
                                    break;
                                }
                            }
                            if(foundEffectTrue == false)
                            {
                                currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.DRAIN);
                                currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(cardData.effectSeverity[i]);
                            }

                        break;

                        case Card.Effect.STUN:

                        break;

                        case Card.Effect.AAA:
                            RoundEnergy += cardData.effectSeverity[i];
                            //Maybe instantiate a particle system here that bursts a bunch of lightning and deactivates again?
                        break;
                        case Card.Effect.DISCARDTOENERGY:
                            RoundEnergy += DiscardsThisTurn;
                            DiscardsThisTurn = 0;
                        break;
                        case Card.Effect.ENERGYTODISCARD:
                            DiscardsThisTurn += RoundEnergy;
                            RoundEnergy = 0;
                        break;

                        case Card.Effect.SELFDESTRUCT:
                            MainMech.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.SELFDESTRUCT);
                            MainMech.GetComponent<UnitTracker>().activeEffectDuration.Add(cardData.effectSeverity[i]);
                        break;

                        case Card.Effect.RECURSIVE:
                            int temp = currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1];
                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] = currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2];
                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[2] = temp;
                            //Play weird sound effect, Instantiate Particle system or cool camera overlay (Purple action lines on the sides of the screen with opacity fadeout?)
                        break;

                        case Card.Effect.NON:
                        break;

                        case Card.Effect.DRAW:
                            DrawRandomCard(cardData.effectSeverity[i]);
                        break;
                        
                        case Card.Effect.DOUBLEDMG:
                            roundHackDmgOutput = roundHackDmgOutput*cardData.effectSeverity[i];
                            roundPhyDmgOutput = roundPhyDmgOutput*cardData.effectSeverity[i];
                            UpdateDMGCounters();
                        break;    

                        case Card.Effect.DOUBLEDMGHACK:
                            roundHackDmgOutput = roundHackDmgOutput*cardData.effectSeverity[i];
                            UpdateDMGCounters();
                        break;    

                        case Card.Effect.DOUBLEDMGPHY:
                            roundPhyDmgOutput = roundPhyDmgOutput*cardData.effectSeverity[i];
                            UpdateDMGCounters();
                        break;  

                        case Card.Effect.SELFHARM:
                            MainMech.GetComponent<UnitTracker>().healthBarCurrentValues[0] -= cardData.effectSeverity[i];
                            MainMech.GetComponent<BattleHUD>().FullUpdateCheck();
                            //special effects of damage
                        break; 

                        case Card.Effect.ENEMYSWITCH:
                            HealthbarSwitch(currentEnemy);
                        break;

                        case Card.Effect.SELFSWITCH:
                                HealthbarSwitch(MainMech);
                        break;

                        case Card.Effect.DRAWPHYIFSHIELD:
                            MainMech.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.DRAWPHYIFSHIELD);
                            MainMech.GetComponent<UnitTracker>().activeEffectDuration.Add(cardData.effectSeverity[i]);
                        break;

                        case Card.Effect.DRAWHACKIFPHY:
                            MainMech.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.DRAWHACKIFPHY);
                            MainMech.GetComponent<UnitTracker>().activeEffectDuration.Add(cardData.effectSeverity[i]);
                        break;
                        case Card.Effect.PURGE:
                            for(int j = 1; j <= MainMech.GetComponent<UnitTracker>().activeEffects.Count; j++)
                            {
                                MainMech.GetComponent<UnitTracker>().activeEffects.RemoveAt(MainMech.GetComponent<UnitTracker>().activeEffects.Count-j);
                                MainMech.GetComponent<UnitTracker>().activeEffects.RemoveAt(MainMech.GetComponent<UnitTracker>().activeEffectDuration.Count-j);
                            }
                        break;
                        case Card.Effect.PILEDRIVER:
                            bool found_piledriver = false;

                            for(int b = 0; b < MainMech.GetComponent<UnitTracker>().activeEffects.Count; b++)
                            {
                                if(MainMech.GetComponent<UnitTracker>().activeEffects[b] == Card.Effect.PILEDRIVER)
                                {
                                    MainMech.GetComponent<UnitTracker>().activeEffectDuration[b] += 1;
                                    found_piledriver = true;
                                    break;
                                }
                            }
                            if(found_piledriver == false)
                            {
                                MainMech.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.PILEDRIVER);
                                MainMech.GetComponent<UnitTracker>().activeEffectDuration.Add(1);
                            }

                        break;

                        case Card.Effect.ODAY:
                            for(int b = 0; b < MainMech.GetComponent<UnitTracker>().activeEffects.Count; b++)
                            {
                                if(MainMech.GetComponent<UnitTracker>().activeEffects[b] == Card.Effect.ODAY)
                                {
                                    MainMech.GetComponent<UnitTracker>().activeEffectDuration[b] += 1;
                                    foundEffectTrue = true;
                                    break;
                                }
                            }
                            if(foundEffectTrue == false)
                            {
                                currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.ODAY);
                                currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(1);
                            }
                        break;
                    }
                }
                UpdateUIEffects();
                
            }
        
    }
    public void UpdateUIEffects()
    {
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
        UpdateDrawDiscard();
        MainMech.GetComponentInChildren<EffectIconLogic>().UpdateIcons();
        currentEnemy.GetComponentInChildren<EffectIconLogic>().UpdateIcons();
        MainMech.GetComponent<BattleHUD>().FullUpdateCheck();
        currentEnemy.GetComponent<BattleHUD>().FullUpdateCheck();
        
    }


Card.DamageType currentAttackingType;

    void EnemyTurn()
    {
        print("Enemies Turn is now");
        bool isAlreadyAttacking = false;
        bool isAlreadySwitching = false;
        //four random values: Attack Dmg, Attack Type, turns till switch, turns till damage
        for (int i = 0; i < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; i++)
        {
            if(currentEnemy.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.ENEMYATTACK)
            {
                isAlreadyAttacking = true;
                currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                if(currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] <= 0)
                    {
                        DealDamage(MainMech, UnityEngine.Random.Range(currentEnemy.GetComponent<UnitTracker>().unitData.minDamage,currentEnemy.GetComponent<UnitTracker>().unitData.maxDamage), currentAttackingType);
                        currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i); 
                    }     
            }   
        } 
        for (int i = 0; i < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; i++)
        {
            if(currentEnemy.GetComponent<UnitTracker>().activeEffects[i] == Card.Effect.ENEMYSWITCH)
                {
                    isAlreadySwitching = true;
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                    if(currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] <= 0)
                    {
                        HealthbarSwitch(currentEnemy);
                        currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(i);
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(i);
                    }
                }
        }
        if(isAlreadyAttacking == false)
            {
                //if already attacking is false start new attack counter, randomise attack 2-4 turns away
                currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.ENEMYATTACK);
                currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(UnityEngine.Random.Range(1,4));
                //determining the attacking Damage type giving both a flat 50-50 chance 
                int rand = UnityEngine.Random.Range(1,100);
                if(rand + currentEnemy.GetComponent<UnitTracker>().unitData.PreferenceOffset < 50)
                {
                    currentAttackingType = Card.DamageType.PHY;
                    currentEnemy.GetComponent<UnitTracker>().isAttackPhy = true;
                }
                else
                {
                    currentAttackingType = Card.DamageType.HACK;
                    currentEnemy.GetComponent<UnitTracker>().isAttackPhy = false;
                }
                
            }
            if(isAlreadySwitching == false)
            {
                //if already switching is false start new switch counter, randomise switch timer 1-3 turns away
                switch(UnityEngine.Random.Range(1,3))
                {
                    case 1:
                        //doesn't initiate a switch
                    break;
                    case 2:
                        currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.ENEMYSWITCH);
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(UnityEngine.Random.Range(1,2));
                        
                    break;
                    case 3:
                        currentEnemy.GetComponent<UnitTracker>().activeEffects.Add(Card.Effect.ENEMYSWITCH);
                        currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.Add(UnityEngine.Random.Range(1,3));
                    break;

                }
            }
            UpdateUIEffects();

    }
[SerializeField] GameObject OutsideGameCanvas;
public void InstantiateCardSelection(int selectionNr, int choiceNr)
    { 
        if(selectionNr >= choiceNr)
        {
            GameObject tempPrefab = Instantiate(SelectNewCardPrefab, OutsideGameCanvas.transform);
            tempPrefab.GetComponent<NewCardSelector>().cardAmount = selectionNr;
            tempPrefab.GetComponent<NewCardSelector>().cardsToChoose = choiceNr;
        }
        else
        {
            print("ERROR: More choices than Cards available, Not Instatiating Card Selection like that");
        }
    }
    void HealthbarSwitch(GameObject Mech)
    {
        switch(Mech.GetComponent<UnitTracker>().activeHealthbar)
            {
                case 0:
                    Mech.GetComponent<UnitTracker>().activeHealthbar = 1;
                    Mech.GetComponent<BattleHUD>().MainHealthbar(1);
                break; 
                case 1:
                    Mech.GetComponent<UnitTracker>().activeHealthbar = 0;
                    Mech.GetComponent<BattleHUD>().MainHealthbar(0);
                break;
            }
    }
void DealDamage(GameObject Target, int dmg, Card.DamageType DamageType)
    {
        GameObject tempDmgNumber;
        GameObject tempDmgNumber2;
        GameObject location;
        UnitTracker TargetUnit = Target.GetComponent<UnitTracker>();
        if(Target == currentEnemy)
        {
            location = EnemyMechLocation;
        }
        else
        {
            location = MainMechLocation;
        }
        switch(TargetUnit.activeHealthbar)
        {
            case 0:
                if(DamageType != Card.DamageType.PHY)
                {
                    dmg = dmg/2;
                }

                TargetUnit.healthBarCurrentValues[1] -= dmg;



                if(TargetUnit.healthBarCurrentValues[1] < 0)
                     {
                        dmg = -TargetUnit.healthBarCurrentValues[1];
                        if(DamageType != Card.DamageType.PHY)
                        {
                            dmg = dmg*2;
                        }
                        else
                        {
                            dmg = dmg/2;
                        }

                        TargetUnit.healthBarCurrentValues[2] -= dmg;

                        if(TargetUnit.healthBarCurrentValues[2] < 0)
                        {
                            tempDmgNumber = Instantiate(DMGNr, location.transform);
                            tempDmgNumber.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[2]-TargetUnit.healthBarCurrentValues[1]).ToString();
                            tempDmgNumber.GetComponent<TMP_Text>().color = Color.green;
                            if(DamageType == Card.DamageType.PHY)
                            {
                                dmg = -TargetUnit.healthBarCurrentValues[2]*2;
                            }
                            else
                            {
                                dmg = -TargetUnit.healthBarCurrentValues[2];
                            }

                            TargetUnit.healthBarCurrentValues[0] -= dmg;

                            tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[2]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;

                            TargetUnit.healthBarCurrentValues[2] = 0;
                        }
                        else
                        {
                            tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[1]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                        }


                        TargetUnit.healthBarCurrentValues[1] = 0;
                    }
                else
                {
                        tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = dmg.ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                }
            break;

            case 1:
                if(DamageType != Card.DamageType.HACK)
                {
                    dmg = dmg/2;
                }

                TargetUnit.healthBarCurrentValues[2] -= dmg;



                if(TargetUnit.healthBarCurrentValues[2] < 0)
                     {
                        dmg = -TargetUnit.healthBarCurrentValues[2];
                        if(DamageType != Card.DamageType.HACK)
                        {
                            dmg = dmg*2;
                        }
                        else
                        {
                            dmg = dmg/2;
                        }

                        TargetUnit.healthBarCurrentValues[1] -= dmg;

                        if(TargetUnit.healthBarCurrentValues[1] < 0)
                        {
                            tempDmgNumber = Instantiate(DMGNr, location.transform);
                            tempDmgNumber.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[1]-TargetUnit.healthBarCurrentValues[2]).ToString();
                            tempDmgNumber.GetComponent<TMP_Text>().color = Color.red;
                            
                            if(DamageType == Card.DamageType.HACK)
                                    {
                                        dmg = -TargetUnit.healthBarCurrentValues[1]*2;
                                    }
                                    else
                                    {
                                        dmg = -TargetUnit.healthBarCurrentValues[1];
                                    }

                                    TargetUnit.healthBarCurrentValues[0] -= dmg;

                            tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[1]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;

                            TargetUnit.healthBarCurrentValues[1] = 0;
                        }
                        else
                        {
                            tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                            tempDmgNumber2.GetComponent<TMP_Text>().text = (-TargetUnit.healthBarCurrentValues[2]).ToString();
                            tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;
                        }


                        TargetUnit.healthBarCurrentValues[2] = 0;
                    }
                else
                {
                        tempDmgNumber2 = Instantiate(DMGNr, location.transform);
                        tempDmgNumber2.GetComponent<TMP_Text>().text = dmg.ToString();
                        tempDmgNumber2.GetComponent<TMP_Text>().color = Color.green;
                }
            break;
        }
        
    }
[YarnCommand("RepairMech")]
public void RepairMainMech(int options, int percentage)
    {
        /*//Repair Hull, press 1
        if(options == 0)
        {
            MainMech.GetComponent<UnitTracker>().HealMech(0, 100);
        }
        //Repair Armor and Shield press 2
        else if(options == 1)
        {
            MainMech.GetComponent<UnitTracker>().HealMech(1, 30);
        }
        else if(options == 2)
        {
            MainMech.GetComponent<UnitTracker>().HealMech(2, 30);
        }*/
        MainMech.GetComponent<UnitTracker>().HealMech(options, percentage);
    }
public void InstantiateCommonEnemy(Unit enemy)
    {
        ClearEnemyVisuals();
        currentEnemy.GetComponent<UnitTracker>().unitData = enemy;
        Instantiate(enemy.VisualsPrefab, currentEnemy.transform.GetChild(0));
        StartofCombat();
    }

public void ClearEnemyVisuals()
    {
        print("CHILDCOUNT: "+ currentEnemy.transform.GetChild(0).childCount);
        for(int i = 1; i <= currentEnemy.transform.GetChild(0).childCount; i++)
        {
            if(currentEnemy.transform.GetChild(0).childCount != 0)
            {
                Debug.Log("Clearing Enemy Visuals: " + currentEnemy.transform.GetChild(0).GetChild(0).gameObject);
                Destroy(currentEnemy.transform.GetChild(0).GetChild(currentEnemy.transform.GetChild(0).childCount - i).gameObject);
            }
        }
    }
IEnumerator EndBattle()
    {
        if(MainMech.GetComponent<UnitTracker>().activeEffects.Count != 0)
        {
            for(int j = 1; j <= MainMech.GetComponent<UnitTracker>().activeEffects.Count; j++)
            {
                MainMech.GetComponent<UnitTracker>().activeEffects.RemoveAt(MainMech.GetComponent<UnitTracker>().activeEffects.Count-j);
                MainMech.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(MainMech.GetComponent<UnitTracker>().activeEffectDuration.Count-j);
            }
        }
        if(currentEnemy.GetComponent<UnitTracker>().activeEffects.Count != 0)
        {
            for(int j = currentEnemy.GetComponent<UnitTracker>().activeEffects.Count-1; j >= 0; j--)
            {
                    print("removing Enemy Components");
                    currentEnemy.GetComponent<UnitTracker>().activeEffects.RemoveAt(j);
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration.RemoveAt(j);
            }
        }
        
        discardPile.Clear();
        drawPile.Clear();
        roundHackDmgOutput = 0;
        roundPhyDmgOutput = 0;
        UpdateDMGCounters();
        UpdateUIEffects();
        UpdateDrawDiscard();
        
        //Activate deathAnimation here
        Animator deathAnimator = currentEnemy.transform.GetChild(0).GetComponentInChildren<Animator>();
            if(deathAnimator != null)
            {
                deathAnimator.SetTrigger("Death");
            }
        //Instantiate Particle System or something here! vvv
        int random_explosion_amount = UnityEngine.Random.Range(5,10);
        for(int i = 0; i < random_explosion_amount; i++)
        {
            
            float random_x = UnityEngine.Random.Range(-1,2);
            float random_y = UnityEngine.Random.Range(-3,2);
            float random_z = UnityEngine.Random.Range(-0.2f, 0.5f);
            UnityEngine.Vector3 rand_offset = new UnityEngine.Vector3(random_x, random_y, random_z);
            float random_wait = UnityEngine.Random.Range(0.1f,0.5f);
            yield return new WaitForSeconds(random_wait);
            GameObject tempObj = Instantiate(ParticleExplosionPrefab, currentEnemy.transform.GetChild(0)); 
            tempObj.transform.position = currentEnemy.transform.GetChild(0).position + rand_offset;
           //Instantiate(); 
        }
       
        
        for(int j = 1; j <= CardEmpty.transform.childCount; j++)
        {
            if(CardEmpty.transform.childCount != 0)
            {
                Destroy(CardEmpty.transform.GetChild(CardEmpty.transform.childCount-j).gameObject);
            }
            
        }

        yield return new WaitForSeconds(3);
        ClearEnemyVisuals();
        MainMech.GetComponent<BattleHUD>().FullUpdateCheck();
        //currentEnemy.GetComponent<BattleHUD>().FullUpdateCheck();
        UpdateUIEffects();
        currentEnemy.GetComponent<UnitTracker>().unitData = null;
        //run.SwitchScene(0);
        BattleWonPanel.SetActive(true);

    }
IEnumerator LoseRoutine()
    {
        discardPile.Clear();
        roundHackDmgOutput = 0;
        roundPhyDmgOutput = 0;
        UpdateDMGCounters();
        UpdateUIEffects();
        UpdateDrawDiscard();
        
        //Activate deathAnimation here
        Animator deathAnimator = MainMech.transform.GetChild(0).GetComponentInChildren<Animator>();
            if(deathAnimator != null)
            {
                deathAnimator.SetTrigger("Death");
            }
        int random_explosion_amount = UnityEngine.Random.Range(5,14);
        for(int i = 0; i < random_explosion_amount; i++)
        {
            
            float random_x = UnityEngine.Random.Range(-1,2);
            float random_y = UnityEngine.Random.Range(-3,2);
            float random_z = UnityEngine.Random.Range(-0.2f, 0.5f);
            UnityEngine.Vector3 rand_offset = new UnityEngine.Vector3(random_x, random_y, random_z);
            float random_wait = UnityEngine.Random.Range(0.1f,0.5f);
            yield return new WaitForSeconds(random_wait);
            GameObject tempObj = Instantiate(ParticleExplosionPrefab, MainMech.transform.GetChild(0)); 
            tempObj.transform.position = MainMech.transform.GetChild(0).position + rand_offset;
           //Instantiate(); 
        }
        yield return new WaitForSeconds(2);
        //Put Loss Consequences Down Here vvv
        GameOver.SetActive(true);

    }
IEnumerator WaitBetweenEndRoundAndNewRound()
    {
        yield return new WaitForSeconds(2);
        NewRoundCheck();
    }
[YarnCommand("EnergyGain")]
public void EnergyGain(int amount)
    {
        RoundlyEnergy += amount;
    }
[YarnCommand("DiscardGain")]
public void DiscardGain(int amount)
    {
        TurnlyDiscards += amount;
    }
[YarnCommand("UpgradeMechHp")]
public void UpgradeMechHp(int hpType, float percentile)
    {
        MainMech.GetComponent<UnitTracker>().unitData.healthBarMaxValues[hpType] = (int)(MainMech.GetComponent<UnitTracker>().unitData.healthBarMaxValues[hpType] * percentile);
    }
}