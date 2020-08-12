using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Plane m_plane;

    // Start is called before the first frame update
    void Start()
    {
        m_plane = new Plane(Vector3.back, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (m_plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }
}
