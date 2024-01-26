using UnityEngine;

public abstract class Tool : MonoBehaviour
{

	public abstract void Grab(Transform character);
	public abstract void Use(Transform character);
	public abstract void Drop(Transform character);
}