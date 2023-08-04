using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle : MonoBehaviour
{
    //Destroy the circle when the line crosses it.
    void OnTriggerExit2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
