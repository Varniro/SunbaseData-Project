using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClientData : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Various varaibles to map to the recieved data from the API

    [SerializeField]
    int id;

    [SerializeField]
    string label;

    [SerializeField]
    string cName = "N/A";

    [SerializeField]
    int points = 0;

    [SerializeField]
    string address = "N/A";

    [SerializeField]
    bool isManager;

    public bool isLabel = false; //If the prefab is data or label
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Assigning data from API Dictionary
    public void AssignData(int clientID, List<string> dataList){

        id = clientID;
        if(dataList.ElementAtOrDefault(0) != null){
            label = dataList[0];
        }
        
        if(dataList[1] == "true"){
            isManager = true;
        }else{
            isManager = false;
        }

        if(dataList.ElementAtOrDefault(2) != null){
            cName = dataList[2];
        }else{
            cName = "N/A";
        }

        if(dataList.ElementAtOrDefault(3) != null){
            points = int.Parse(dataList[3]);
        }

        if(dataList.ElementAtOrDefault(4) != null){
            address = dataList[4];
        }else{
            address = "N/A";
        }

        transform.GetChild(0).GetComponent<Text>().text = label;
        transform.GetChild(1).GetComponent<Text>().text = points.ToString();
        
    }

    //Handling Pointer Events for GUI interaction
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(!isLabel)
       UIManager.instance.showWindow(label,cName,address,points);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(!isLabel)
        transform.GetComponent<Image>().color = new Color(1,1,1,1);
    }

     public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!isLabel)
        transform.GetComponent<Image>().color = new Color(0,0,0,0);
    }
}
