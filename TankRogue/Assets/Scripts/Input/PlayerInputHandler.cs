using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private static PlayerInputHandler instance;
    public static PlayerInputHandler Instance
    {
        get
        {
            return instance;
        }
        private set{
            
        }
    }
    private PlayerInput playerInput;

    private event Action OnUpdateAction;

    void Awake()
    {
        instance = this;
        playerInput = new PlayerInput();
    }

    void Update()
    {
        OnUpdateAction?.Invoke();
    }

    #region Player Movement

    private ICharacter _playerCharacter;

    //turns movement on and off as the character gets assigner
    //if there's no character assigned to the singleton, no character moves!
    public ICharacter PlayerCharacter
    {
        get
        {
            return _playerCharacter;
        }
        set
        {
            if(value != null)
            {
                OnUpdateAction += RunCharacterMovement;
                playerInput.Movement.Enable();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                OnUpdateAction -= RunCharacterMovement;
                playerInput.Movement.Disable();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            _playerCharacter = value;
        }
    }

    private void RunCharacterMovement()
    {
        //if paused return

        //Jump
        if (playerInput.Movement.Jump.triggered)
        {
            PlayerCharacter.Jump();
        }

        //move
        Vector2 moveWASD = playerInput.Movement.Move.ReadValue<Vector2>();
        PlayerCharacter.MoveWASD(moveWASD);

        Vector2 lookInput = playerInput.Movement.MouseLook.ReadValue<Vector2>();
        PlayerCharacter.LookAround(lookInput);

    }

    #endregion Player Movement
}
