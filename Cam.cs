using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    Vector3 defaultPosition = new Vector3(0, 0, -10);
    Quaternion defaultRotation = new Quaternion(0, 0, 0, 1);
        
    public void ResetPosition()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }
}
