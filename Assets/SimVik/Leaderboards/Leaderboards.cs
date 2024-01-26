using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SimVik.Leaderboards
{
	public static class Leaderboards
	{
		/// <summary>
		///  The base url for the leaderboard server. Used by all API calls. Change this to your own server if you want to host your own leaderboard.
		/// </summary>
		public static string baseUrl = "https://simvik.fly.dev/piped";

		/// <summary>
		/// a place to store the name of a player. Saved between sessions. Does not affect the API calls.
		/// </summary>
		static string _myName = "";
		public static string MyName
		{
			get
			{
				_myName = PlayerPrefs.GetString("user_name", "dude");
				return _myName;
			}
			set
			{
				_myName = value;
				PlayerPrefs.SetString("user_name", _myName);
			}
		}


		/// <summary>
		/// Gets the current leaderboard values.
		/// </summary>
		/// <returns> A string of the current leaderboard values. null if failed to find or load the board</returns>
		public static async Task<List<(string name,int score)>> GetValues(string boardName,int from = -1, int to = 50)
		{
			var url = $"{baseUrl}/boards/{boardName}";
			if(from >= 0) url += $"/{from}-{to}";

			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				var values = new List<(string name, int score)>();

				var lines = request.downloadHandler.text.Split("|");
				foreach (var line in lines)
				{
					var parts = line.Split(",");
					if (parts.Length < 2) break;
					values.Add((parts[0], int.Parse(parts[1])));
				}


				return values;
			}

			return null;
		}


		/// <summary>
		/// Adds a value to the leaderboard. If the value already exists, it will be updated only if score is new best.
		/// </summary>
		/// <returns> true if value was added or updated</returns>
		public static async Task<bool> SetValue(string boardName,string name, int score)
		{
			var url = $"{baseUrl}/boards/{boardName}/{name}/{score}";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();

			return request.HasSucceeded();
		}


		/// <summary>
		/// Gets the current position of the value in the leaderboard in descending order. 0 if not found.
		/// </summary>
		public static async Task<int> GetPosition(string boardName,string name)
		{
			var url = $"{baseUrl}/boards/{boardName}/{name}/position";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			if (request.HasFailed()) return 0;

			return int.Parse(request.downloadHandler.text);
		}


		/// <summary>
		/// Removes current board from database. Use with caution. This cannot be undone.
		/// </summary>
		public static async Task<bool> Delete(string boardName)
		{
			var url = $"{baseUrl}/boards/{boardName}/delete";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();

			return request.HasSucceeded();
		}


		/// <summary>
		///  Gets the current value for the name. 0 if not found.
		/// </summary>
		public static async Task<int> GetValue(string boardName, string name)
		{
			var url = $"{baseUrl}/boards/{boardName}/{name}";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			if (request.HasFailed()) return 0;

			return int.Parse(request.downloadHandler.text.Split("|")[0]);
		}


		/// <summary>
		///  Deletes the value for the name. true if deleted.
		/// </summary>
		public static async Task<bool> DeleteValue(string boardName,string name)
		{
			var url = $"{baseUrl}/boards/{boardName}/{name}/delete";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();

			return request.HasSucceeded();
		}


		/// <summary>
		/// Increments the value for the name. return true if incremented.
		/// </summary>
		public static async Task<int> IncrementValue(string boardName,string name, int increment = 1)
		{
			var url = $"{baseUrl}/boards/{boardName}/{name}/{increment}/add";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			if (request.HasFailed()) return 0;

			return int.Parse(request.downloadHandler.text);
		}


		/// <summary>
		/// Bucket boards are used to count same values. Returns a list of every value occurrences up to max value.
		/// </summary>
		public static async Task<List<int>> GetBucketValues(string boardName)
		{
			var url = $"{baseUrl}/buckets/{boardName}";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			if (request.HasFailed()) return null;

			// if board is empty, return empty list
			var values = new List<int>();
			var text = request.downloadHandler.text;
			if (text[0] == 'B') return values;

			var lines = text.Split("|");
			var currentBucket = 0;

			foreach (var line in lines)
			{
				var parts = line.Split(",");
				var bucketID = int.Parse(parts[0]);
				var count = int.Parse(parts[1]);

				// add 0's for non existing buckets
				while (currentBucket < bucketID && currentBucket < 1000)
				{
					values.Add(0);
					currentBucket++;
				}

				currentBucket++;
				values.Add(count);
			}

			return values;
		}


		/// <summary>
		/// Increments the bucket value. return true if incremented.
		/// </summary>
		public static async Task<bool> IncrementBucket(string boardName,int bucket)
		{
			var url = $"{baseUrl}/buckets/{boardName}/{bucket}";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			
			return request.HasSucceeded();
		}


		/// <summary>
		/// Deletes a board with all buckets. Use with caution. This cannot be undone.
		/// </summary>
		public static async Task<bool> DeleteBuckets(string boardName)
		{
			var url = $"{baseUrl}/buckets/{boardName}/delete";
			var request = UnityWebRequest.Get(url);
			await request.SendWebRequest();

			return request.HasSucceeded();
		}
	}
}