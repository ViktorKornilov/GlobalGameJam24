using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
	public List<Transform> target;
	[FormerlySerializedAs("smoothTime")] public float smoothness = 0.3F;

	void LateUpdate()
	{
		if (target.Count > 0) return;
		
		//find mid pos of all targets
		var pos = Vector3.zero;
		foreach (var t in target)
		{
			pos += t.position;
		}
		pos /= target.Count;
		
		//var pos = target.position;
		//pos += Vector3.up * 1;
		transform.position =  Vector3.Lerp(transform.position, pos, 1 - smoothness);
	}
}