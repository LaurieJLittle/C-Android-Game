using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateDestroy : MonoBehaviour {
    
    void FixedUpdate()
    {
        transform.position += new Vector3(0f, 0f, -Player.velocity * Time.fixedDeltaTime);

        if (transform.position.z <= -40f)
            Destroy(gameObject);
    }
    
}
