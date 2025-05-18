namespace GimGim.Game {
    public abstract class GameState {
        public GameStateType Type;
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
    }
}