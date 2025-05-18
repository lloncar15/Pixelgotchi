using UnityEngine;

namespace GimGim.Game {
    public class MenuState : GameState {
        public MenuState() {
            Type = GameStateType.Menu;
        }
        
        public override void OnUpdate() {
            
        }
        
        public override void OnEnter() {
            Time.timeScale = 0f;
        }
        
        public override void OnExit() {
            Time.timeScale = 1f;
        }
    }
}