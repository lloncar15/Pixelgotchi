using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerInputHandler : MonoBehaviour {
        public Vector2 MoveInput { get; private set; }
        public bool InteractPressed { get; private set; }
        public bool AttackPressed { get; private set; }
        public bool Choice1Pressed { get; private set; }
        public bool Choice2Pressed { get; private set; }
        public bool CancelPressed { get; private set; }
        public bool PausePressed { get; private set; }
        
        private PlayerInput _playerInput;
        private InputActionMap _playerInputMap;

        private void Awake() {
            _playerInput = GetComponent<PlayerInput>();
            _playerInputMap = _playerInput.currentActionMap;
            _playerInputMap["Move"].performed += OnMove;
            _playerInputMap["Interact"].performed += OnInteract;
            _playerInputMap["Attack"].performed += OnAttack;
            _playerInputMap["Choice1"].performed += OnChoice1;
            _playerInputMap["Choice2"].performed += OnChoice2;
            _playerInputMap["CancelInteraction"].performed += OnCancel;
            _playerInputMap["Pause"].performed += OnPause;
        }

        private void OnEnable() {
            _playerInputMap.Enable();
        }
        
        private void OnDisable() {
            _playerInputMap["Move"].performed -= OnMove;
            _playerInputMap["Interact"].performed -= OnInteract;
            _playerInputMap["Attack"].performed -= OnAttack;
            _playerInputMap["Choice1"].performed -= OnChoice1;
            _playerInputMap["Choice2"].performed -= OnChoice2;
            _playerInputMap["CancelInteraction"].performed -= OnCancel;
            _playerInputMap["Pause"].performed += OnPause;
            _playerInputMap.Disable();
        }

        public void OnMove(InputAction.CallbackContext context) {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context) {
            InteractPressed = context.performed;
        }

        public void OnAttack(InputAction.CallbackContext context) {
            AttackPressed = context.performed;
        }

        public void OnChoice1(InputAction.CallbackContext context) {
            Choice1Pressed = context.performed;
        }

        public void OnChoice2(InputAction.CallbackContext context) {
            Choice2Pressed = context.performed;
        }

        public void OnCancel(InputAction.CallbackContext context) {
            CancelPressed = context.performed;
        }

        public void OnPause(InputAction.CallbackContext context) {
            PausePressed = context.performed;
        }
    }
}