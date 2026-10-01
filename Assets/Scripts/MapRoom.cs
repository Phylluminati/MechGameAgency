using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MapRoom : MonoBehaviour
{
    
    Sprite lineImage = null;
    [SerializeField]
    GameObject Sprite2D;
    [SerializeField]
    Sprite EncounterSprite;
    [SerializeField]
    Sprite ShopSprite;
    [SerializeField]
    Sprite EnemySprite;
    [SerializeField]
    Sprite BossSprite;
    public GameObject Selected;
    public Encounter room;
    Animator animator;
    [SerializeField]
    Image line2D;
    public bool available = false;

    // Start is called one before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetRoom(Encounter new_data)
    {
        room = new_data;
        this.transform.localPosition = new Vector3(room.position.X, room.position.Y, 0);
        switch(room.type)
        {
            case Encounter.Type.ENCOUNTER:
                this.GetComponentInChildren<Image>().sprite = EncounterSprite;
                Sprite2D.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            break;

            case Encounter.Type.ENEMY:
                this.GetComponent<Image>().sprite = EnemySprite;
                Sprite2D.transform.localScale = Vector3.one;
            break;

            case Encounter.Type.SHOP:
                this.GetComponent<Image>().sprite = ShopSprite;
                Sprite2D.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            break;

            case Encounter.Type.BOSS:
                this.GetComponent<Image>().sprite = BossSprite;
                Sprite2D.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            break;
        }
        /*for(int m = 0; m < Map_Data[i][j].next_rooms.Count; m++)
        {
            MakeLine(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y);
        }*/
    }

    void MakeLine(float ax, float ay, float bx, float by, Color col) {
        GameObject NewObj = new GameObject();
        NewObj.name = "line from "+ax+" to "+bx;
        Image NewImage = NewObj.AddComponent<Image>();
        NewImage.sprite = lineImage;
        NewImage.color = col;
        RectTransform rect = NewObj.GetComponent<RectTransform>();
        rect.SetParent(transform);
        rect.localScale = Vector3.one;

        Vector3 a = new Vector3(ax/**graphScale.x*/, ay/**graphScale.y*/, 0);
        Vector3 b = new Vector3(bx/**graphScale.x*/, by/**graphScale.y*/, 0);


        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, 0.1f);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
   }
   public void SetAvailable(bool new_value)
    {
        available = new_value;
        if(available)
        {
            //animation trigger here
            animator.SetBool(0,true);
        }
        else if(room.selected)
        {
            //play animation trigger here for the room you're in.
            animator.SetBool(0,false);
        }
    }
   void OnInput()
    {
        if(!available)
        {
            return;
        }
        room.selected = true;
        //play select animation here
        
    }
    public void ShowSelected()
    {
        line2D.color = Color.green;
    }

}
