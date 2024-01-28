using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public int time = 60;
    async void Start()
    {
        while (time > 0)
        {
            time--;
            // Update UI
            radialImage.fillAmount = time / (duration * 60);
            text.text = time.ToString();
            text.transform.DOLocalRotate( new Vector3(0,0,360), 0.3f, RotateMode.FastBeyond360);

            // Special timed events
            if( time == 10)countdownSound.Play();
            if (time == 0)
            {
                timeUpSound.Play();
                AudienceManager.Instance.CrownWinner();
                await new WaitForSeconds(15);
                Restart();
                return;
            }

            await new WaitForSeconds(1);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}