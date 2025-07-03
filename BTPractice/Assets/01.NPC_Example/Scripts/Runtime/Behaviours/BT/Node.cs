using NPC_Example.Behaviours.BT;

namespace NPC_Example.Behaviours.BT
{
    public abstract class Node
    {
        public Node(BehaviourTree tree)
        {
            this.tree = tree;
            this.blackboard = tree.blackboard;
        }

        protected BehaviourTree tree;
        protected Blackboard blackboard;

        public abstract Result Invoke();

        public virtual void OnDrawGizmos() { }
    }
}
