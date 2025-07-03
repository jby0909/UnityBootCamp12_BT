using UnityEngine;

namespace NPC_Example.FSM
{
    public enum State
    {
        None,
        Move = 1,
        Attack = 10,
    }

    public class BehaviourBase : StateMachineBehaviour
    {
        //실제 문자열로 SetBool하면 모든 파라미터에 대해 문자열이 같은지 검사하므로 비용이 많이 듦
        // -> 해쉬값으로 검색하게 만듦
        readonly int IS_DIRTY_HASH = Animator.StringToHash("IsDirty");

        [SerializeField] State _state;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<MotionController>().current = _state;
            animator.SetBool(IS_DIRTY_HASH, false);
        }

    }
}

