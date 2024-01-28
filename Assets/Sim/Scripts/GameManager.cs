using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SoundPlayer fartSound;
	public SoundPlayer laugh;
	public SoundPlayer ew;

	void Start()
	{
		Movement.onDaze.AddListener(CameraShake.Shake);
		Movement.OnFart.AddListener(fartSound.Play);
		Movement.OnFart.AddListener(CameraShake.Shake);

		Movement.onHit.AddListener(OnPlayerHit);
		Movement.onMiss.AddListener(OnPlayerMiss);
	}

	public async void OnPlayerHit()
	{
		await new WaitForSeconds(0.5f);
		laugh.Play();
	}

	public async void OnPlayerMiss()
	{
		await new WaitForSeconds(0.5f);
		ew.Play();
	}
}