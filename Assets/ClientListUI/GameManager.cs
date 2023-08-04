using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string reponse; // API Response

    Dictionary<int, List<string>> data = new Dictionary<int, List<string>>(); //Dictionary to store the JSON response
    //The Key of each entry is the ID of the clients,
    //The Value is list of string which holds the values in the following manner
    // <Label, isManager, ClientName, Points, Address>


    public GameObject listItem; //GUI prefab for Client List

    public Transform ContentHolder; // Scroll View Content holder reference

    public Dropdown options; // Filtering dropdown reference

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchData()); //Fetching Data from the API
    }

    //Function to display the client data
    public void display(){

        foreach (Transform child in ContentHolder)
        {
            Destroy(child.gameObject);
        }

        GameObject instance = Instantiate(listItem, new Vector2(0,0), Quaternion.identity);
        instance.transform.SetParent(ContentHolder);
        instance.GetComponent<ClientData>().isLabel = true;
        instance.transform.localScale = new Vector3(1,1,1);

        //Filtering data based on dropdown
        if(options.value == 0){
            foreach (var kvp in data){
                instance = Instantiate(listItem, new Vector2(0,0), Quaternion.identity);

                instance.transform.SetParent(ContentHolder);
                instance.transform.localScale = new Vector3(1,1,1);
                instance.GetComponent<ClientData>().AssignData(kvp.Key,kvp.Value);
            }
        }else if(options.value == 1){
             foreach (var kvp in data){
                if(kvp.Value[1] == "true"){
                    instance = Instantiate(listItem, new Vector2(0,0), Quaternion.identity);

                    instance.transform.SetParent(ContentHolder);
                    instance.transform.localScale = new Vector3(1,1,1);
                    instance.GetComponent<ClientData>().AssignData(kvp.Key,kvp.Value);
                }
            }
        }else if(options.value == 2){
             foreach (var kvp in data){
                if(kvp.Value[1] == "false"){
                    instance = Instantiate(listItem, new Vector2(0,0), Quaternion.identity);

                    instance.transform.SetParent(ContentHolder);
                    instance.transform.localScale = new Vector3(1,1,1);
                    instance.GetComponent<ClientData>().AssignData(kvp.Key,kvp.Value);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Fetching Data using UnityWebRequest
    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Failure: " + request.error);
            }
            else
            {
                //Initial Data Cleaning
                reponse = request.downloadHandler.text;
                reponse = reponse.Replace("\n", "").Replace("\r", "");
                parser(reponse);
            }
        }
    }


    //Custom Made parser to read the JSON response and map all the data to Dictionary
    void parser(string resp){
        string clients;

        int clientDataStart = resp.IndexOf("[")+1;
        int clientDataEnd = resp.IndexOf("]");

        clients = resp.Substring(clientDataStart, clientDataEnd-clientDataStart);

        int count = 0;
        int clientNum = 0;
        
        while(count <clients.Length-1){
            if(clients[count] == '}'){
                if(clients[count+1] == ',')
                clientNum++;
            }
            
            count++;
        }

        int dEndIndex;

        for(int i = 0; i<clientNum+1; i++){
            dEndIndex = clients.IndexOf('}');

            string cData = clients.Substring(0,dEndIndex+1);
            if(cData[0] == ','){
                cData = cData.Substring(1);
            }
            clients = clients.Substring(dEndIndex+1);

            List<string> cDataList = new List<string>();
            string cID = "0";

            if(cData.Contains("label")){
                int index = cData.IndexOf("label")+8;
                string dataString = cData.Substring(index);
                if(dataString.IndexOf('"') == -1){
                    cDataList.Add(cData.Substring(index,dataString.IndexOf('}')));
                }else{
                     cDataList.Add(cData.Substring(index,dataString.IndexOf('"')));
                }
            }else if(cData.Contains("name")){
                int index = cData.IndexOf("name")+7;
                string dataString = cData.Substring(index);
                if(dataString.IndexOf('"') == -1){
                    cDataList.Add(cData.Substring(index,dataString.IndexOf('}')));
                }else{
                     cDataList.Add(cData.Substring(index,dataString.IndexOf('"')));
                }
            }

            if(cData.Contains("id")){
                int index = cData.IndexOf("id")+4;
                string dataString = cData.Substring(index);
                if(dataString.IndexOf(',') == -1){
                    cID = cData.Substring(index,dataString.IndexOf('}'));
                }else{
                    cID = cData.Substring(index, dataString.IndexOf(','));
                }
                
            }

            if(cData.Contains("isManager")){
                int index = cData.IndexOf("isManager")+11;
                string dataString = cData.Substring(index);
                if(dataString.IndexOf(',') == -1){
                    cDataList.Add(cData.Substring(index,dataString.IndexOf('}')));
                }else{
                     cDataList.Add(cData.Substring(index,dataString.IndexOf(',')));
                }
            }

            data.Add(int.Parse(cID), cDataList);
        }

        string eData;

        int eDataStart = resp.IndexOf("data")+7;
        int eDataEnd = resp.IndexOf("}},")+1;

        eData = resp.Substring(eDataStart, eDataEnd-eDataStart);

        count = 0;
        int dataNum = 0;
        while(count <eData.Length-1){
            if(eData[count] == '"' && System.Char.IsNumber(eData[count+1])){
                dataNum++;
            }
            
            count++;
        }

        dEndIndex = 0;

        for(int i = 0; i<dataNum; i++){
            dEndIndex = eData.IndexOf('}');
            string cData = eData.Substring(0,dEndIndex+1);

            if(cData[0] == ','){
                cData = cData.Substring(1);
            }

            eData = eData.Substring(dEndIndex+1);

            List<string> cDataList = new List<string>();
            string cID = "";

            int tempCounter = 0;
            while(cData[tempCounter+1] != '"'){
                cID+=cData[tempCounter+1];
                tempCounter++;
            }

            cDataList = data[int.Parse(cID)];

            if(cData.Contains("name")){
                int index = cData.IndexOf("name")+7;
                string dataString = cData.Substring(index);
                cDataList.Add(cData.Substring(index,dataString.IndexOf('"')));
            }

             if(cData.Contains("points")){
                int index = cData.IndexOf("points")+8;
                string dataString = cData.Substring(index);
                cDataList.Add(cData.Substring(index,dataString.IndexOf('}')));
            }

            if(cData.Contains("address")){
                int index = cData.IndexOf("address")+10;
                string dataString = cData.Substring(index);
                cDataList.Add(cData.Substring(index,dataString.IndexOf('"')));
            }

            data[int.Parse(cID)] = cDataList;

        }

        //Displaying the data after parsing is completed
        display();

    }


}
