using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimVik;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Tool closestTool;
    public List<Tool> availableTools = new();
    public EquipPopup equipPopup;
    public Tool currentTool;

    [Header("Hitting")]
    public float hitCooldown = 0.5f;
    public float hitCooldownLeft;
    public GameObject hitCollider;
    public AudioClip swooshSound;

    void Update()
    {
        // find closest tool
        var newClosest = availableTools.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
        if(closestTool != newClosest)
        {
            closestTool = newClosest;
            equipPopup.Target = closestTool == null ? null : closestTool.transform;
            print( closestTool);
        }


        hitCooldownLeft -= Time.deltaTime;

        // hold current tool
        if (currentTool != null)
        {
            currentTool.transform.position = transform.position + transform.forward * 0.5f;
            currentTool.transform.rotation = transform.rotation;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        var tool = other.GetComponent<Tool>();
        if (tool == null) return;
        availableTools.Add(tool);
    }

    void OnTriggerExit(Collider other)
    {
        var tool = other.GetComponent<Tool>();
        if (tool == null) return;
        availableTools.Remove(tool);
    }



    public void Use()
    {
        if (currentTool == null)
        {
            Hit();
        }
        else
        {
            currentTool.Use(transform);
        }
    }

    public async void Hit()
    {
        if (hitCooldownLeft > 0) return;
        hitCooldownLeft = hitCooldown;


        hitCollider.SetActive(true);
        await new WaitForSeconds(0.1f);
        hitCollider.SetActive(false);
        swooshSound.Play();
    }

    public void Drop()
    {
        print("trying Dropping");
        if (currentTool == null)
        {
            print("no current tool");
            Grab();
            return;
        }
        currentTool.Drop(transform);
        currentTool = null;
        closestTool = null;
    }

    public void Grab()
    {
        if (currentTool != null) return;
        if (closestTool == null) return;
        currentTool = closestTool;

        currentTool.Grab(transform);
        equipPopup.Target = null;

    }
}