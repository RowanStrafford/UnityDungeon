using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public GameObject[] m_objects;

    private Transform m_leftDoor;
    private Transform m_rightDoor;

    private Vector2 m_leftDoorTarget;
    private Vector2 m_rightDoorTarget;

    public float m_openSpeed = 1.0f;

    private bool m_activate = false;

    public bool vertical = false;

    void Start()
    {
        // Original positions of the doors
        m_leftDoor = transform.GetChild(0).transform;
        m_rightDoor = transform.GetChild(1).transform;

        if (vertical)
        {
            // Door moves vertically
            m_leftDoorTarget = new Vector2(m_leftDoor.position.x, m_leftDoor.position.y - 1.0f);
            m_rightDoorTarget = new Vector2(m_rightDoor.position.x, m_rightDoor.position.y + 1.0f);
        }
        else
        {
            // Door moves horizontally
            m_leftDoorTarget = new Vector2(m_leftDoor.position.x - 1.0f, m_leftDoor.position.y);
            m_rightDoorTarget = new Vector2(m_rightDoor.position.x + 1.0f, m_rightDoor.position.y);
        }
    }

    void Update()
    {
        if(m_activate)
        {
            Open();
        }
    }

    bool CheckAllObjectsDead()
    {
        if(m_objects.Length <= 0)
        {
            return true;
        }

        for(int i = 0; i < m_objects.Length; i++)
        {
            if(m_objects[i] == null)
            {               
                continue;
            } else
            {
                return false;
            }
        }

        return true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if(CheckAllObjectsDead())
            {
                m_activate = true;
            }
        }
    }

    void Open()
    {
        m_leftDoor.position = Vector2.MoveTowards(m_leftDoor.position, m_leftDoorTarget, Time.deltaTime * m_openSpeed);
        m_rightDoor.position = Vector2.MoveTowards(m_rightDoor.position, m_rightDoorTarget, Time.deltaTime * m_openSpeed);

    }
}
