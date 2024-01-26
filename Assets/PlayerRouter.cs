using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRouter : MonoBehaviour
{
	PlayerInput playerInput;
	Movement character;
	Inventory inventory;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		// find character
		character = FindFirstObjectByType<Movement>();
		inventory = FindFirstObjectByType<Inventory>();

		playerInput.actions["Move"].performed += ctx => character.Move(ctx.ReadValue<Vector2>());
		playerInput.actions["Move"].canceled += ctx => character.Move(Vector2.zero);

		playerInput.actions["Fire"].performed +=  _ => inventory.Use();
		playerInput.actions["Drop"].performed += _ => inventory.Drop();
	}

}