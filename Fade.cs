using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	void Update ()
    {
        fade();
    }

    void fade()
    {
        if (transform.position.z <= 0f)
        {
            Color color = GetComponent<Renderer>().material.color;
            color.a -= Time.deltaTime * 4f;
            GetComponent<Renderer>().material.color = color;
        }
    }

}
