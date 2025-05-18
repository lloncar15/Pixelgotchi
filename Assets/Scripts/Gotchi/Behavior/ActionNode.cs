using System;

namespace GimGim.Gotchi {
    public class ActionNode : Node {
        public Func<NodeState> Action;

        public ActionNode(Func<NodeState> action) {
            Action = action;
        }

        public override NodeState Evaluate() {
            State = Action();
            return State;
        }
    }
}