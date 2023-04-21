using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool isAiming;
		public bool ability;
		public bool pause;
		public InputAction SwitchFireMode;
		public InputAction Shoot;
		public InputAction PrimaryWeapon;
		public InputAction SecondaryWeapon;
		private PlayerInput _playerInput;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private void Start()
		{
			_playerInput = GetComponent<PlayerInput>();
			if(_playerInput != null)
			{
				SwitchFireMode = _playerInput.actions["SwitchFireMode"];
				Shoot = _playerInput.actions["Shoot"];
				PrimaryWeapon = _playerInput.actions["PrimaryWeapon"];
				SecondaryWeapon = _playerInput.actions["SecondaryWeapon"];
			}
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			isAiming = value.isPressed;
		}

		public void OnAbility(InputValue value)
		{
			AbilityInput(value.isPressed);
		}

		public void OnPause(InputValue inputValue)
		{
			pause = inputValue.isPressed;
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AbilityInput( bool newAbilityState)
		{
			ability = newAbilityState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}