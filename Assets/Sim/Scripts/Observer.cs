using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Observer : MonoBehaviour
{
    public List<GameObject> polls;
    
    public Transform head;
    public Transform rightArm;
    public Transform leftArm;
    
    private Quaternion leftRotation;
    private Quaternion rightRotation;


    public void AddPoll(Color color)
    {
        //get random poll
        var poll = polls[Random.Range(0, polls.Count)];
        poll.SetActive(true);
        
        //change renderer color
        var renderer = poll.GetComponent<Renderer>();
        renderer.material.color = color;
    }
    
    void Start()
    {
        transform.DOScaleY(1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutCubic).SetDelay(Random.Range(0,0.5f));
        leftRotation = leftArm.rotation;
        rightRotation = rightArm.rotation;
    }

   
    public void Cheer(float time)
    {
        StartCoroutine(CheerCoroutine(time));
    }

    public IEnumerator CheerCoroutine(float duration)
    {
        var startTime = Time.time;
        var offset = Random.Range(-5, 5f);
        while (Time.time - startTime < duration)
        {
            var armAngle = Mathf.Sin(Time.time * 20 + offset) * 45;
            leftArm.rotation = Quaternion.Euler(-armAngle, -40, 45);
            rightArm.rotation = Quaternion.Euler(armAngle, 40, -45);
            yield return new WaitForEndOfFrame();
        }
        
        leftArm.rotation = leftRotation;
        rightArm.rotation = rightRotation;
        //transform.position += Mathf.Abs(Mathf.Sin(Time.time * 20 + offset)) * Vector3.up * 0.1f;
    }
    
    public void Shout(float time)
    {
        StartCoroutine(ShoutCoroutine(time));
    }

    public IEnumerator ShoutCoroutine(float duration)
    {
        var startTime = Time.time;
        var offset = Random.Range(-5, 5f);
        while (Time.time - startTime < duration)
        {
            var armAngle = Mathf.Sin(Time.time * 20 + offset) * 45;
            leftArm.rotation = Quaternion.Euler(-armAngle, -40, 45);
            //rightArm.rotation = Quaternion.Euler(armAngle, 40, -45);
            yield return new WaitForEndOfFrame();
        }
        
        leftArm.rotation = leftRotation;
        rightArm.rotation = rightRotation;
    }

}
