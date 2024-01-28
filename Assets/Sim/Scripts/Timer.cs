using System.Collections;
using System.Collections.Generic;
using SimVik;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float duration = 1;
    public TextMeshProUGUI text;
    public AudioClip countdownSound;
    public AudioClip timeUpSound;
    public Image radialImage;

    private float time;
    void Start()
    {
        time = duration * 60;
        Invoke( nameof( CountDown), duration * 45);
         Invoke( nameof( TimeUp), duration * 60);
    }

    void CountDown()
    {
     countdownSound.Play();
    }

    void TimeUp()
    {
        timeUpSound.Play();
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

        radialImage.fillAmount = time / (duration * 60);
    }
    
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}