using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardEventInput : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool focused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
    }
    public void OnPointerClick (PointerEventData eventData)
    {
        if(this.GetComponent<CardPlayScript>().isCardPlayable == true)
        {
            OnPointerExit(eventData);
        }
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            this.GetComponent<CardPlayScript>().UseCard();
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            this.GetComponent<CardPlayScript>().DiscardCard();
        }

    }
    Canvas TempCanvas;
    int previous_hierarchy_place;
    [SerializeField]
    GameObject PlaceHolderPrefab;
    GameObject PlaceHolder;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        //it's unbelievable to me that I haven't found a better solution for this, but everywhere I looked it was stated that sorting group is not supported in UI elements
        //at least it's only supposed to happen on one card at a time, so there is no scaling of lag due to additional complexity
        if(this.GetComponent<CardPlayScript>().isAlreadyPicked == true)
        {
            return;
        }
        this.transform.localScale = new Vector3(1.75f,1.75f, 1);
        if(this.GetComponent<CardPlayScript>().isCardPlayable == true)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100, this.transform.localPosition.z);
            
        }
            previous_hierarchy_place = this.transform.GetSiblingIndex();
            this.GetComponent<LayoutElement>().ignoreLayout = true;
            PlaceHolder = Instantiate(PlaceHolderPrefab, this.transform.parent);
            PlaceHolder.transform.SetSiblingIndex(previous_hierarchy_place);
            StartCoroutine(SetAsLastSiblingNextFrame(this.transform));
        if(this.GetComponent<CardPlayScript>().isCardPlayable == false && this.GetComponent<CardPlayScript>().isCardPickable == false)
        {
            this.GetComponent<Image>().raycastPadding = new Vector4(50, 50, 50, 50);
        }
    }
    private IEnumerator SetAsLastSiblingNextFrame(Transform childTransform)
    {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();

        // Set as last sibling
        childTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if(this.GetComponent<CardPlayScript>().isCardPlayable == true)
        {
            this.transform.localScale = new Vector3(1,1, 1);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 100, this.transform.localPosition.z);
        }
        else if(this.GetComponent<CardPlayScript>().isCardPickable == true && this.GetComponent<CardPlayScript>().isAlreadyPicked == false)
        {
            this.transform.localScale = new Vector3(0.75f,0.75f, 1);
        }
        else if(this.GetComponent<CardPlayScript>().isCardPickable == false && this.GetComponent<CardPlayScript>().isCardPlayable == false)
        {
            this.transform.localScale = new Vector3(0.5f,0.5f, 1);
            this.GetComponent<Image>().raycastPadding = new Vector4(0, 0, 0, 0);
        }
        if(PlaceHolder != null)
        {
            Destroy(PlaceHolder);
        }
        this.transform.SetSiblingIndex(previous_hierarchy_place);
        this.GetComponent<LayoutElement>().ignoreLayout = false;

        
    }
}
