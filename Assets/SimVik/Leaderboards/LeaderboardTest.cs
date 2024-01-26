using UnityEngine;

namespace SimVik.Leaderboards
{
	public class LeaderboardTest : MonoBehaviour
	{
		public int score;


		async void Start()
		{
			await new WaitForSeconds(1);
			var board = FindObjectOfType<LeaderBoard>();
			board.PostScore(Leaderboards.MyName,score);
			//StaticAPITests();
		}

		async void StaticAPITests()
		{
			//_ = Leaderboards.Delete();

			if (await Leaderboards.SetValue("test","Bob", 101))
			{
				print("successfully added score!");
			}

			var score = await Leaderboards.GetValues("test");
			print("User " + score[0].name + " : " + score[0].score);

			var top3 = await Leaderboards.GetValues("test",1,4);
			print("TOP3 : " + string.Join(", ", top3));

			var bobScore = await Leaderboards.GetValue("test","Bob");
			var bobPlace = await Leaderboards.GetPosition("test","Bob");
			print("Bobs score : " + bobScore + "\t place: " + bobPlace);

			var value = await Leaderboards.IncrementValue("test","Bob", 10);
			print( "Bob's incremented score : " + value);

			// BUCKETS
			if (await Leaderboards.IncrementBucket("test",1))
			{
				print( "Bucket incremented!");
			}
			await Leaderboards.IncrementBucket("test",5);

			var buckets = await Leaderboards.GetBucketValues("test");
			print(string.Join(", ", buckets));

			var deleted = await Leaderboards.DeleteBuckets("test");
			print("Buckets deleted : " + deleted);
		}
	}
}