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
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        InstantiateCardSelection(true);
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
        }
            
    }
    void InstantiateCardSelection(bool isRandom)
    {
        UpdateString();
        if(isRandom == false)
        {
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
                Card tempCardData = gameManager.AllCardsInPlay[UnityEngine.Random.Range(0, (gameManager.AllCardsInPlay.Count-1))];
                card.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                card.GetComponent<CardDisplayScript>().cardData = tempCardData;
                card.GetComponent<CardPlayScript>().cardData = tempCardData;
                card.GetComponent<CardPlayScript>().isCardPlayable = false;
                card.GetComponent<CardPlayScript>().isCardPickable = true;
            }
        }
    }
}
