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
    public List<Card> cardsPlayedThisTurn;
    public List<Card> drawPile;
    public List<Card> discardPile;
    [SerializeField] GameObject CardEmpty;
    [SerializeField] TMP_Text DrawNumber;
    [SerializeField] TMP_Text DiscardNumber;
    //DMGNr is the PreFab for the Damage Numbers that pop up when inflicting damage of any kind
    [SerializeField] GameObject DMGNr;

    //the EnemyMech and Mainmech locations are empty gameObjects to determine the Mechs location on the canvas so UI effects can be instantiated in the right positions
    [SerializeField] GameObject EnemyMechLocation;
    [SerializeField] GameObject MainMechLocation;

    private GameObject _PHYDMGCounter;
    private GameObject _HACKDMGCounter;

    public bool mainShieldActive;
    public bool enemyShieldActive;


    public GameObject cardPrefab;
    public int roundHackDmgOutput;
    public int roundPhyDmgOutput;

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
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
        GenerateDrawPile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewRoundCheck()
    {
        //check off Effects on player (MELT does dmg at the beginning of Players turn)

       if(drawPile.Count == 0)
        {
            GenerateDrawPile(); 
        }
        // if I wanna add an effect that carries over energy between rounds I need to implement it right before the round energy gets reset to 4
        RoundEnergy = 4;
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);

        UpdateUIEffects();
    }
    public void EndOfRoundCheck()
    {
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
        //Apply Round damage
        //Check off Enemy Effects
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
            for(int i = 0; i < MainMech.GetComponent<UnitTracker>().activeEffects.Count; i++)
            {
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
                }  
            }
        
        roundHackDmgOutput = 0;
        roundPhyDmgOutput = 0;
        UpdateDMGCounters();
    }
        
        
        MainMech.GetComponent<BattleHUD>().FullUpdateCheck();
        currentEnemy.GetComponent<BattleHUD>().FullUpdateCheck();
        UpdateUIEffects();

        // ---
        //New round begins after End of Round and Enemy turn
        NewRoundCheck();
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
    public void PlayCardEffect(Card cardData)

    {
        //this method is responsible for causing card effects to happen, 
        // it will read Carddata from played/discarded cards, that got played with the DiscardCard() method in the carddisplayscript
        print("Card: " + cardData.cardName + "/n CardType: " + cardData.cardType);


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
                            roundHackDmgOutput = roundHackDmgOutput*2;
                            roundPhyDmgOutput = roundPhyDmgOutput*2;
                        break;    

                        case Card.Effect.DOUBLEDMGHACK:
                            roundHackDmgOutput = roundHackDmgOutput*2;
                        break;    

                        case Card.Effect.DOUBLEDMGPHY:
                            roundPhyDmgOutput = roundPhyDmgOutput*2;
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
                    }
                }
                UpdateUIEffects();
                
            }
        
    }
    public void UpdateUIEffects()
    {
        this.GetComponent<ENTracker>().UpdateEN(RoundEnergy);
        MainMech.GetComponentInChildren<EffectIconLogic>().UpdateIcons();
        currentEnemy.GetComponentInChildren<EffectIconLogic>().UpdateIcons();
        
    }

    void GameOver()
    {
        
    }

    void GameWin()
    {
        
    }

Card.DamageType currentAttackingType;
    void EnemyTurn()
    {
        //four random values: Attack Dmg, Attack Type, turns till switch, turns till damage
        for (int i = 03; i < currentEnemy.GetComponent<UnitTracker>().activeEffects.Count; i++)
        {
            switch(currentEnemy.GetComponent<UnitTracker>().activeEffects[i])
            {
                case Card.Effect.ENEMYATTACK:
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                    if(currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] <= 0)
                    {
                        
                        DealDamage(MainMech, UnityEngine.Random.Range(currentEnemy.GetComponent<UnitTracker>().unitData.minDamage,currentEnemy.GetComponent<UnitTracker>().unitData.maxDamage), currentAttackingType);
                    }
                break;
                case Card.Effect.ENEMYSWITCH:

                break;
            }
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
}