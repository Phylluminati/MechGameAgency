using UnityEngine;
using CardRunner;

public class CardPlayScript : MonoBehaviour
{
    private GameObject _gameManager;
    private GameObject _card;
    public Card cardData;
    public bool isCardPlayable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     _card = this.gameObject;
     _gameManager = GameObject.FindWithTag("GameManager");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiscardCard()
    {
        if(isCardPlayable == true)
        {
            if(_gameManager.GetComponent<GameManagerScript>().RoundEnergy >= cardData.cost)
            {
                print("Discarding Card to discardpile");
                //_gameManager.GetComponent<GameManagerScript>().UpdateDMGCounters();
                Destroy(this.gameObject);
                _gameManager.GetComponent<GameManagerScript>().RoundEnergy -= cardData.cost;
                if(_gameManager.GetComponent<GameManagerScript>().RoundEnergy < 0)
                {
                    _gameManager.GetComponent<GameManagerScript>().RoundEnergy = 0;
                }
                _gameManager.GetComponent<GameManagerScript>().discardPile.Add(cardData);
                _gameManager.GetComponent<GameManagerScript>().UpdateDrawDiscard();
                _gameManager.GetComponent<GameManagerScript>().PlayCardEffect(cardData);
            }
            else
            {
                print("This card cannot be played due to its cost");
            }
        }
    }
}
