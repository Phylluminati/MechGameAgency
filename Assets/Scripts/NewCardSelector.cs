using System.Collections.Generic;
using CardRunner;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class NewCardSelector : MonoBehaviour
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
    DialogueManager dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        dialogue = gameManager.gameObject.GetComponent<DialogueManager>();
        InstantiateCardSelection();
    }

    // Update is called once per frame
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
    public void AddSelectionToDeck()
    {
        gameManager.currentDeck.AddRange(pickedCards);
        dialogue.ResetDialogueCanvas();
        Destroy(this.transform.parent.gameObject);
    }
    public void skip()
    {
        gameManager.currentDeck.AddRange(pickedCards);
        dialogue.ResetDialogueCanvas();
        Destroy(this.transform.parent.gameObject);
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
            button.interactable = true ;
        }
            
    }
    void InstantiateCardSelection()
    {
        UpdateString();
        if(typeChoice != Card.CardType.NULL)
        {
            for(int i = 0; i < cardAmount; i++)
            {
                
                Card.CardType temp_card_data = Card.CardType.NULL;
                Card temp_card = null;
                Card previous_card = null;
                while(temp_card_data != typeChoice)
                {
                    temp_card = gameManager.AllCardsInPlay[UnityEngine.Random.Range(0, (gameManager.AllCardsInPlay.Count))];
                    temp_card_data = temp_card.cardType;
                }
                //previous_card = temp_card;
                GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
                card.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                card.GetComponent<CardDisplayScript>().cardData = temp_card;
                card.GetComponent<CardPlayScript>().cardData = temp_card;
                card.GetComponent<CardPlayScript>().isCardPlayable = false;
                card.GetComponent<CardPlayScript>().isCardPickable = true;
            }
            /*GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
            card.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
            card.GetComponent<CardDisplayScript>().cardData = gameManager.GetComponent<GameManagerScript>().currentDeck[i];
            card.GetComponent<CardPlayScript>().isCardPlayable = false;*/
        }
        else
        {
            //card.transform.localScale = new Vector3 (0.5f, 0.5f, 1);

            for(int i= 0; i < cardAmount; i++)
            {
                GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
                Card temp_card_data = gameManager.AllCardsInPlay[UnityEngine.Random.Range(0, (gameManager.AllCardsInPlay.Count-1))];
                card.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                card.GetComponent<CardDisplayScript>().cardData = temp_card_data;
                card.GetComponent<CardPlayScript>().cardData = temp_card_data;
                card.GetComponent<CardPlayScript>().isCardPlayable = false;
                card.GetComponent<CardPlayScript>().isCardPickable = true;
            }
        }
    }
}
