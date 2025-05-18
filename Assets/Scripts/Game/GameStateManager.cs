using System;
using GimGim.Player;
using UnityEngine;

namespace GimGim.Game {
    public class GameStateManager : MonoBehaviour {
        public GameState CurrentState { get; private set; }
        private PlayerInputActions playerControls;

        private bool _isInteracting = false;
        private object _interactedObject = null;

        private void Awake() {
            playerControls = new PlayerInputActions();
        }
        
        
    }
}