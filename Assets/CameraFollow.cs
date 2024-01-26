using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
	public Transform target;
	[FormerlySerializedAs("smoothTime")] public float smoothness = 0.3F;

	void LateUpdate()
	{
		if (target == null) return;
		var pos = target.position;
		//pos += Vector3.up * 1;
		transform.position =  Vector3.Lerp(transform.position, pos, 1 - smoothness);
	}
}