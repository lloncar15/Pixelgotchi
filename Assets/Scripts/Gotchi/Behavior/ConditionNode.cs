using System;

namespace GimGim.Gotchi {
    public class ConditionNode : Node {
        public readonly Func<bool> Condition;

        public ConditionNode(Func<bool> condition) {
            Condition = condition;
        }

        public override NodeState Evaluate() {
            State = Condition() ? NodeState.Success : NodeState.Failure;
            return State;
        }
    }
}