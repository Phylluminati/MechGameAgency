using System.Collections.Generic;
using CardRunner;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;

public class SelectCardsToDiscard : MonoBehaviour
{
    private GameManagerScript gameManager;
    public List<Card> pickedCards;
    public int cardsToChoose = 2;
    public int cardAmount = 6;
    public bool isRandom = true;
    private bool _initiationComplete = false;
    public TMPro.TMP_Text PickNumber;
    public Card.CardType typeChoice = Card.CardType.NULL;
    public Button button;
    public float ChosenAmount = 0;
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        InstantiateCardSelection(); 
    }

  void Update()
    {
        if(_initiationComplete == false)
        {
            if(this.transform.parent.gameObject.transform.localScale.x < 1)
            {
                this.transform.parent.gameObject.transform.localScale += (new Vector3(6f, 0, 0) * Time.deltaTime);
            }
            else if(this.transform.parent.gameObject.transform.localScale.y < 1)
            {
                this.transform.parent.gameObject.transform.localScale += (new Vector3(0, 6f, 0) * Time.deltaTime);
            }
            else
            {
                _initiationComplete = true;
            }
        }
    }
public void UpdateString()
    {
        PickNumber.text = cardsToChoose.ToString();
        if(cardsToChoose > 1)
        {
            PickNumber.color = Color.green;
        }
        else if(cardsToChoose == 1)
        {
            PickNumber.color = Color.yellow;
        }
        else
        {
            PickNumber.color = Color.red;
            button.interactable = true;
        }
            
    }
void InstantiateCardSelection()
    {
        UpdateString();
        /*if(typeChoice != Card.CardType.NULL)
        {
            for(int i = 0; i < cardAmount; i++)
            {
                
                Card.CardType temp_card_data = Card.CardType.NULL;
                Card temp_card = null;
                while(temp_card_data != typeChoice)
                {
                    temp_card = gameManager.currentDeck[UnityEngine.Random.Range(0, (gameManager.AllCardsInPlay.Count-1))];
                    temp_card_data = temp_card.cardType;
                }
                GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
                card.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                card.GetComponent<CardDisplayScript>().cardData = temp_card;
                card.GetComponent<CardPlayScript>().cardData = temp_card;
                card.GetComponent<CardPlayScript>().isCardPlayable = false;
                card.GetComponent<CardPlayScript>().isCardPickable = true;
            }
        }
        else
        {*/
            //card.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
            List<int> previousPicks = new List<int>();
            previousPicks.Add(gameManager.currentDeck.Count+1);
            bool is_previous_pick = true;
            Card temp_card_data = null;
            int random = 0;

            for(int i= 0; i < cardAmount; i++)
            {
                while(is_previous_pick == true)
                {
                    is_previous_pick = false;
                    random = UnityEngine.Random.Range(0, (gameManager.currentDeck.Count));
                    temp_card_data = gameManager.currentDeck[random];
                    print("Random Card: "+ temp_card_data + " || Random Int: " + random);
                    for(int j = 0; j < previousPicks.Count; j++)
                    {
                        if(random == previousPicks[j])
                        {
                            is_previous_pick = true;
                        }
                    }
                }
                previousPicks.Add(random);

                GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
                card.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                card.GetComponent<CardDisplayScript>().cardData = temp_card_data;
                card.GetComponent<CardPlayScript>().cardData = temp_card_data;
                card.GetComponent<CardPlayScript>().isCardPlayable = false;
                card.GetComponent<CardPlayScript>().isCardPickable = true;
                is_previous_pick = true;
            }
        //}
    }
    public void RemoveSelectionFromDeck()
    {
        bool is_found = false;
        //gameManager.currentDeck.AddRange(pickedCards);
        for(int i = 0; i < pickedCards.Count; i++)
        {
            for(int j = 0; j < gameManager.currentDeck.Count; j++)
            {
                if(is_found == false && pickedCards[i] == gameManager.currentDeck[j])
                {
                    gameManager.currentDeck.RemoveAt(j);
                    pickedCards.RemoveAt(i);
                    i--;
                    j--;
                    is_found = true;
                }
            }
            is_found = false;
        }
        InMemoryVariableStorage memory = this.transform.root.GetComponentInChildren<InMemoryVariableStorage>();
        memory.SetValue("$ChosenAmount", ChosenAmount);
        gameManager.gameObject.GetComponent<DialogueManager>().ResetDialogueCanvas();
        
        Destroy(this.transform.parent.gameObject);
    }
}

