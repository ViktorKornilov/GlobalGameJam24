using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnimationState
{
    Active,
    Dazed,
    Carrying,
    Dead
}

public class Animator : MonoBehaviour
{
    public bool running = false;
    public bool animateHands = true;
    public AnimationState state = AnimationState.Active;
    
    public float walkSpeed = 5;
    public float runSpeed = 10;
    
    [Header("Body Parts")]
    public Transform rightLeg;
    public Transform leftLeg;
    public Transform head;
    public Transform rightArm;
    public Transform leftArm;

    [Header("VFX")] 
    public GameObject dazedParticles;
    
    private float armSpeedOffset;
    private float headSpeedOffset;

    private void Start()
    {
        armSpeedOffset = Random.Range(1, 1.5f);
        headSpeedOffset = Random.Range(1, 1.5f);
    }

    void Update()
    {
        if(state == AnimationState.Active)
            AnimateBody();
        
        if(state == AnimationState.Dazed)
            Dazed();
        else //TODO: Remove this
        {
            dazedParticles.SetActive(false);
        }
        
    }

    public void Dazed()
    {
        head.rotation = Quaternion.Euler(Mathf.Sin(Time.time * 4) * 30, 0, 0);
        dazedParticles?.SetActive(true);
    }
    
    public void AnimateBody()
    {
        var speed = running? runSpeed : walkSpeed;
        
        var legAngle = Mathf.Sin(Time.time * speed) * 45;

        rightLeg.rotation = Quaternion.Euler(-180, 0, legAngle);
        leftLeg.rotation = Quaternion.Euler(180, 0, -legAngle);

        if (animateHands)
        {
            var armAngle = Mathf.Sin(Time.time * (speed * armSpeedOffset)) * 45;
            leftArm.rotation = Quaternion.Euler(170, -40, armAngle);
            rightArm.rotation = Quaternion.Euler(-170, 40, -armAngle);
        }

        //var headBob = Mathf.Sin(Time.time * (speed * headSpeedOffset)) * 45;
        //head.localPosition = new Vector3(0, 0.008f + headBob / 45 * 0.001f, 0) ;
    }
}
