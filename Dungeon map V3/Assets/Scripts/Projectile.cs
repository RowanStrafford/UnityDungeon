using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 dir;
    float m_speed = 20.0f;

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
        transform.localScale = new Vector2(scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * m_speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
