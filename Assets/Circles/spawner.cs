using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    //Coordinates defining the spawn area
    float top,left,bottom,right;

    [SerializeField]
    [Range(1,5)]
    float radRangeMin; // Minimum radius of the circles
    [SerializeField]
    [Range(1,5)]
    float radRangeMax; // Maximum radius of the circles

    [SerializeField]
    List<Color> colors = new List<Color>(); //Color of the circles

    [SerializeField]
    GameObject circle; //Circle Prefab
    // Start is called before the first frame update
    void Start()
    {
        //Getting the coordinates from child empty object within the scene
        top = transform.GetChild(0).position.y;
        left = transform.GetChild(0).position.x;
        bottom = transform.GetChild(1).position.y;
        right = transform.GetChild(1).position.x;

        int SpawnNumber = Random.Range(5,10); //Number of circles spawned

        //Spawning the circles and adding varius attributes to make them unique
        for(int i = 0; i<SpawnNumber; i++){
            float x = Random.Range(left,right);
            float y = Random.Range(top,bottom);

            GameObject instance = Instantiate(circle, new Vector2(x,y), Quaternion.identity);

            float radius = Random.Range(radRangeMin,radRangeMax);

            instance.transform.localScale = new Vector2(radius,radius);

            int colorNum = Random.Range(0,colors.Count-1);

            instance.GetComponent<SpriteRenderer>().color = colors[colorNum];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
