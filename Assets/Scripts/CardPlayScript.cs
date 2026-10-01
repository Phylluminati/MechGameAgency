using UnityEngine;
using CardRunner;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CardPlayScript : MonoBehaviour
{
    //private GameObject _gameManager;
    private GameManagerScript _gameManager;
    private GameObject _card;
    public Card cardData;
    public bool isCardPlayable = true;
    public bool isCardPickable = false;
    bool isAlreadyPicked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     _card = this.gameObject;
     _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiscardCard()
    {
        if(isCardPlayable == true)
        {
            if(_gameManager.RoundEnergy >= cardData.cost)
            {
                print("Discarding Card to discardpile");
                //_gameManager.GetComponent<GameManagerScript>().UpdateDMGCounters();
                Destroy(this.gameObject);
                _gameManager.RoundEnergy -= cardData.cost;
                if(_gameManager.RoundEnergy < 0)
                {
                    _gameManager.RoundEnergy = 0;
                }
                _gameManager.discardPile.Add(cardData);
                _gameManager.UpdateDrawDiscard();
                _gameManager.PlayCardEffect(cardData);
            }
            else
            {
                print("This card cannot be played due to its cost");
            }
        }
        else if(isCardPickable == true && isAlreadyPicked == false)
        {
            if (this.transform.parent.GetComponent<NewCardSelector>().cardsToChoose > 0)
            {
                this.transform.parent.GetComponent<NewCardSelector>().cardsToChoose -= 1;
                this.transform.parent.GetComponent<NewCardSelector>().UpdateString();
                this.transform.parent.GetComponent<NewCardSelector>().pickedCards.Add(this.cardData);
                this.gameObject.GetComponent<Image>().color = Color.yellow;
                this.transform.localScale = new Vector3 (1f, 1f, 1);
                //_gameManager.currentDeck.Add(this.cardData);
                isAlreadyPicked = true;
                //Destroy(this.gameObject);
            }
            else
            {
                //Destroy(this.transform.parent.gameObject);
                this.transform.parent.GetComponent<NewCardSelector>().button.interactable = true;
                //TODO MAKE THIS MAYBE INTERACTABLE AFTER TAKING ONLY ONE CARD AND MAKE THE COMPANY GUILT TRIP YOU IN AN EXTRA CONFIRM WINDOW THAT
                //YOU ARE NOT MAKING YOURSELF AS EFFICIENT FOR BUSINESS AS YOU COULD MAKE YOURSELF BE
            }
            if(this.transform.parent.GetComponent<NewCardSelector>().cardsToChoose == 0)
            {
                //Destroy(this.transform.parent.gameObject);
                this.transform.parent.GetComponent<NewCardSelector>().button.interactable = true;
            }
        }
        else if(isCardPickable == true && isAlreadyPicked == true)
        {
            for(int i = 0; i < this.transform.parent.GetComponent<NewCardSelector>().pickedCards.Count; i++)
            {
                if(this.transform.parent.GetComponent<NewCardSelector>().pickedCards[i] == this.cardData)
                {
                    this.transform.parent.GetComponent<NewCardSelector>().pickedCards.RemoveAt(i);
                    this.transform.parent.GetComponent<NewCardSelector>().cardsToChoose += 1;
                    this.transform.parent.GetComponent<NewCardSelector>().button.interactable = false;
                    this.gameObject.GetComponent<Image>().color = Color.gray;
                    this.transform.localScale = new Vector3 (0.75f, 0.75f, 1);
                    this.transform.parent.GetComponent<NewCardSelector>().UpdateString();
                    isAlreadyPicked = false;
                    break;
                }
            }
        }        
    }
}
