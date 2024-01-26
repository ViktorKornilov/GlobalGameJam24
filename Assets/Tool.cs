using UnityEngine;

public abstract class Tool : MonoBehaviour
{
	public abstract void Grab(Character character);
	public abstract void Use(Character character);
	public abstract void Drop(Character character);
}