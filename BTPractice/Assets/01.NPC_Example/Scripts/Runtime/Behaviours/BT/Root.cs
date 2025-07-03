using NPC_Example.Behaviours.BT;

namespace NPC_Example.Behaviours.BT
{
    public class Root : Node, IParentOfChild
    {
        public Root(BehaviourTree tree) : base(tree)
        {
        }

        public Node child { get; set; }

        public void AttachChild(Node node)
        {
            child = node;
        }

        public override Result Invoke()
        {
            tree.stack.Push(child);
            return child.Invoke();
        }
    }
}
