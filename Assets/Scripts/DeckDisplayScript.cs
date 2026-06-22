using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DeckDisplayScript : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    public void OnOpen()
    {
        for( int i=this.transform.childCount-1; i>0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < gameManager.GetComponent<GameManagerScript>().currentDeck.Count; i++)
        {
            GameObject card = Instantiate(gameManager.GetComponent<GameManagerScript>().cardPrefab, this.transform);
            card.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
            card.GetComponent<CardDisplayScript>().cardData = gameManager.GetComponent<GameManagerScript>().currentDeck[i];
            card.GetComponent<CardPlayScript>().isCardPlayable = false;
        }
        
    }

    public void OnClose()
    {
        for(int i = 0; i < gameManager.GetComponent<GameManagerScript>().currentDeck.Count; i++)
        {
            Destroy(this.transform.GetChild(i+1));
        }
    }
    
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
