using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Example.Behaviours.BT
{
    public class BehaviourTree : MonoBehaviour
    {
        public Blackboard blackboard { get; set; }
        public Root root { get; set; }
        private bool _isRunning;
        public Stack<Node> stack;


        private void Update()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            StartCoroutine(C_Tick());
        }

        IEnumerator C_Tick()
        {
            stack.Push(root);

            while(stack.Count > 0)
            {
                Node node = stack.Pop();
                Result result = node.Invoke();

                if(result == Result.Running)
                {
                    stack.Push(node);
                    yield return null;
                }
            }

            _isRunning = false;
        }

        private void OnDrawGizmos()
        {
            if (stack != null && stack.Count > 0)
                stack.Peek().OnDrawGizmos();
        }
    }
}
