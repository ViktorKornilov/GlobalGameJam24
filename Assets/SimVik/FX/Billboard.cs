using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    // always face camera

    void LateUpdate()
    {
        if(Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}