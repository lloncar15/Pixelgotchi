using System;
using GimGim.EventSystem;
using GimGim.Player;
using GimGim.Utility.Logger;
using Player;
using UnityEngine;

namespace GimGim.Game {
    public enum GameStateType {
        Peace,
        Fight,
        Menu,
        Cinematic
    }
    
    public class GameStateManager : MonoBehaviour {
        private GameLogger _logger;
        
        public static GameStateManager Instance { get; private set; }
        
        public GameState CurrentState { get; private set; }

        private PeaceState _peace;
        private FightState _fight;
        private MenuState _menu;
        private CinematicState _cinematic;
        
        private void Awake() {
            if (!Instance) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }

            _logger = GameLogger.Create<GameStateManager>(Color.yellow);
            
            _peace = new PeaceState();
            _fight = new FightState();
            _menu = new MenuState();
            _cinematic = new CinematicState();
        }

        private void Start() {
            CurrentState = _peace;
            _peace.OnEnter();
        }

        private void Update() {
            
        }
        
        public void ChangeState(GameState newState) {
            if (CurrentState.Type == newState.Type) return;
            
            _logger.LogInfo("Changing state from {0} to {1}", CurrentState.Type, newState.Type);
            
            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
            
            NotificationEventSystem.PostEvent(new OnStateChangedEvent(this, CurrentState));
        }

        public void SetPeaceState() => Instance.ChangeState(_peace);
        public void SetFightState() => Instance.ChangeState(_fight);
        public void SetMenuState() => Instance.ChangeState(_menu);
        public void SetCinematicState() => Instance.ChangeState(_cinematic);
        
        public static GameStateType GetGameStateType() => Instance.CurrentState.Type;
    }
}