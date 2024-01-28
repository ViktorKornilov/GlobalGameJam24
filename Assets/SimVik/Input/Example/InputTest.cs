using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
	PlayerInput playerInput;

	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
	}

	void Update()
	{
		var device = playerInput.devices[0];
		var name = device.GetType().ToString();
		if (name == "UnityEngine.InputSystem.Joystick")
		{
			var controller = device as UnityEngine.InputSystem.Joystick;

		}
	}
}