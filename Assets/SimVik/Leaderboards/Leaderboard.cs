using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimVik.Leaderboards
{
	public class LeaderBoard : MonoBehaviour
	{
		public string boardName = "test";
		public int maxEntries = 10;
		[SerializeField]int minScore = 0;
		[SerializeField]int maxScore = 1000;

		[Header("UI")]
		[SerializeField]LeaderboardEntryUI entryPrefab;
		[SerializeField]GameObject loadingIndicator;
		[SerializeField]GameObject noEntriesIndicator;
		[SerializeField]LeaderboardEntryUI myEntryIndicator;
		[SerializeField]Transform entryParent;

		List<(string name, int score)> entries = new();
		[SerializeField]int myBest = 0;
		string bestScoreKey = "leaderboard_";

		public UnityEvent onNewBest = new();
		public UnityEvent onGetScores = new();

		async void Start()
		{
			bestScoreKey += boardName;
			myBest = PlayerPrefs.GetInt(bestScoreKey, 0);

			loadingIndicator.SetActive(true);
			entries = await Leaderboards.GetValues(boardName);

			loadingIndicator.SetActive(false);
			if (entries.Count == 0) noEntriesIndicator.SetActive(true);
			else UpdateEntries();
		}

		void UpdateEntries()
		{
			// delete old entries
			foreach (Transform child in entryParent) Destroy(child.gameObject);

			// populate new entries
			foreach (var entry in entries)
			{
				var go = Instantiate(entryPrefab.gameObject, entryParent);
				var entryUI = go.GetComponent<LeaderboardEntryUI>();
				entryUI.Set(entries.IndexOf(entry) + 1, entry.name, entry.score);
			}

			onGetScores.Invoke();
		}


		public async void PostScore(string userName, int score)
		{
			// post only if new best
			if (score <= myBest) return;
			myBest = score;
			onNewBest.Invoke();
			PlayerPrefs.SetInt(bestScoreKey, myBest);

			// is it worth posting?
			if (score < minScore || score > maxScore) return;
			var minTopScore = entries.Count > 0 ? entries[^1].score : 0;
			if (score <= minTopScore && entries.Count >= maxEntries)
			{
				// show self in leaderboard with position as last entry
				var position = await Leaderboards.GetPosition(boardName,userName);
				myEntryIndicator.gameObject.SetActive(true);
				myEntryIndicator.Set(position + 1, userName, score);
			}
			else
			{
				// post score and get new leaderboard
				entries = await Leaderboards.GetValues(boardName);
				UpdateEntries();
			}
		}
	}
}