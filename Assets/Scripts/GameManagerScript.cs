using System;
using System.Collections.Generic;
using UnityEngine;
using CardRunner;
using TMPro;
using System.Data;

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
    public List<Card> currentDeck;
    public List<Card> cardsPlayedThisTurn;
    public List<Card> drawPile;
    public List<Card> discardPile;
    [SerializeField] GameObject CardEmpty;
    [SerializeField] TMP_Text DrawNumber;
    [SerializeField] TMP_Text DiscardNumber;
    [SerializeField] GameObject DMGNr;
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
    }
    public void EndOfRoundCheck()
    {
        //Apply Round damage
        //Check off Enemy Effects
        switch(currentEnemy.GetComponent<UnitTracker>().activeHealthbar)
        {
            case 0:
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundHackDmgOutput/2;
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundPhyDmgOutput;
            break;

            case 1:
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundHackDmgOutput;
                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= roundPhyDmgOutput/2;
            break;
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
                    tempDmgNumber.GetComponent<TMP_Text>().text = MELTFullDmg.ToString("0000");
                    tempDmgNumber.GetComponent<TMP_Text>().color = Color.red;
                    if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] > 0)
                        {
                            currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] -= MELTFullDmg;
                            if(currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] < 0)
                            {
                                GameObject tempDmgNumber2 = Instantiate(DMGNr, EnemyMechLocation.transform);
                                tempDmgNumber2.GetComponent<TMP_Text>().text = (-currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1]).ToString("0000");
                                tempDmgNumber2.GetComponent<TMP_Text>().color = Color.red;
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[0] += currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1];
                                currentEnemy.GetComponent<UnitTracker>().healthBarCurrentValues[1] = 0;
                            }
                        }
                    else
                        {
                            
                        }
                    currentEnemy.GetComponent<UnitTracker>().activeEffectDuration[i] -= 1;
                    break;

                    case Card.Effect.STUN:

                    break;

                    case Card.Effect.AAA:

                    break;

                    case Card.Effect.SELFDESTRUCT:

                    break;

                    case Card.Effect.RECURSIVE:

                    break;

                    case Card.Effect.NON:
                    break;

                    case Card.Effect.DRAW:
                        
                    break;
                    
                    case Card.Effect.DOUBLEDMG:

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
            for(int i = drawPile.Count -1; i > 0; i--)
            {
                drawPile.RemoveAt(i);
            }
        }
        for(int i = currentDeck.Count -1; i > 0; i--)
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
    public void PlayCardEffect(Card cardData)
    {
        //this method is responsible for causing card effects to happen, 
        // it will read Carddata from played/discarded cards, that got played with the DiscardCard() method in the carddisplayscript
        print("Card: " + cardData.cardName + "/n CardType: " + cardData.cardType);
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
            switch(cardData.applyEffect[0])
            {
                case Card.Effect.MELT:

                break;

                case Card.Effect.STUN:

                break;

                case Card.Effect.AAA:

                break;

                case Card.Effect.SELFDESTRUCT:

                break;

                case Card.Effect.RECURSIVE:

                break;

                case Card.Effect.NON:
                break;

                case Card.Effect.DRAW:
                    DrawRandomCard(cardData.effectSeverity[0]);
                break;
                
                case Card.Effect.DOUBLEDMG:

                break;    
            }
        }
    }
}