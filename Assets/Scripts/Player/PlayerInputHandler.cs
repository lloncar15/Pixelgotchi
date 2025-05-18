using System;
using GimGim.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerInputHandler : MonoBehaviour {
        private PlayerInputActions playerControls;

        public Vector2 MoveInput;
        
        private void Awake() {
            playerControls = new PlayerInputActions();
        }

        private void OnEnable() {
            playerControls.Player.Enable();
            
            playerControls.Player.Move.performed += OnMovePerformed;
            playerControls.Player.Move.canceled += OnMoveCanceled;

            playerControls.Player.Attack.performed += OnAttackPerformed;
            playerControls.Player.Interact.performed += OnInteractPerformed;
            playerControls.Player.CancelInteraction.performed += OnCancelInteractionPerformed;
            playerControls.Player.Choice1.performed += OnChoice1InteractionPerformed;
            playerControls.Player.Choice2.performed += OnChoice2InteractionPerformed;
            playerControls.Player.Pause.performed += OnPausePerformed;
        }
        
        private void OnDisable() {
            playerControls.Player.Disable();
            
            playerControls.Player.Move.performed -= OnMovePerformed;
            playerControls.Player.Move.canceled -= OnMoveCanceled;
            
            playerControls.Player.Attack.performed -= OnAttackPerformed;
            playerControls.Player.Interact.performed -= OnInteractPerformed;
            playerControls.Player.CancelInteraction.performed -= OnCancelInteractionPerformed;
            playerControls.Player.Choice1.performed -= OnChoice1InteractionPerformed;
            playerControls.Player.Choice2.performed -= OnChoice2InteractionPerformed;
            playerControls.Player.Pause.performed -= OnPausePerformed;
        }
        
        private void OnMovePerformed(InputAction.CallbackContext obj) {
            MoveInput = obj.ReadValue<Vector2>();
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext obj) {
            MoveInput = Vector2.zero;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj) {
            
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext obj) {
            
        }
        
        private void OnCancelInteractionPerformed(InputAction.CallbackContext obj) {
            
        }
        
        private void OnChoice1InteractionPerformed(InputAction.CallbackContext obj) {
            
        }
        
        private void OnChoice2InteractionPerformed(InputAction.CallbackContext obj) {
            
        }
        
        private void OnPausePerformed(InputAction.CallbackContext obj) {
            
        }
    }
}