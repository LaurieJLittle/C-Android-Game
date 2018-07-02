using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player;
    public List<Transform> wheels;

    public const float velocity = 16f;

    public int nuggetcount { get; private set; }
    public int nuggetcombo { get; set; }
    public int highCombo { get; private set; }
    public float score { get; private set; }
    public float runTime { get; private set; }


    float clickStartY = 0f; // for pc testing only

    float t = 0f;
    float t1 = 0f;

    float steeringStrength;
    bool jumping = false;
    Vector3 startOfJumpPos;

    void Awake()
    {
        if (player == null)
            player = this;
        else
            Destroy(gameObject);

        foreach (Transform child in transform)
        {
            if (child.CompareTag("wheel"))
                wheels.Add(child);
        }
    }
	
	void FixedUpdate () {

        if (Time.timeScale > 0.1f)
        {
            runTime += Time.fixedDeltaTime / Time.timeScale;
            score += Mathf.Pow(Time.timeScale, 2f) * Time.fixedDeltaTime * 5f;
            increaseGameSpeed();
        }

        foreach (Transform wheel in wheels)
            wheel.Rotate(new Vector3(transform.localRotation.eulerAngles.x - 90f, 0f, 0f), (400f * -velocity / (2 * Mathf.PI)) * Time.fixedDeltaTime);
        

        if (jumping)
        {
            if (t < 0.001f)
                startOfJumpPos = transform.position;       

            float jumpHeight = 1f - (0.6f * Mathf.Sin(4f * t));
            transform.position = MoveAlongRadius(startOfJumpPos, 4.92f, jumpHeight);

            t += Time.fixedDeltaTime;

            if (transform.position.magnitude > 4.8f && t > 0.2f)
            {
                jumping = false;
                t = 0f;
            }
        }
        else
        {
            
#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
                clickStartY = Input.mousePosition.y / Screen.height;
            
            // Jump input
            if (!jumping && Input.GetMouseButtonUp(0) && clickStartY > 0.2f)
                jumping = true;

            // Steering input
            if (Input.GetMouseButton(0) && clickStartY < 0.2f)
                steeringStrength = 2f * (Input.mousePosition.x / Screen.width - 0.5f) * Time.fixedDeltaTime * 50f;
            else
                steeringStrength = 0f;

#else
            // Jump input
            if (!jumping && Input.touchCount >= 1 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (Input.touches[0].position.x / Screen.width < 0.85f || Input.touches[0].position.y / Screen.height > 0.15)
                {
                    jumping = true;
                }
            }
                
            // Steering input
            if (Mathf.Abs(Input.acceleration.x) > 0.01f)
            {
                steeringStrength = -1.7f * Input.acceleration.x * Time.fixedDeltaTime * 50f;

                if (steeringStrength > 1.2f)
                    steeringStrength = 1.2f;
                if (steeringStrength < -1.2f)
                    steeringStrength = -1.2f;
            }
            else
            {
                steeringStrength = 0f;
            }
#endif

            Vector3 steeringDir = Vector3.Cross(transform.position, new Vector3(0, 0, 1));
            transform.Translate(0.1f * steeringStrength * steeringDir, Space.World);

            float rotationAngle = Mathf.Rad2Deg * Mathf.Atan2(transform.position.y, transform.position.x);
            transform.rotation = Quaternion.Euler(new Vector3(steeringStrength * transform.position.x * 11f, steeringStrength * transform.position.y * 11f, rotationAngle + 90f));
        }
    }


    /// <summary>
    /// Moves a an object along the radial line from (0,0) to its initial (x,y) position .
    /// </summary>
    Vector3 MoveAlongRadius(Vector3 initialPos, float initialMagnitude, float scaleFactor)
    {
        float newMagnitude = initialMagnitude * scaleFactor;
        float ANG = Mathf.Atan2(initialPos.y, initialPos.x);

        float newY = Mathf.Sin(ANG) * newMagnitude;
        float newX = Mathf.Cos(ANG) * newMagnitude;
        
        return new Vector3(newX, newY, 0f);
    }

    void increaseGameSpeed ()
    {
        t1 += Time.fixedDeltaTime / Time.timeScale;
        
        if (t1 > 30f && Time.timeScale < 0.99f && Time.timeScale > 0.5f)
        {
            Time.timeScale += 0.02f;
            GetComponent<AudioSource>().pitch = Time.timeScale + 0.1f;
            t1 = 0f;
        }
    }

    public void nuggetCollected()
    {
        nuggetcount += 1;
        nuggetcombo += 1;

        if (nuggetcombo > highCombo)
            highCombo = nuggetcombo;

        score += nuggetcombo * 12f * Mathf.Pow(Time.timeScale, 2f);
    }          

    public void Reset()
    {
        t1 = 0f;

        nuggetcombo = 0;
        highCombo = 0;
        score = 0f;
        runTime = 0f;
        jumping = false;
    }
}