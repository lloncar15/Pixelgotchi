using GimGim.EventSystem;

namespace GimGim.Player {
    public class PlayerDiedEvent : EventData {
        public PlayerDiedEvent(object obj) : base(obj) {
        }
    }
}