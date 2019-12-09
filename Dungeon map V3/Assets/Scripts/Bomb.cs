using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float m_countdown = 5.0f;

    private Animator m_anim;

    private float m_explosionRadius = 3.0f;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        CountDown();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Projectile")
        {
            m_anim.SetTrigger("Explode");
            Destroy(col.gameObject);
        }
    }
        

    void CountDown()
    {
        m_countdown -= Time.deltaTime;

        if(m_countdown <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        m_anim.SetTrigger("Explode");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        CheckDamage(enemies);

        Die();
    }

    void CheckDamage(GameObject[] enemies)
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, enemies[i].transform.position);

            // We are within the radius of explosion
            if(distance < m_explosionRadius)
            {
                float damageToDeal = (m_explosionRadius - distance) * 100.0f;
                enemies[i].GetComponent<Creature>().TakeDamageDelay(damageToDeal);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject, 1.0f);
    }
}
