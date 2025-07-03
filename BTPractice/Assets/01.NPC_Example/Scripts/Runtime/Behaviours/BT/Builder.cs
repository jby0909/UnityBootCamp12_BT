using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Example.Behaviours.BT
{
    /// <summary>
    /// Builder 체이닝 메서드 자동 생성 대상에서 제외할 때 부착
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ExcludeFromBuilderAttribute : Attribute { }

    public partial class Builder
    {
        public Builder(BehaviourTree tree)
        {
            _tree = tree;
        }

        BehaviourTree _tree;
        Node _current;
        Stack<Composite> _compositeStack;

        public Builder Root()
        {
            _compositeStack = new Stack<Composite>();
            Blackboard blackboard = new Blackboard(_tree.gameObject);
            _tree.blackboard = blackboard;
            _tree.stack = new Stack<Node>();
            _current = _tree.root = new Root(_tree);
            return this;
        }

        public Builder CompleteCurrentComposite()
        {
            if (_compositeStack.Count > 0)
                _compositeStack.Pop();
            else
                throw new System.Exception("완성할 컴포지트가 없어요...");

            if (_compositeStack.Count > 0)
                _current = _compositeStack.Peek();

            return this;
        }

        private void Attach(Node parent, Node child)
        {
            if (parent is IParent)
                ((IParent)parent).AttachChild(child);
            else
                throw new System.Exception($"{parent.GetType().Name} 은 자식을 가질 수 없습니다.");

            if(child is IParent)
            {
                if (child is Composite)
                    _compositeStack.Push((Composite)child);

                _current = child;
            }
            else
            {
                if (_compositeStack.Count > 0)
                    _current = _compositeStack.Peek();
                else
                    _current = null;
            }
        }

    }
}
