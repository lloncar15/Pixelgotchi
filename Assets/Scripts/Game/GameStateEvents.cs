using GimGim.EventSystem;

namespace GimGim.Game {
    public class OnStateChangedEvent : EventData {
        public GameState State { get; }
        
        public OnStateChangedEvent(object obj, GameState state) : base(obj) {
            State = state;
        }
    }
}