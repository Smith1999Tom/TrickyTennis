using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BallController : MonoBehaviour
{

    UnityEngine.Vector3 m_startingPoint;
    UnityEngine.Vector3 m_targetPoint;

    float maxHeight = 3f;
    float minHeight = 1f;

    float m_farPlane = 25f;
    float m_nearPlane = 15f;
    public GameObject m_reticule;

    public GameObject m_target;

    public float m_moveSpeed = 2f;
    float m_time = 0f;

    float m_timeToKill = 5f;
    float m_timeSinceHit = 0f;

    public GameObject m_Enemy;
    public bool m_missed = false;
    

    // Start is called before the first frame update
    void Start()
    {

        m_startingPoint = transform.position;
        SetTargetPoint(m_nearPlane);

        m_Enemy = GameObject.FindGameObjectWithTag("Enemy");
     

    }

    // Update is called once per frame
    void Update()
    {


        /*
        float distance = GetDistanceToTarget();
        // distance += .2f;
        

        if (distance < 0)
        {
            Destroy(m_target);
        }


        UnityEngine.Vector3 movement = new UnityEngine.Vector3(0, 0, m_moveSpeed * Time.deltaTime);
        transform.position -= movement;


        float y = GetYValue(distance);

        transform.position = new UnityEngine.Vector3(transform.position.x, y, transform.position.z);

        if(m_target != null)
        {
            m_target.transform.position = new UnityEngine.Vector3(transform.position.x, GetYValue(0), m_goalPlane);
        }
        */
        
    }


    void FixedUpdate()
    {
        m_time += Time.deltaTime;
        m_timeSinceHit += Time.deltaTime;
        m_missed = CheckIfMissed();

        CheckTimeSinceHit();
        UpdateCrosshair(1-(m_time / m_moveSpeed));

        MoveBall();
        MoveTarget();
    }



    void SetTargetPoint(float zValue)
    {
        m_targetPoint = new UnityEngine.Vector3(GetXValue(1), GetYValue(1), zValue);
    }

    void SetStartingPoint(float zValue)
    {
        m_startingPoint = new UnityEngine.Vector3(transform.position.x, transform.position.y, zValue);
    }

    void MoveTarget()
    {
        m_target.transform.position = m_targetPoint;
    }

    void UpdateCrosshair(float t)
    {
        float scaleOffset = 0.15f;
        m_reticule.GetComponent<RectTransform>().localScale = new UnityEngine.Vector3(t + scaleOffset, t + scaleOffset, 1);
    }

    void MoveBall()
    { 

        UnityEngine.Vector3 movement = new UnityEngine.Vector3(GetXValue(m_time / m_moveSpeed), GetYValue(m_time/m_moveSpeed), GetZValue(m_time/m_moveSpeed));

        transform.position = movement;
    }

    bool CheckIfMissed()
    {
        bool missed = false;
        float missOffset = 0.6f;

        if (transform.position.z < (m_nearPlane - missOffset))
            missed = true;
        if (transform.position.z > (m_farPlane + missOffset))
            missed = true;

        return missed;
    }

    void CheckTimeSinceHit()
    {
        if (m_timeSinceHit > m_timeToKill)
        {
            Destroy(gameObject);
        }
    }


    void OnHit()
    {
        //Flip the x rotation
        UnityEngine.Quaternion newDirection = transform.rotation;
        newDirection.x *= -1;
        transform.rotation = newDirection;

        //Set new target and starting
        if (m_targetPoint.z == m_nearPlane)
        {
            SetTargetPoint(m_farPlane);
            SetStartingPoint(m_nearPlane);
        }
        else
        {
            SetTargetPoint(m_nearPlane);
            SetStartingPoint(m_farPlane);
        }

        //Reset time
        m_time = 0f;
        m_timeSinceHit = 0f;

        if (m_targetPoint.z == m_farPlane)
        {
            m_Enemy.GetComponent<EnemyController>().QueueBall(this); // Add self to the list of balls to be hit by enemy
            print("queueing ball");
        }
        else
        {
            m_Enemy.GetComponent<EnemyController>().RemoveBall();
        }
    }


    float GetXValue(float t)
    {
        return m_startingPoint.x;
    }

    float GetYValue(float t)
    {
        
        float yScale = -4f * (float)Math.Pow(t - 0.5f, 2f) + 1f;
        float y = (maxHeight - minHeight) * yScale + minHeight;
        return y;
    }

    float GetZValue(float t)
    {
        float direction = transform.rotation.x;
        if (direction < 0)
            direction = -1;
        else
            direction = 1;

        if (direction == -1)
            t = 1 - t;


        float z = (Mathf.Abs(m_startingPoint.z - m_targetPoint.z) * t + Math.Min(m_startingPoint.z, m_targetPoint.z));



        return z;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8) //Racket layer
        {
            OnHit();
        }
        
    }
    


}
