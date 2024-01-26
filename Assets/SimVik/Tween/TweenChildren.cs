using DG.Tweening;
using UnityEngine;

public class TweenChildren : MonoBehaviour
{
    public float childDelay = 0.05f;

    public async void Show()
    {
        var i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            var targetScale = child.transform.localScale.y;
            child.transform.localScale = new Vector3(1, 0, 1);
            child.transform.DOScaleY(targetScale, 0.5f).SetEase(Ease.OutBack).SetDelay(i++ * childDelay);
        }
    }
}