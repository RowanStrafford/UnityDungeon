using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 1;

    private int health = 100;
    private int m_coins = 0;

    public float firerate = 1.0f;

    public GameObject bullet;

    public Slider m_healthSlider;
    public Text m_coinText;

    void Start()
    {
        m_healthSlider.value = health;
        m_coinText.text = m_coins.ToString();
    }

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        transform.position = transform.position + (movement * Time.deltaTime * speed);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Swipe");
            transform.GetChild(0).gameObject.SetActive(true);
            Invoke("Disable", 1.0f);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }

        if(Input.GetMouseButton(0))
        {
            Fire();
        }

        firerate -= Time.deltaTime;



    }

    void Fire()
    {
        if(firerate <= 0)
        {
            Vector2 dir = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            Projectile p = obj.GetComponent<Projectile>();
            p.SetDir(dir.x, dir.y);
            p.SetSpeed(20);
            p.SetScale(0.5f);

            firerate = 0.5f;
        }
    }

    void Disable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {     
        if (col.tag == "CoinPickup")
        {
            m_coins++;
            m_coinText.text = m_coins.ToString();
            Destroy(col.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("health : " + health);
        health -= damage;
        m_healthSlider.value = health;
    }
}
