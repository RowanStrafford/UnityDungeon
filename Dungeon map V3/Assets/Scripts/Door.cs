using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform target;
    public GameObject[] enemies;

    public bool horiz = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int deathCount = 0;

        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                deathCount++;
            } 
        }
        if(deathCount == 3) OpenDoor();
    }

    void OpenDoor()
    {

        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 2.0f);
    }
}
