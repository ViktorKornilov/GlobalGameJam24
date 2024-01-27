using UnityEngine;

namespace SimVik
{
	public static class AudioExtensions
	{
		public static void Play(this AudioClip clip,float volume = 1f)
		{
			Audio.PlaySound(clip,volume);
		}
	}
}