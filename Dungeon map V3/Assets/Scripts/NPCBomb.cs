using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBomb : MonoBehaviour
{
    bool m_activated = false;

    private Transform m_playerTransform;
    public float m_moveSpeed = 2.0f;

    private float m_freezeTimer = 10.0f;

    private int m_state = 0;

    public GameObject m_bomb;
    // 0 - Unactivated
    // 1 - Following
    // 2 - Charge at enemy
    // 3 - Flee

    void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, m_playerTransform.position);

        if (m_state == 0)
        {
            if (distance < 2.0f)
            {
                m_state = 1;
            }
        }

        if (m_state == 1)
        {
            if (m_freezeTimer <= 0)
            {
                m_state = 2;
            }

            if (distance > 1.5f)
            {
                FollowPlayer();
            }

            m_freezeTimer -= Time.deltaTime;
        }

        if (m_state == 2)
        {
            Vector2 closestEnemy = FindNearestBombSpot();
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy, Time.deltaTime * m_moveSpeed * 2);

            if (Vector2.Distance(transform.position, closestEnemy) < 0.5f)
            {
                m_freezeTimer = 10.0f;
                PlaceBomb();
                m_state = 1;
            }
        }
    }

    Vector2 FindNearestBombSpot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Vector2 posSum = Vector2.zero;

        for (int i = 0; i < enemies.Length; i++)
        {
            posSum += (Vector2)enemies[i].transform.position;
        }

        posSum /= enemies.Length;

        return posSum;
    }

    void PlaceBomb()
    {
        Instantiate(m_bomb, transform.position, Quaternion.identity);
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerTransform.position, Time.deltaTime * m_moveSpeed);
    }
}
