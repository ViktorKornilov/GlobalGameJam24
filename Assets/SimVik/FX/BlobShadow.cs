using UnityEngine;

public class BlobShadow : MonoBehaviour
{
	Transform target;

	void Start()
	{
		target = transform.parent;
		 transform.parent = null;
	}

	void LateUpdate()
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		var pos = target.position;
		pos.y = -0.49f;
		transform.position = pos;
	}
}