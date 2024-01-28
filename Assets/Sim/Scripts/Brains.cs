using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brains : MonoBehaviour
{
    public float randomChoiceInterval = 5;
    public float wanderRadius = 15;
    public float burgerRange = 5;
    public float playerRange = 5;
    public float fartRange = 5;

    Movement movement;
    Vector3 targetPos;
    Movement enemy;

    void Start()
    {
        movement = GetComponent<Movement>();
        RandomRoutine();
    }

    async void RandomRoutine()
    {
        while (true)
        {
            var time = Random.Range(randomChoiceInterval,randomChoiceInterval*2);
            await new WaitForSeconds(time);

            int choice = Random.Range(0, 3);
            if (choice == 0)
            {
                // attack
                targetPos = movement.enemy.transform.position;
            }else if (choice == 1)
            {
                // fart
                if (movement.CanFart())
                {
                    movement.Fart();
                }
            }else if (choice == 2)
            {
                // stop
                targetPos = transform.position;
            }
        }
    }

    Burger FindClosestBurger()
    {
        var burgers = FindObjectsOfType<Burger>();
        Burger closest = null;
        var dist = float.MaxValue;
        foreach (var burger in burgers)
        {
            var d = Vector3.Distance(transform.position, burger.transform.position);
            if (d < dist)
            {
                dist = d;
                closest = burger;
            }
        }

        return closest;
    }


    void Update()
    {
        var targetDist = Vector3.Distance(transform.position, targetPos);
        if (targetDist < 1f)
        {
            FindNewTarget();
            // fart if close to player
            var dist = Vector3.Distance(transform.position, movement.enemy.transform.position);
            if (movement.CanFart() && dist < fartRange)
            {
                movement.Fart();
            }
        }

        // move towards target
        var dir = targetPos - transform.position;
        movement.Move(new Vector2(dir.x, dir.z));
    }

    void FindNewTarget()
    {
        // try to find a burger
        var burger = FindClosestBurger();
        if(burger != null && Vector3.Distance(transform.position, burger.transform.position) <  burgerRange)
        {
            targetPos = burger.transform.position;
            return;
        }

        // if have fart, find a player
        if (movement.CanFart())
        {
            var dist = Vector3.Distance(transform.position, movement.enemy.transform.position);
            if (dist <  playerRange)
            {
                targetPos = movement.enemy.transform.position;
                return;
            }
        }

        // otherwise find a random position
        var randomPos = Random.insideUnitSphere * wanderRadius;
        randomPos.y = 0;
        targetPos = transform.position + randomPos;
    }
}