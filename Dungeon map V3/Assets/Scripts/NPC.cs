using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // The player has approached the NPC
        if (m_state == 0)
        {
            if ((distance < 2.0f) && (!m_playerScript.m_NPCChosen))
            {
                m_textObj.SetActive(true);
                m_text.text = "TA : A very friendly AI that throws bombs.";
                m_state = 1;
                Invoke("DisableText", 3.0f);
                m_playerScript.m_NPCChosen = true;
                m_playerScript.AddCoins(-2);
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

        // Attack an enemy, then return to following the player
        if (m_state == 2)
        {
           // GameObject closestEnemy = FindNearestEnemy();
            Transform newPos = FindNearestEnemy();
            if(newPos.position == transform.position)
            {
                Debug.Log("No enemies in this room");
                m_state = 1;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, newPos.position, Time.deltaTime * m_moveSpeed * 2);

                if (Vector2.Distance(transform.position, newPos.position) < 0.5f)
                {
                    m_freezeTimer = 10.0f;
                    AttackEnemy(newPos.gameObject);
                    m_state = 1;
                }
            }            
        }       
    }

    // //This function finds the nearest enemy that is within 10 units.
    //GameObject FindNearestEnemy()
    //{

    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    float closestDist = 10000.0f;
    //    int closestIndex = 10;

    //    for (int i = 0; i < enemies.Length; i++)
    //    {
    //        float distance = Vector2.Distance(transform.position, enemies[i].transform.position);

    //        //If the distance is less than the least distance found, then replace it(this new one is the new closest)
    //        if (distance < closestDist)
    //        {
    //            closestDist = distance;
    //            closestIndex = i;
    //        }
    //    }

    //    return enemies[closestIndex];
    //}

    // This function finds the nearest enemy that is within 10 units. 
    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDist = 10000.0f;
        int closestIndex = 10;

        for (int i = 0; i < enemies.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, enemies[i].transform.position);           

            if(distance > 10.0f) continue;            

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

    void AttackEnemy(GameObject enemy)
    {
        enemy.GetComponent<Creature>().Freeze();
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerTransform.position, Time.deltaTime * m_moveSpeed);
    }
}
