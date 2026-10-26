using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CardRunner;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class DeckDisplayScript : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;
    enum DisplayChoice
    {
        Current,
        Discard,
        Draw,
        All,
        Unassigned
    }
    [SerializeField]
    DisplayChoice choice = DisplayChoice.Unassigned;
    List<Card> displayChoice;
    public void OnOpen()
    {
        switch(choice)
        {
            case DisplayChoice.Current: 
                displayChoice = gameManager.GetComponent<GameManagerScript>().currentDeck;
            break;
            case DisplayChoice.Discard:
                displayChoice = gameManager.GetComponent<GameManagerScript>().discardPile;
            break;
            case DisplayChoice.Draw:
                displayChoice = gameManager.GetComponent<GameManagerScript>().drawPile;
            break;
            case DisplayChoice.All:
                displayChoice = gameManager.GetComponent<GameManagerScript>().AllCardsInPlay;

            break;
            case DisplayChoice.Unassigned:
                Debug.LogWarning("Display Deck Unassigned when Booting up, you have to pick between Draw, Discard, All and Current in the Serialized Enumerator");
                return;
            
        }
        
        for( int i=this.transform.childCount-1; i>0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < displayChoice.Count; i++)
        {
            GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
            card.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
            card.GetComponent<CardDisplayScript>().cardData = displayChoice[i];
            card.GetComponent<CardPlayScript>().isCardPlayable = false;
        }
        
    }

    /*public void OnClose()
    {
        for(int i = 0; i < gameManager.GetComponent<GameManagerScript>().currentDeck.Count; i++)
        {
            Destroy(this.transform.GetChild(i+1));
        }
    }*/
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    void OnEnable()
    {
        OnOpen();
    }
    void OnDisable()
    {}
    // Update is called once per frame
    void Update()
    {
        
    }
}
