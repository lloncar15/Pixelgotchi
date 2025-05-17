using System;
using UnityEngine;

namespace Game {
    public class GameStateManager : MonoBehaviour {
        public static GameStateManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.Peace;
        public event Action<GameState> OnGameStateChanged;

        public void Awake() {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetState(GameState state) {
            if (state == CurrentState) return;
            CurrentState = state;
            OnGameStateChanged?.Invoke(CurrentState);
        }
    }
}