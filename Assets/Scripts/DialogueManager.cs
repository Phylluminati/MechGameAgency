using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    GameObject DialogueImage;
    [SerializeField]
    private bool fadeIn = false;
    [SerializeField]
    private bool fadeOut = false;
    private CanvasGroup ImageGroup;

[YarnCommand("FadeIn")]
    public void FadeIn()
    {
        fadeIn = true;
    }
[YarnCommand("FadeOut")]
    public void FadeOut()
    {
        fadeOut = true;
    }
[YarnCommand("ChangeBackgroundImage")]
    public void ChangeBackgroundImage(Sprite sprite)
    {
        DialogueImage.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
    }
    [YarnCommand("ClearBackgroundImage")]
    public void ClearBackgroundImage()
    {
        DialogueImage.GetComponent<UnityEngine.UI.Image>().sprite = null;
    }
    
    void Awake()
    {
        ImageGroup = DialogueImage.GetComponent<CanvasGroup>();
        
    }
    void Start()
    {
        
    }
    /*public void FadeIn()
    {
        Color b = DialogueImage.GetComponent<UnityEngine.UI.Image>().color;
        float _progress = 1;
    
        while(_progress > 0)
        {
            //DialogueImage.GetComponent<UnityEngine.UI.Image>().color = 0f;
            b.a = Mathf.MoveTowards(_progress, 0f, 0.05f * Time.deltaTime);
            DialogueImage.GetComponent<UnityEngine.UI.Image>().color = b;
            _progress = b.a;
        }
        print("End of while loop exiting method");
        
        Color tmp = DialogueImage.GetComponent<UnityEngine.UI.Image> ().color;
		tmp.a = 0f;
		DialogueImage.GetComponent<UnityEngine.UI.Image> ().color = tmp;
		float _progress = 0.0f;

		while (_progress < 1) 
		{
			Color _tmpColor = DialogueImage.GetComponent<UnityEngine.UI.Image> ().color;
			DialogueImage.GetComponent<UnityEngine.UI.Image> ().color = new Color (_tmpColor.r, _tmpColor.g, _tmpColor.b, Mathf.Lerp (tmp.a, 255, _progress)); //startAlpha = 0 <-- value is in tmp.a
			_progress += Time.deltaTime * 0.5f;
		}
    }
    
    void fadeOut()
    {
        
    }
    */
    void Update()
    {
        if(fadeIn)
        {
            if(ImageGroup.alpha < 1)
            {
                ImageGroup.alpha += Time.deltaTime;
                if(ImageGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if(fadeOut)
        {
            if(ImageGroup.alpha > 0)
            {
                ImageGroup.alpha -= Time.deltaTime;
                if(ImageGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
