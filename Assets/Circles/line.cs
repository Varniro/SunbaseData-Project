using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    public bool end; //When user stops drawing the line, triggering the shrinking
    List<Vector2> pts = new List<Vector2>(); //End Points of the edge collider so the collider shirnks with the line
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(end){
            //Lerping the line to shrink as it crosses the circle
            GetComponent<LineRenderer>().SetPosition(0, Vector3.Lerp(GetComponent<LineRenderer>().GetPosition(0), GetComponent<LineRenderer>().GetPosition(1), Time.deltaTime));
            Vector2 endPoint = Vector3.Lerp(GetComponent<LineRenderer>().GetPosition(0), GetComponent<LineRenderer>().GetPosition(1), Time.fixedDeltaTime);
            pts.Clear();
            pts.Insert(0, GetComponent<LineRenderer>().GetPosition(0));
            pts.Insert(1, endPoint);
            //Shrinking the coliider with the line
            GetComponent<EdgeCollider2D>().points = pts.ToArray();

            //Destroying the line once it's small enough
            if(Vector2.Distance(GetComponent<LineRenderer>().GetPosition(0), GetComponent<LineRenderer>().GetPosition(1))<0.3f){
                Destroy(gameObject);
            }

        }
    }
}
