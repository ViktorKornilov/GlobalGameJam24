using SimVik;
using UnityEngine;

public class Burger : MonoBehaviour
{
	public AudioClip throwSound;
    public AudioClip eatSound;
    public GameObject eatParticles;

    void Start()
    {
	     throwSound.Play();
    }


    bool isQuitting;
    void OnApplicationQuit()
    {
	    isQuitting = true;
    }

    void OnDestroy()
    {
	    // return if we're quitting
	    if (isQuitting) return;

          eatSound.Play();
          Instantiate(eatParticles, transform.position, Quaternion.identity);
    }
}