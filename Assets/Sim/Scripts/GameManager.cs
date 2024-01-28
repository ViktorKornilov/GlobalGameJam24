using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SoundPlayer fartSound;

	void Start()
	{
		Movement.onDaze.AddListener(CameraShake.Shake);
		Movement.OnFart.AddListener(fartSound.Play);
	}
}