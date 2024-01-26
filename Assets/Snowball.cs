using UnityEngine;


public class Snowball : Tool
{
    protected Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    public override void Grab(Transform character)
    {
        Debug.Log("Grabbing banana");
    }

    public override void Use(Transform character)
    {
        Drop( character);
    }

    public override void Drop(Transform character)
    {
        rb.velocity = transform.forward * 10;
        Destroy( gameObject, 1f);
    }
}