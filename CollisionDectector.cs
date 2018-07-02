using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDectector : MonoBehaviour {

	void OnTriggerStay2D (Collider2D other) 
	{
		if (other.CompareTag ("Player") && Mathf.Abs(transform.position.z) < 1.2f)
			Crash ();
	}


	void Crash ()
    {
        if (GetComponent<AudioSource>() != null)
    		GetComponent<AudioSource>().Play ();

        Menu.menu.crashReplay();
    }
}
