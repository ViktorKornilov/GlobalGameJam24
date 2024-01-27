using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float duration = 1;
    public TextMeshProUGUI text;

    private float time;
    void Start()
    {
        time = duration * 60;
    }

    void Update()
    {
        if (time < 0)
        {
            AudienceManager.Instance.CrownWinner();
            Invoke(nameof(Restart), 15);
        }
        else
        {
            time -= Time.deltaTime; 
            text.text = $"{Mathf.FloorToInt(time / 60)}:{Mathf.FloorToInt(time % 60)}";
        }
    }
    
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
