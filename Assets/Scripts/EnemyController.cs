using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Queue<BallController> ballsToHit = new Queue<BallController>();
    BallController currentBall;

    public float moveSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {

        print("queued " + ballsToHit.Count.ToString());
        if (ballsToHit.Count > 0)
        {
            print("count");

            currentBall = ballsToHit.Peek();

            if (currentBall.m_missed)
            {
                ballsToHit.Dequeue();
                currentBall = ballsToHit.Peek();
            }


            MoveTowardsBall();
        }

    }

    public void QueueBall(BallController ball)
    {
        ballsToHit.Enqueue(ball);
        print("ball queued " + ballsToHit.Count.ToString());
    }

    public void RemoveBall()
    {
        ballsToHit.Dequeue();
    }

    void MoveTowardsBall()
    {
        Vector3 movement = currentBall.m_target.transform.position - transform.position;
        movement.z = 0;
        print("movement=" + (movement.x).ToString());
        transform.position += movement * moveSpeed * Time.deltaTime;
    }


}
