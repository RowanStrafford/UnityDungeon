using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    bool m_activated = false;

    private Transform m_playerTransform;
    public float m_moveSpeed = 2.0f;

    private float m_freezeTimer = 10.0f;

    private int m_state = 0;
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
            if (distance < 5.0f)
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
            GameObject closestEnemy = FindNearestEnemy();
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.transform.position, Time.deltaTime * m_moveSpeed * 2);

            if (Vector2.Distance(transform.position, closestEnemy.transform.position) < 0.5f)
            {
                m_freezeTimer = 10.0f;
                AttackEnemy(closestEnemy);
                m_state = 1;
            }
        }

      

       
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDist = 10000.0f;
        int closestIndex = 0;

        for(int i = 0; i <  enemies.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, enemies[i].transform.position);

            if(distance < closestDist)
            {
                closestDist = distance;
                closestIndex = i;
            }
        }

        return enemies[closestIndex];
    }

    void AttackEnemy(GameObject enemy)
    {
        enemy.GetComponent<Creature>().Freeze();
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerTransform.position, Time.deltaTime * m_moveSpeed);
    }
}
