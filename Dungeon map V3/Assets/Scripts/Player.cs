using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 1;

    private int health = 100;
    public int m_coins = 0;

    public float firerate = 1.0f;

    public GameObject bullet;

    public Slider m_healthSlider;
    public Text m_coinText;

    private float m_maxSize = 1.0f;
    private float m_currentScale;
    private GameObject m_currentBullet;
    private Transform m_gunTip;

    public bool m_NPCChosen = false;

    void Start()
    {
        m_healthSlider.value = health;
        m_coinText.text = m_coins.ToString();

        if(transform.childCount > 0)
        {
            m_gunTip = transform.GetChild(0);
        }
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

        if(Input.GetMouseButtonDown(0))
        {
            BeginFire();
        }

        if(Input.GetMouseButton(0))
        {
            HoldFire();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        firerate -= Time.deltaTime;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rot;

    }

    void BeginFire()
    {
        //m_currentBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

        m_currentScale = 0.2f;
        //m_currentBullet.transform.localScale = new Vector2(m_currentScale, m_currentScale);
    }

    void HoldFire()
    {
        m_currentScale += Time.deltaTime * 1.0f;
        if(m_currentScale >= 1.0f)
        {
            m_currentScale = 1.0f;
        }

        //m_currentBullet.transform.localScale = new Vector2(m_currentScale, m_currentScale);
    }

    void Fire()
    {
        Vector2 dir = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        GameObject obj = Instantiate(bullet, m_gunTip.position, Quaternion.identity) as GameObject;

        Projectile p = obj.GetComponent<Projectile>();
        p.SetDir(dir.x, dir.y);
        p.SetSpeed(20);
        p.SetScale(m_currentScale);
        p.SetRotation(transform.position);
       // Vector2 dir2 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float lookAngle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
        //p.SetRotation(lookAngle);

        //p.FireProjectile();
        
        /*
        if(firerate <= 0)
        {
            Vector2 dir = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            Projectile p = obj.GetComponent<Projectile>();
            p.SetDir(dir.x, dir.y);
            p.SetSpeed(20);
            p.SetScale(0.5f);

            firerate = 0.5f;
        }*/
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

        if(col.tag == "BossProjectile")
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("health : " + health);
        health -= damage;
        m_healthSlider.value = health;

        if(health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        m_coins += coinsToAdd;
        m_coinText.text = m_coins.ToString();
    }
}
