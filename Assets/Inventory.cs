using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Tool closestTool;
    public List<Tool> availableTools = new();
    public EquipPopup equipPopup;



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
}