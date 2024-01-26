using SimVik.Leaderboards;
using TMPro;
using UnityEngine;

public class PlayCounter : MonoBehaviour
{
	async void Start()
	{
		var plays = await Leaderboards.IncrementValue("stats","plays");
		GetComponent<TMP_Text>().text = plays + " plays";
	}
}