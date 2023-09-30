using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;



    // Update is called once per frame
    void Update()
    {   //chekd 
        transform.Rotate(0f, rotationSpeed *Time.deltaTime, 0f);        
    }
}