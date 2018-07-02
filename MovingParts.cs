using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParts : MonoBehaviour {

    public static MovingParts movingparts;

    public GameObject tunnel;
    public GameObject nugget;
    public List<GameObject> singleObstacles;
    public List<GameObject> rocks;
    public List<GameObject> ActiveObstacleInstances;

    float tunnelLength = 20f;
    float spawnRateTunnel;
    float spawnRateObstacle;

    float offset = 40f;

    public bool gamePlaying = false;
    float t = 0f;
    float tObst = 0f;

    private void Awake()
    {
        if (movingparts != null)
            Destroy(gameObject);
        else
            movingparts = this;
    }


    void Start()
    {
        spawnRateTunnel = tunnelLength / Player.velocity;
        spawnRateObstacle = tunnelLength * 2f / Player.velocity;
    }


    void FixedUpdate()
    {
        if (gamePlaying)
        {
            t += Time.fixedDeltaTime;
            tObst += Time.fixedDeltaTime;

            if (t >= spawnRateTunnel - 0.011f)
                spawnTunnel();

            if (tObst >= spawnRateObstacle * 1f)
                spawnRandomObstacle();
        }
    }

    void spawnTunnel()
    {
        t = 0f;
        Instantiate(tunnel, new Vector3(0f, 0f, 80f), transform.rotation);
    }

    void spawnRandomObstacle ()
    {
        float rockOrNot = Random.value;
        tObst = 0f;

        if (rockOrNot > 0.3f) 
        {
            spawnNugget(70f);
            spawnRateObstacle = tunnelLength * 2f / Player.velocity;

            int i = (int)(Random.value * singleObstacles.Count);
            GameObject randomObj = singleObstacles[i];
            ActiveObstacleInstances.Add(Instantiate(randomObj, new Vector3(0f, 0f, 80f + offset), transform.rotation));
        }
        else
        {
            spawnNugget(95f);
            spawnRateObstacle = 3f * tunnelLength / Player.velocity;

            for (int m = 0; m <= 2; m++)
            {
                GameObject[] randomRocks = new GameObject[3];

                for (int n = 0; n <= 2; n++)
                {
                    int i = (int)(Random.value * rocks.Count);
                    randomRocks[n] = rocks[i];
                }

                List<float> rockRotations = rockRandomizer(3);

                for (int p = 0; p <= 2; p++)
                {
                    ActiveObstacleInstances.Add(Instantiate(randomRocks[p], new Vector3(0f, 0f, offset + 80f + 20f * (m - 1)), Quaternion.Euler(0f, 180f, rockRotations[p])));
                }
            }
        }
    }


    List<float> rockRandomizer(int numRocks)
    {

        List<float> rotations = new List<float>();

        while (rotations.Count < numRocks)
        {
            float newRotation = Random.value * 360f;
            bool addRotation = true;

            foreach (float rotation in rotations)
            {
                if (Mathf.Abs(newRotation - rotation) < 40 || Mathf.Abs((newRotation - 360f) - rotation) < 40)
                    addRotation = false;
            }

            if (addRotation)
                rotations.Add(newRotation);
        }
        return rotations;
    }


    void spawnNugget(float spawnZ)
    {
        ActiveObstacleInstances.Add(Instantiate(nugget, new Vector3(0f, 0f, offset + spawnZ), Quaternion.Euler(0f, 0f, Random.value * 360f) ));
    }
}