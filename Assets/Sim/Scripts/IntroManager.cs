using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 5;
    public Ease ease = Ease.OutCubic;
    public GameObject ui;
    
    private Timer timer;
    bool started = false;
    private Transform target;
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        timer.gameObject.SetActive(false);
        target = Camera.main.transform;
        Camera.main.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if (started)
        {
            transform.LookAt(Vector3.zero);
        }
        
        //if any key is pressed, start the game
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            started = true;
            ui.SetActive(false);
            var sequence = DOTween.Sequence();
            
            sequence.Append(transform.DOMove(target.position, duration).SetEase(ease).SetDelay(1));
            sequence.Append(canvasGroup.DOFade(1, 1));
            sequence.Play().OnComplete(() =>
            {
                target.gameObject.SetActive(true);
                timer.gameObject.SetActive(true);
                Destroy(gameObject);
            });
        }
    }
}
