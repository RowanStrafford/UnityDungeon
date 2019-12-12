using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamesystem : MonoBehaviour
{

    public GameObject m_textObj;
    private Text m_text;

    public GameObject m_finalBoss;

    void Start()
    {
        m_textObj.SetActive(true);
        m_text = m_textObj.transform.GetChild(0).GetComponent<Text>();
        m_text.text = "You have entered the Dungeon. Can you find your friends?";
        Invoke("DisableText", 5.0f);
    }

    void DisableText()
    {
        m_textObj.SetActive(false);
    }

    void Update()
    {
        if(m_finalBoss == null)
        {
            m_textObj.SetActive(true);
            m_text.text = "You have killed the final boss! You are free to leave the Dungeon.";
            Invoke("EndGame", 3.0f);
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
