using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function to reload the scene
    public void reset(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
