using System;
using System.Reflection;

namespace NPC_Example.Behaviours.BT
{
    public class Condition : Node, IParentOfChild
    {
        public Condition(BehaviourTree tree, string propertyName) : base(tree)
        {
            _propertyInfo = tree.blackboard.GetType().GetProperty(propertyName);
        }

       
        public Node child { get; set; }

        private PropertyInfo _propertyInfo;

        public override Result Invoke()
        {
            bool cond = (bool)_propertyInfo.GetValue(blackboard);

            if(cond)
            {
                tree.stack.Push(child);
                return Result.Success;
            }

            return Result.Failure;
        }

        public void AttachChild(Node node)
        {
            child = node;
        }
    }
}
