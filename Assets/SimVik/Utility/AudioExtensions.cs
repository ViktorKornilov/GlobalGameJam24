using UnityEngine;

namespace SimVik
{
	public static class AudioExtensions
	{
		public static void Play(this AudioClip clip)
		{
			Audio.PlaySound(clip);
		}
	}
}