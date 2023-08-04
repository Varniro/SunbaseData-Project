using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public static UIManager instance; //Singleton for ease of access

    public Transform popUpPanel; //Reference to UI panel

    //Animation control times
    public float fadeTime;
    public float animTime;

    private void Awake() {
        if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //Functions controlling Animations of the GUI using DOTween, also assigning data to the required GUI Texts
    public void showWindow(string label, string name, string address, int points){
        popUpPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = label;
        popUpPanel.GetChild(0).GetChild(1).GetComponent<Text>().text = "Name - "+name;
        popUpPanel.GetChild(0).GetChild(2).GetComponent<Text>().text = "Points - "+points.ToString();
        popUpPanel.GetChild(0).GetChild(3).GetComponent<Text>().text = "Address - "+address;
        popUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        popUpPanel.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,0f), animTime, false).SetEase(Ease.OutElastic);
        popUpPanel.GetComponent<CanvasGroup>().DOFade(1,fadeTime);
    }

    public void close(){
        popUpPanel.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-100f), animTime, false).SetEase(Ease.OutQuint);
        popUpPanel.GetComponent<CanvasGroup>().DOFade(0,fadeTime);
        popUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
