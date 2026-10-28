using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn;
using Yarn.Unity;

public class Run : MonoBehaviour
{
    [SerializeField] GameObject Game;
    [SerializeField] GameObject Map;
    [SerializeField] GameObject Menu;
    [SerializeField] List<Unit> NormalEnemies;
    [SerializeField] Unit Boss;
    GameObject GameManagerObject;
    DialogueRunner dialogue;
    List<bool> playedEncounters = new List<bool>(){false, false, false, false, false, false, false, false, false, false, false, false};
    List<int> Repeatables = new List<int>(){3,1,5};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManagerObject = GameObject.Find("GameManagerObject");
        dialogue = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartRun()
    {
        
    }
    //0 is Map Menu, 1 is Game Menu, 2 is Main Menu
    public void SwitchScene(int switch_scene_number)
    {
        GameManagerObject.GetComponent<DialogueManager>().FadeIn();
        StartCoroutine(SceneSwitchRoutine(2, switch_scene_number));
    }
    IEnumerator SceneSwitchRoutine(float duration, int switch_scene_number)
    {
        DialogueManager dia = GameManagerObject.GetComponent<DialogueManager>();
        yield return new WaitForSeconds(duration);
        switch(switch_scene_number)
        {
            case 0:
                
                //TestRoutine(2);
                Game.SetActive(false);
                Map.SetActive(true);
                if(Map.GetComponentInChildren<Map>().LastRoom != null)
                {
                    Map.GetComponentInChildren<Map>().UnlockNextRoom();
                }
                
                
            break;
            case 1:
                //TestRoutine(2);
                Map.SetActive(false);
                Game.SetActive(true);
            break;
            case 2:

            break;
            default:

            break;
        }
        dia.FadeOut();
    }
    IEnumerator RoomRoutine(float duration, Encounter room)
{
    Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
    yield return new WaitForSeconds(duration);
    Debug.Log($"Ended at {Time.time}"); 

    switch(room.type)
        {
            case Encounter.Type.CARDS:
                //Map.SetActive(false);
                //Game.SetActive(true);
                //dialogue.StartDialogue("Cards");
                GameManagerObject.GetComponent<DialogueManager>().RewardCards(8, 3);
            break;

            case Encounter.Type.ENEMY:
                Map.SetActive(false);
                Game.SetActive(true);
                int rand = UnityEngine.Random.Range(0, NormalEnemies.Count);
                Unit selectedEnemy = NormalEnemies[rand];
                GameManagerObject.GetComponent<GameManagerScript>().InstantiateCommonEnemy(selectedEnemy);
                //Implement Array of Usual Enemy Types here.
            break;

            case Encounter.Type.ENCOUNTER:
                Map.SetActive(false);
                Game.SetActive(true);
                bool is_repeatable = false;
                bool go_ahead = false;
                while(go_ahead == false)
                {
                    //Random range Max should be the max amount of random encounters there can be 
                    int random = UnityEngine.Random.Range(0,5);
                    for(int j = 0; j < Repeatables.Count; j++)
                        {
                            if(Repeatables[j] == random)
                            {
                                is_repeatable = true;
                            }
                        }
                        if(playedEncounters[random] == false || is_repeatable == true)
                        {
                            playedEncounters[random] = true;
                            dialogue.StartDialogue("RandomEncounter"+random.ToString());
                            go_ahead = true;
                        }
                        
                    is_repeatable = false;
                }
                
            break;

            case Encounter.Type.BOSS:
                Map.SetActive(false);
                Game.SetActive(true);
                GameManagerObject.GetComponent<GameManagerScript>().InstantiateCommonEnemy(Boss);
                GameManagerObject.GetComponent<GameManagerScript>().isFinalBoss = true;
            break;

            case Encounter.Type.SHOP:
                Map.SetActive(false);
                Game.SetActive(true);
                dialogue.StartDialogue("SHOP");
            break;
        }
    GameManagerObject.GetComponent<DialogueManager>().FadeOut();
}
    public void InitiateRoom(Encounter room)
    {
        DialogueManager dia = GameManagerObject.GetComponent<DialogueManager>();
        dia.FadeIn();
        StartCoroutine(RoomRoutine(2, room));
        
    }
}
