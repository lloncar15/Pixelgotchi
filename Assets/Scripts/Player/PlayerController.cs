using Player;
using UnityEngine;

namespace GimGim.Player {
    public class PlayerController : MonoBehaviour {
        private PlayerInputHandler _inputHandler;

        public PlayerStats stats;
        
        public bool isDead = false;
        
        private void Awake() {
            _inputHandler = GetComponent<PlayerInputHandler>();
        }
        
        private void Update() {
            HandleMovement();
        }
        
        private void HandleMovement() {
            Vector2 moveInput = _inputHandler.moveInput;
            Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
            transform.Translate(moveDirection * (Time.deltaTime * stats.moveSpeed));
        }
    }
}