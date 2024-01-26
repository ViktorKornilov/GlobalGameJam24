using UnityEngine.Networking;

namespace SimVik.Leaderboards
{
	public static class UnityWebRequestExtensions
	{
		public static bool HasSucceeded(this UnityWebRequest request)
		{
			return request.result == UnityWebRequest.Result.Success;
		}

		public static bool HasFailed(this UnityWebRequest request)
		{
			return request.result != UnityWebRequest.Result.Success;
		}
	}
}