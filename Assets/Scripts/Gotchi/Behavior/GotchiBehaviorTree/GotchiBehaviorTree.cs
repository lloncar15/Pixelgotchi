namespace GimGim.Behavior {
    public class GotchiBehaviorTree : Tree {
        protected override Node SetupTree() {
            Node root = new Selector();

            return root;
        }
    }
}