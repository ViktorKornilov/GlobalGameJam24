using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof( TMP_Text))]
public class AutoFade : MonoBehaviour
{
	TMP_Text text;

	void OnEnable()
	{
		text = GetComponent<TMP_Text>();
		text.color = text.color.Alpha(0);
		//text.color.TweenAlpha(1f, 1f);
	}
}