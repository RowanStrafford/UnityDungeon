using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 dir;
    float m_speed = 20.0f;

    float m_scale;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetDir(float x, float y)
    {
        dir = new Vector2(x, y);
    }

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    public void SetScale(float scale)
    {
        m_scale = scale;
        transform.localScale = new Vector2(m_scale, m_scale);
    }

    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        //transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }

    public void FireProjectile()
    {
        // rb2d.velocity = transform.up * 1.0f;  

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * m_speed * Time.deltaTime);

        m_scale -= 0.5f * Time.deltaTime;
        transform.localScale = new Vector2(m_scale, m_scale);

        if (m_scale <= 0.01f) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        //rb2d.AddForce(transform.right * 1.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
