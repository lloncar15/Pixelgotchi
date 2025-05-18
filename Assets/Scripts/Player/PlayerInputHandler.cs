using System;
using GimGim.EventSystem;
using GimGim.Game;
using GimGim.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerInputHandler : MonoBehaviour {
        private PlayerInputActions _playerControls;

        public Vector2 moveInput;
        
        private IEventSubscription _subscription;
        
        private void Awake() {
            _playerControls = new PlayerInputActions();
        }

        private void OnEnable() {
            _playerControls.Player.Enable();
            
            _playerControls.Player.Move.performed += OnMovePerformed;
            _playerControls.Player.Move.canceled += OnMoveCanceled;

            _playerControls.Player.Attack.performed += OnAttackPerformed;
            _playerControls.Player.Interact.performed += OnInteractPerformed;
            _playerControls.Player.CancelInteraction.performed += OnCancelInteractionPerformed;
            _playerControls.Player.Choice1.performed += OnChoice1InteractionPerformed;
            _playerControls.Player.Choice2.performed += OnChoice2InteractionPerformed;
            _playerControls.Player.Pause.performed += OnPausePerformed;

            _subscription =
                NotificationEventSystem.Subscribe(new EventSubscription<OnStateChangedEvent>(OnStateChanged));
        }
        
        private void OnDisable() {
            _playerControls.Player.Disable();
            
            _playerControls.Player.Move.performed -= OnMovePerformed;
            _playerControls.Player.Move.canceled -= OnMoveCanceled;
            
            _playerControls.Player.Attack.performed -= OnAttackPerformed;
            _playerControls.Player.Interact.performed -= OnInteractPerformed;
            _playerControls.Player.CancelInteraction.performed -= OnCancelInteractionPerformed;
            _playerControls.Player.Choice1.performed -= OnChoice1InteractionPerformed;
            _playerControls.Player.Choice2.performed -= OnChoice2InteractionPerformed;
            _playerControls.Player.Pause.performed -= OnPausePerformed;
            
            NotificationEventSystem.Unsubscribe(_subscription);
        }
        
        private void Update() {
#if UNITY_EDITOR
            if (Keyboard.current.digit3Key.wasPressedThisFrame) GameStateManager.Instance.SetPeaceState();
            if (Keyboard.current.digit4Key.wasPressedThisFrame) GameStateManager.Instance.SetFightState();
            if (Keyboard.current.digit5Key.wasPressedThisFrame) GameStateManager.Instance.SetMenuState();
            if (Keyboard.current.digit6Key.wasPressedThisFrame) GameStateManager.Instance.SetCinematicState();
#endif
        }
        
        private void OnMovePerformed(InputAction.CallbackContext obj) {
            moveInput = obj.ReadValue<Vector2>();
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext obj) {
            moveInput = Vector2.zero;
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

        private void OnStateChanged(OnStateChangedEvent eventData) {
            GameStateType type = eventData.State.Type;
            
            if (type == GameStateType.Cinematic || type == GameStateType.Menu) {
                _playerControls.Player.Disable();
            } else {
                _playerControls.Player.Enable();
            }
        }
    }
}