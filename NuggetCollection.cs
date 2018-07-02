using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuggetCollection : MonoBehaviour {
    
    bool collectedOrMissed = false;
    
    void Update ()
    {
        if (transform.position.z < -1.2f && !collectedOrMissed) // nugget is  missed
        {
            collectedOrMissed = true;
            Player.player.nuggetcombo = 0;
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Mathf.Abs(transform.position.z) < 1.2f && !collectedOrMissed) // nugget is collected
        {
            collectedOrMissed = true;
            Player.player.nuggetCollected();
            GetComponent<AudioSource>().Play();

            foreach (Transform child in transform)
                child.gameObject.SetActive(false);

            Destroy(gameObject, 1f);
        }
    }
}
