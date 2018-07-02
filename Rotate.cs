using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float rateX = 0f;
    public float rateY = 0f;
    public float rateZ = 0f;
        
    void Update ()
    {
        transform.Rotate(new Vector3(rateX * Time.deltaTime, rateY * Time.deltaTime, rateZ * Time.deltaTime));
	}
}
