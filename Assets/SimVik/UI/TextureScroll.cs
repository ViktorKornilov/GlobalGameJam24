using UnityEngine;

public class TextureScroll : MonoBehaviour
{
	public Vector2 scrollSpeed;
	 Renderer rend;
	 void Start()
	 {
		  rend = GetComponent<Renderer>();
	 }

	 void Update()
	 {
		  var offset = Time.time * scrollSpeed;
		  rend.material.SetTextureOffset("_MainTex", offset);
	 }
}