using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private Transform playerTransform;

    private float m_currentSpeed = 0.0f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        m_currentSpeed += 25.0f * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * m_currentSpeed);
    }
}
