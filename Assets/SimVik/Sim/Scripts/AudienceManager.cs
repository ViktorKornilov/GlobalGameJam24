using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class PlayerInfo
{
    public Transform transform;
    public float approval = 0;
    public Transform uiBar;
}

public class AudienceManager : MonoBehaviour
{
    public static AudienceManager Instance;
    
    public List<PlayerInfo> players;
    
    public List<Color> palette;
    public GameObject observerPrefab;
    
    [Header("Population settings")]
    public int segments = 10;
    public int peoplePerSegment = 10;
    public float arenaWdth = 5;
    public float arenaHeight = 3;

    [Header("Clips")] 
    public AudioClip cheers;
    public AudioClip boo;
    public AudioClip laugh;
    public AudioClip ew;

    
    [Header("Brains")]
    [Range(0,1)]
    public float chanceToReact = 0.1f;
    public float startingDistance = 3;
    
    
    private Transform[,] audience;
    private Vector3[] audiencePositions;
    private bool[] inAction;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        audience = new Transform[segments, peoplePerSegment];
        audiencePositions = new Vector3[segments];
        inAction = new bool[segments];

        var angle = 360f / segments * Mathf.Deg2Rad;
        var pAngle = 360f / peoplePerSegment  * Mathf.Deg2Rad;
        
        for (int s = 0; s < segments; s++)
        {
            var x = Mathf.Sin(angle * s) * arenaWdth;
            var z = Mathf.Cos(angle * s) * arenaWdth;
            
            //var o = Instantiate(observerPrefab, new Vector3(x, arenaHeight, z), Quaternion.identity);
            //o.transform.LookAt(Vector3.zero);
            audiencePositions[s] = new Vector3(x, 0, z);

            for (int p = 0; p < peoplePerSegment; p++)
            {
                var px = Mathf.Sin(pAngle * p) * 2 + Random.Range(-0.5f, 0.5f);
                var pz = Mathf.Cos(pAngle * p) * 2 + Random.Range(-0.5f, 0.5f);
                
                var person = Instantiate(observerPrefab, new Vector3(x, arenaHeight, z) + new Vector3(px, arenaHeight, pz), Quaternion.identity);
                person.transform.LookAt(Vector3.zero);
                //every person should have different color
                person.GetComponentInChildren<Renderer>().material.color = palette[Random.Range(0, palette.Count)];
                
                audience[s, p] = person.transform;
            }
        }
    }

    void Update()
    {
        foreach (var player in players)
        {
            for (int i = 0; i < segments; i++)
            {
                var distance = Vector3.Distance(player.transform.position, audiencePositions[i]);

                if (distance <= startingDistance)
                {
                    var percentage = distance / startingDistance * chanceToReact;
                    
                    if (Random.Range(0f, 1f) <= percentage && !inAction[i])
                    {
                        //TODO: check approval
                        if (player.approval >= 50)
                        {
                            Audio.PlaySound(cheers, 0.5f);
                            Cheer(i);
                        }
                        else
                        {
                            Audio.PlaySound(boo, 0.5f);
                            Shout(i);
                        }
                        
                    }
                }
                
            }
            
        }
    }

    /*private void OnDrawGizmos()
    {
        //check if play mode
        if (!Application.isPlaying) return;
        
        for (int i = 0; i < segments; i++)
        {
            var distance = Vector3.Distance(players[0].transform.position, audiencePositions[i]);

            if(distance < startingDistance) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            
            Gizmos.DrawLine(players[0].transform.position, audiencePositions[i]);
        }
    }*/

    public void Cheer(int segmentIndex)
    {
        if(inAction[segmentIndex]) return;
        
        inAction[segmentIndex] = true;
        for (int i = 0; i < peoplePerSegment; i++)
        {
            var member = audience[segmentIndex, i];
            var startPos = member.transform.position;
            
            //member.DOMove(member.position + Vector3.up * 0.5f, 0.3f).ChangeStartValue(startPos).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic).SetDelay(Random.Range(0,0.3f));
            member.GetComponent<Observer>().Cheer(0.5f);
        }
        inAction[segmentIndex] = false;
    }
    
    public void Shout(int segmentIndex)
    {
        if(inAction[segmentIndex]) return;
        
        inAction[segmentIndex] = true;
        for (int i = 0; i < peoplePerSegment; i++)
        {
            var member = audience[segmentIndex, i];
            var startPos = member.transform.position;
            
            //member.DOMove(member.position + Vector3.up * 0.5f, 0.3f).ChangeStartValue(startPos).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic).SetDelay(Random.Range(0,0.3f));
            member.GetComponent<Observer>().Shout(0.5f);
        }
        inAction[segmentIndex] = false;
    }

    public void Laugh()
    {
        Audio.PlaySound(laugh, 1);
        for (int i = 0; i < segments; i++)
        {
            Cheer(i);
        }
    }

    public void Hate()
    {
        Audio.PlaySound(ew, 1);
    }

    public void SetApproval(Transform player, float approval)
    {
        print(transform.name + " " + approval);
        //find player
        foreach (var p in players)
        {
            if (p.transform == player)
            {
                print(player.name);
                p.approval += approval;

                if (p.approval < 0) p.approval = 0;
                
                p.uiBar.DOScaleX(p.approval / 100f, 0.3f).SetEase(Ease.OutBounce);
                return;
            }
        }
    }
}
