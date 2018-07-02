using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRandomizer : MonoBehaviour {


	void Start ()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
	}
}
