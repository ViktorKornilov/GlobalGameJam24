using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRouter : MonoBehaviour
{
	PlayerInput playerInput;
	Movement character;
	Inventory inventory;

	void Start()
	{
		playerInput = GetComponent<PlayerInput>();

		// find character
		//character = FindFirstObjectByType<Movement>();
		//inventory = FindFirstObjectByType<Inventory>();
		//var chars = FindObjectsByType<Movement>(FindObjectsSortMode.None);
		//var invs = FindObjectsByType<Inventory>(FindObjectsSortMode.None);

		/*for (int i = 0; i < chars.Length; i++)
		{
			if (chars[i].bot)
			{
				character = chars[i];
				//inventory = invs[i];
				character.bot = false;
				break;
			}
		}*/

		foreach (var player in AudienceManager.Instance.players)
		{
			var m = player.transform.GetComponent<Movement>();
			if (m.bot)
			{
				character = m;
				character.bot = false;
				break;
			}
		}

		

		try
		{
			playerInput.actions["Move"].performed += ctx => character.Move(ctx.ReadValue<Vector2>());
			playerInput.actions["Move"].canceled += ctx => character.Move(Vector2.zero);

			playerInput.actions["Fire"].performed += _ => character.Hit();
			playerInput.actions["Drop"].performed += _ => character.Fart();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
		
	}

}