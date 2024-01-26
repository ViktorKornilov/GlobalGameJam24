using UnityEngine;


public class Banana : Tool
{
    public override void Grab(Character character)
    {
        Debug.Log("Grabbing banana");
    }

    public override void Use(Character character)
    {
        Debug.Log("Using banana");
    }

    public override void Drop(Character character)
    {
        Debug.Log("Dropping banana");
    }
}