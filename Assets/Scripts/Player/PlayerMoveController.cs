using System;
using Game;
using UnityEngine;

namespace Player {
    public class PlayerMoveController : MonoBehaviour {
        [SerializeField] private float moveSpeed = 5f;
        private PlayerInputHandler _input;

        private void Awake() {
            _input = GetComponent<PlayerInputHandler>();
        }

        private void Update() {
            if (GameStateManager.Instance.CurrentState is GameState.Cinematic or GameState.Menu) return;
            
            Vector2 moveDirection = new Vector2(_input.MoveInput.x, _input.MoveInput.y);
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}