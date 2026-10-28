using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CardRunner;
using NUnit.Framework.Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayScript : MonoBehaviour
{
    private GameObject _card;
    public Card cardData;

    [SerializeField]
    private Sprite[] energySprites;
    private GameObject _gameManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _card = this.gameObject;
        UpdateCardName();
        _gameManager = GameObject.FindWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateCardName()
    {
       _card.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = cardData.cardImage;
       _card.transform.GetChild(1).GetComponent<TMP_Text>().text = cardData.cardName;
       _card.transform.GetChild(2).GetComponent<TMP_Text>().text = cardData.cardDescriptor;
       if(cardData.dmg <= 0)
        {
            _card.transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            _card.transform.GetChild(3).GetComponent<TMP_Text>().text = cardData.dmg.ToString();
        }
        switch(cardData.cost)
        {
            case 4:
            _card.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = energySprites[4];
            break;

            case 3:
            _card.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = energySprites[3];
            break;

            case 2:
            _card.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = energySprites[2];
            break;

            case 1:
            _card.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = energySprites[1];
            break;

            case 0:
            _card.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = energySprites[0];
            break;
           
            default:
            print("sth went wrong with Card Cost");
            break;
        }
        switch(cardData.cardType)
        {
            case Card.CardType.PHY:
                _card.transform.GetChild(5).GetComponent<TMP_Text>().text = Card.CardType.PHY.ToString();
                _card.transform.GetChild(5).GetComponent<TMP_Text>().color = Color.red;
                break;
            case Card.CardType.HACK:
                _card.transform.GetChild(5).GetComponent<TMP_Text>().text = Card.CardType.HACK.ToString();
                _card.transform.GetChild(5).GetComponent<TMP_Text>().color = Color.green;
                break;
            case Card.CardType.MULT:
                _card.transform.GetChild(5).GetComponent<TMP_Text>().text = Card.CardType.MULT.ToString();
                _card.transform.GetChild(5).GetComponent<TMP_Text>().color = Color.black;
                break;
            case Card.CardType.RUST:
                _card.transform.GetChild(5).GetComponent<TMP_Text>().text = Card.CardType.RUST.ToString();
                _card.transform.GetChild(5).GetComponent<TMP_Text>().color = Color.purple;
                break;
        }
    }
}
