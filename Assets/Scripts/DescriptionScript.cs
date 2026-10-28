using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject DescriptionPanel;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //
        if(this.GetComponentInChildren<TMP_Text>().text.Length != 0)
        {
            DescriptionPanel.SetActive(true);
        }
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        if(this.GetComponentInChildren<TMP_Text>().text.Length != 0)
        {
            DescriptionPanel.SetActive(false);
        }
        //throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DescriptionPanel = this.transform.GetChild(1).gameObject;
        DescriptionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
