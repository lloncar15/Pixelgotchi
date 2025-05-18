using System.Collections.Generic;

namespace GimGim.Behavior {
    public enum NodeState {
        Running,
        Success,
        Failure
    }
    
    public abstract class Node {
        protected NodeState State;
        public Node Parent;
        protected List<Node> Children = new List<Node>();
        
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        
        public Node() {
            Parent = null;
        }
        
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        private void Attach(Node child)
        {
            child.Parent = this;
            Children.Add(child);
        }
        
        public virtual NodeState Evaluate() => NodeState.Failure;

        public void SetData(string key, object value) {
            _dataContext[key] = value;
        }

        public object GetData(string key) {
            if (_dataContext.TryGetValue(key, out object value)) return value;

            Node node = Parent;
            while (node != null) {
                value = node.GetData(key);
                if (value != null) return value;
                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key) {
            if (_dataContext.Remove(key)) {
                return true;
            }
            
            Node node = Parent;
            while (node != null) {
                bool cleared = node.ClearData(key);
                if (cleared) {
                    return true;
                }

                node = node.Parent;
            }

            return false;
        }
    }
}