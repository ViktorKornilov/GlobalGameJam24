using SimVik.Utility;
using UnityEngine;

public class Audio : Singleton<Audio>
{
	 public AudioSource audioSource;

	 void Awake()
	 {
		 audioSource = gameObject.AddComponent<AudioSource>();
	 }

	 // STATIC METHODS
	 public static void PlaySound(AudioClip clip,float volume = 1f)
	 {
		 Instance.audioSource.PlayOneShot(clip,volume);
	 }
}