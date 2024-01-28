using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollPicker : MonoBehaviour
{
    public List<GameObject> polls;
    
    void Start()
    {
        var poll = polls[Random.Range(0, polls.Count)];
        poll.SetActive(true);
    }


}
