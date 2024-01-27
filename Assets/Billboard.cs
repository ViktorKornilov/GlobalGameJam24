using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    // always face camera

    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}