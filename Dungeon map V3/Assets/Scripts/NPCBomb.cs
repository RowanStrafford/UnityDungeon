using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject m_textObj;
    private Text m_text;

    private Player m_playerScript;

    void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_playerScript = m_playerTransform.GetComponent<Player>();

        m_text = m_textObj.transform.GetChild(0).GetComponent<Text>();
    }

    void DisableText()
    {
        m_textObj.SetActive(false);
    }


    void Update()
    {
        float distance = Vector2.Distance(transform.position, m_playerTransform.position);

        if (m_state == 0)
        {
            // If the player is within the distance of the NPC, activate it
            if ((distance < 2.0f) && (!m_playerScript.m_NPCChosen))
            {
                m_textObj.SetActive(true);
                m_text.text = "Johny Boris : A moderetaly friendly AI that throws bombs.";
                Invoke("DisableText", 3.0f);

                m_playerScript.m_NPCChosen = true;
                m_playerScript.AddCoins(-3);
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
            Transform closestEnemy = FindNearestEnemy();
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, Time.deltaTime * m_moveSpeed * 2);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 0.5f)
            {
                m_freezeTimer = 10.0f;
                PlaceBomb();
                m_state = 1;
            }
        }
    }

    //Vector2 FindNearestBombSpot()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    Vector2 posSum = Vector2.zero;

    //    for (int i = 0; i < enemies.Length; i++)
    //    {
    //        posSum += (Vector2)enemies[i].transform.position;
    //    }

    //    posSum /= enemies.Length;

    //    return posSum;
    //}

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDist = 10000.0f;
        int closestIndex = 10;

        for (int i = 0; i < enemies.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, enemies[i].transform.position);

            if (distance > 10.0f) continue;

            // If the distance is less than the least distance found, then replace it (this new one is the new closest)
            if (distance < closestDist)
            {
                closestDist = distance;
                closestIndex = i;
            }
        }

        if (closestDist == 10000.0f) return transform;
        else return enemies[closestIndex].transform;
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
