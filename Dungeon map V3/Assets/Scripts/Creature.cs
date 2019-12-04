﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private float speed = 3;
    private float normalSpeed;
    private bool woken = false;
    private GameObject targetObj;

    private Transform playerTransform;
    private Player playerScript;
    private float health = 40.0f;

    private float m_attackCooldown;

    public GameObject coinPickup;

    void Start()
    {
        float randRad = Random.Range(4.5f, 6.0f);
        transform.localScale = new Vector2(randRad, randRad);
       // GetComponent<CircleCollider2D>().radius = randRad * 2;
        normalSpeed = 3 / randRad;
        speed = normalSpeed;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        m_attackCooldown -= Time.deltaTime;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if(distance < 10.0f)
        {
            if(distance < 1.0f)
            {
                Attack();
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * speed);
            }
        }       
    }

   

    void OnTriggerEnter2D(Collider2D col)
    {    
        if(col.tag == "Projectile")
        {
            float damage = col.transform.localScale.x * 30.0F;

            health -= damage;
            Destroy(col.gameObject);
            if(health <= 0.0f)
            {
                Instantiate(coinPickup, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void Attack()
    {
        if(m_attackCooldown <= 0)
        {
            playerScript.TakeDamage(25);
            m_attackCooldown = 3.0f;
        }
    }

    public void Freeze()
    {
        speed = normalSpeed / 3.0f;
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 255);
        Invoke("Unfreeze", 5.0f);
    }

    void Unfreeze()
    {
        speed = normalSpeed;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

}
