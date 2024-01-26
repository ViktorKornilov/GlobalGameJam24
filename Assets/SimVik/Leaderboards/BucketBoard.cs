using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimVik.Leaderboards;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BucketBoard : MonoBehaviour
{
	public string boardName = "test";
	public int binSize = 25;
	public int maxVisualBuckets = 10;

	public int minScore = 0;
	public int maxScore = 1000;
	int myBest = 0;

	public List<int> buckets = new();

	[Header( "UI" )]
	[SerializeField]GameObject loadingIndicator;
	[SerializeField]GameObject noEntriesIndicator;
	[SerializeField]Transform entryParent;
	[SerializeField]GameObject entryPrefab;
	[SerializeField]TMP_Text maxBucketText;

	public UnityEvent onNewBest = new();
	public UnityEvent onGetScores = new();

	async void Start()
	{
		//for(int i = 0;i < 10;i++)PostScoreTest();
		await GetScores();
		UpdateEntries();
	}

	void Update()
	{
		transform.position += Vector3.up * Time.deltaTime * 100f;
	}


	public void UpdateEntries()
	{
		// delete old entries
		foreach (Transform child in entryParent) Destroy(child.gameObject);

		// combine buckets if too many
		List<int> finalBuckets = new();
		var bucketSizeMultiplier = 1;
		if (buckets.Count > maxVisualBuckets)
		{
			bucketSizeMultiplier = buckets.Count / maxVisualBuckets;
			for (int i = 0; i < buckets.Count; i += bucketSizeMultiplier)
			{
				var bucket = buckets.GetRange(i, bucketSizeMultiplier);
				finalBuckets.Add(bucket.Sum());
			}
		}
		else finalBuckets = buckets;


		var max = finalBuckets.Max();
		// populate new entries
		foreach (var entry in finalBuckets)
		{
			var go = Instantiate(entryPrefab, entryParent);
			var ratioHeight = (float)entry / max + 0.05f;
			go.transform.localScale = new Vector3(1, ratioHeight, 1);
		}

		maxBucketText.text = buckets.Count * binSize + "";

		// show my best if any
		myBest = PlayerPrefs.GetInt("bucket_" + boardName + "_score", 0);
		if (myBest > 0)
		{
			var myFinalBucket = myBest / binSize / bucketSizeMultiplier;
			//print( "my best : " + myBest + " bucket : " + finalBucket);
			// make that bucket red
			var myBucket = entryParent.GetChild(myFinalBucket);
			myBucket.GetComponent<Image>().color = Color.red;
		}

		onGetScores.Invoke();
	}


	[ ContextMenu( "Get Scores" )]
	public async Task GetScores()
	{
		buckets = await Leaderboards.GetBucketValues(boardName);
		if (buckets.Count == 0)
		{
			 noEntriesIndicator.SetActive(true);
		}
	}


	public async void PostScore(int score)
	{
		if(score < minScore || score > maxScore) return;
		myBest = PlayerPrefs.GetInt("bucket_" + boardName + "_score", 0);
		if (score <= myBest) return;

		myBest = score;
		onNewBest.Invoke();
		PlayerPrefs.SetInt("bucket_" + boardName + "_score", myBest);

		var bucketID = myBest / binSize;
		await Leaderboards.IncrementBucket(boardName, bucketID);
	}


	[ContextMenu("Post Random Score")]
	public void PostScoreTest()
	{
		var score = Random.Range(0, 1000);
		PostScore(score);
	}
}