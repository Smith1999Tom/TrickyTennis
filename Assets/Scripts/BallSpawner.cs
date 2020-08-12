using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public float m_SpawnRate = 2f;
    float m_timeSinceLastSpawn = 0f;
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceLastSpawn += Time.deltaTime;
        if(m_timeSinceLastSpawn > m_SpawnRate)
        {
            SpawnBall();
            m_timeSinceLastSpawn = 0f;
        }
    }


    void SpawnBall()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        float boxWidth = box.size.x;
        float boxHeight = box.size.y;

        float[] xRange = { box.transform.position.x - (boxWidth / 2), box.transform.position.x + (boxWidth / 2) };
        float[] yRange = { box.transform.position.y - (boxHeight / 2), box.transform.position.y + (boxHeight / 2) };

      
        Vector3 randPos = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), transform.position.z);

        Instantiate(ball, randPos, ball.transform.rotation);


    }

}
