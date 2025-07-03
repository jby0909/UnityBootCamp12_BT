using System.Collections.Generic;

namespace NPC_Example.Behaviours.BT
{
    public abstract class Composite : Node, IParentOfChildren
    {
        protected Composite(BehaviourTree tree) : base(tree)
        {
            children = new List<Node>();
        }

        public List<Node> children { get; set; }

        protected int currentChildIndex;

        public void AttachChild(Node node)
        {
            children.Add(node);
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            children[currentChildIndex].OnDrawGizmos();
        }
    }
}
