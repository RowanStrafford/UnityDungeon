﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    bool triggered = false;
    public GameObject bullet;
    float speed = 1.0f;
    Transform playerPos;

    float m_health = 300.0f;

    public GameObject m_coinDrop;

    private CircleCollider2D m_col;

    public Slider m_healthSlider;
    private bool m_fadeIn = false;

    private float m_targetSliderVal;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        m_col = GetComponent<CircleCollider2D>();
        m_healthSlider.maxValue = m_health;
        m_healthSlider.value = m_health;
        m_healthSlider.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(playerPos.position, transform.position);
        if((dist < 10.0f) && (!triggered))
        {
            InvokeRepeating("Fire", 2.0f, 2.0f);

            triggered = true;

            m_fadeIn = true;
            m_healthSlider.gameObject.SetActive(true);
        }

        Vector3 targetPosition = playerPos.position;
        Vector3 dir = targetPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        LerpSlider();

    }

    void LerpSlider()
    {
        m_healthSlider.value = Mathf.MoveTowards(m_healthSlider.value, m_health, 30.0f * Time.deltaTime);
    }

    void Die()
    {
        for(int i = 0; i < 5; i++)
        {            
            Vector2 direction = Random.insideUnitCircle.normalized;
            direction *= m_col.radius;
            Instantiate(m_coinDrop, (Vector2)transform.position + direction, Quaternion.identity);
            
        }
        Destroy(gameObject);
    }

    void Fire()
    {
        GameObject g = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

        Vector2 diff = playerPos.position - transform.position;
        Projectile p = g.GetComponent<Projectile>();
        p.SetDir(diff.x, diff.y);
        p.SetSpeed(3.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Projectile")
        {
            Destroy(col.gameObject);
            m_health -= 30.0f;
            m_targetSliderVal = m_health;

            if(m_health <= 0)
            {
                Die();
            }
        }
    }

    void RapidFire()
    {

    }
}
