using NPC_Example.FSM;

namespace NPC_Example.Behaviours.BT
{
    public class Attack : Node
    {
        public Attack(BehaviourTree tree) : base(tree)
        {
        }

        bool _isRunning;

        public override Result Invoke()
        {
            //1. 현재 상태가 Attack 이면
            //   Running
            // 아니면
            //   Attack진입 성공한 이후라면 -> 애니메이션 종료되었으므로 success
            //   그렇지 않으면 아직 attack 전환 안함 -> 상태를 Attack으로 전환, running

            MotionController motionController = blackboard.motionController;

            if(_isRunning)
            {
                if (motionController.current == State.Move)
                {
                    _isRunning = false;
                    return Result.Success;
                }

                return Result.Running;
            }
            else
            {
                motionController.ChangeState(State.Attack);
                _isRunning = true;
                return Result.Running;
            }
        }
    }
}
