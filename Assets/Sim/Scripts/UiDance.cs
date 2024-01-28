using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UiDance : MonoBehaviour
{
    [Header("Scale")]
    public bool scale = true;
    public float scaleAmount = 1.5f;
    public float scaleTime = 1;
    
    [Header("Rotate")]
    public bool rotate = false;
    public float rotateAmount = 1.5f;
    public float rotateTime = 1;
    public Ease rotateEase = Ease.OutCubic;
    
    void Start()
    {
        if (scale)
        {
            transform.DOScale(Vector3.one * scaleAmount, scaleTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutCubic);
        }

        if (rotate)
        {
           Sequence sequence = DOTween.Sequence();
           sequence.Append(transform.DORotate(Vector3.forward * rotateAmount, rotateTime).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic));
           sequence.Append(transform.DORotate(Vector3.forward * -rotateAmount, rotateTime).SetLoops(2, LoopType.Yoyo).SetEase(rotateEase));
           sequence.SetLoops(-1, LoopType.Restart).Play();
        }
    }
    
}
