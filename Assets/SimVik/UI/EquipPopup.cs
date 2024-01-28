using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : MonoBehaviour
{
    Transform target;

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            if( target != null )
            {
                // VISIBLE
                image.DOFade(1, fadeTime);
                transform.DOScale( 1, fadeTime * 2).SetEase(Ease.OutBounce);
                //image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                // INVISIBLE
                image.DOFade( 0, fadeTime);
                transform.DOScale( 0, fadeTime);
                //image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public float fadeTime = 0.2f;

    Image image;
    Canvas canvas;
    RectTransform rt;

    void Awake()
    {
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        rt = GetComponent<RectTransform>();
        image.color = new Color(1, 1, 1, 0);
    }

    void LateUpdate()
    {
        if (target == null) return;
        var pos = target.position;
        //pos += Vector3.up * 1;
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
    }
}