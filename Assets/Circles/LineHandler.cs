using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    [SerializeField]
    GameObject line; //LineRenderer Prefab

    GameObject lineInstance;
    Vector2 clickPos; //Initial Click Position
    
    List<Vector2> pts = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check for device type
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //Starting to draw the line as soon as input recieved
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pts.Clear();
                pts.Add(clickPos);
                lineInstance = Instantiate(line,new Vector2(10000,10000), Quaternion.identity);
                lineInstance.GetComponent<LineRenderer>().positionCount = 2;
                lineInstance.GetComponent<LineRenderer>().SetPosition(0, clickPos);
            }
            //Continously Moving the line's end point with the cursor
            if(Input.GetKey(KeyCode.Mouse0)){
                lineInstance.GetComponent<LineRenderer>().SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            //Defining the Edge Collider when user stops drawing
            if(Input.GetKeyUp(KeyCode.Mouse0)){
                pts.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                lineInstance.transform.position = Vector3.zero;
                lineInstance.GetComponent<EdgeCollider2D>().points = pts.ToArray();

                lineInstance.GetComponent<line>().end = true;
            }
        }else if(SystemInfo.deviceType == DeviceType.Handheld){
            //Same system modified for touch
             if(Input.touchCount == 1){
                clickPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).rawPosition);
                pts.Clear();
                pts.Add(clickPos);
                lineInstance = Instantiate(line,Vector3.zero, Quaternion.identity);
                lineInstance.GetComponent<LineRenderer>().positionCount = 2;
                lineInstance.GetComponent<LineRenderer>().SetPosition(0, clickPos);
            }
            if(Input.touchCount == 1){
                lineInstance.GetComponent<LineRenderer>().SetPosition(1, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            }

            if(Input.touchCount == 0){
                pts.Add(lineInstance.GetComponent<LineRenderer>().GetPosition(1));
                lineInstance.GetComponent<EdgeCollider2D>().points = pts.ToArray();

                lineInstance.GetComponent<line>().end = true;
            }
        }
    }
}
