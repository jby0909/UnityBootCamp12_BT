using UnityEngine;
using UnityEngine.AI;

namespace NPC_Example.FSM
{
    public class MotionController : MonoBehaviour
    {
        readonly int MOVE_SPEED_HASH = Animator.StringToHash("MoveSpeed");
        readonly int STATE_HASH = Animator.StringToHash("State");
        readonly int IS_DIRTY_HASH = Animator.StringToHash("IsDirty");

        NavMeshAgent _agent;
        Animator _animator;
        
        public State current { get; set; }
        public State target { get; set; }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(MOVE_SPEED_HASH, _agent.velocity.magnitude / _agent.speed);
        }

        public void ChangeState(State newState)
        {
            _animator.SetInteger(STATE_HASH, (int)newState);
            _animator.SetBool(IS_DIRTY_HASH, true);
            target = newState;
        }

        private void OnAvailableNextMotions(AnimationEvent animationEvent)
        {
            if(animationEvent.objectReferenceParameter is not AvailableNextMotions)
            {
                throw new System.Exception("잘못된 데이터 설정됨");
            }

            // TODO : 타이머 및 가능한 모션 관리
        }
    }
}
