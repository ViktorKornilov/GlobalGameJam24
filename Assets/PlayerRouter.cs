using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRouter : MonoBehaviour
{
	PlayerInput playerInput;
	Character character;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		// find character
		character = FindFirstObjectByType<Character>();

		playerInput.actions["Move"].performed += ctx => character.Move(ctx.ReadValue<Vector2>());
		playerInput.actions["Move"].canceled += ctx => character.Move(Vector2.zero);

		playerInput.actions["Fire"].performed +=  _ => character.Use();
		playerInput.actions["Drop"].performed += _ => character.Drop();
	}

}