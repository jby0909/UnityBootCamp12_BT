using UnityEngine;

namespace NPC_Example.Behaviours.BT
{
    public class Monitor : TargetDetection
    {
        public Monitor(BehaviourTree tree,
                       float angle,
                       float radius,
                       float height,
                       float maxDistance,
                       LayerMask targetMask,
                       LayerMask obstacleMask,
                       float monitoringMinTime,
                       float monitoringMaxTime)
            : base(tree, angle, radius, height, maxDistance, targetMask, obstacleMask)
        {
            _monitoringMinTime = monitoringMinTime;
            _monitoringMaxTime = monitoringMaxTime;
        }

        private float _monitoringMinTime;
        private float _monitoringMaxTime;
        private float _monitoringTime; // 이번 감시에서 감시할 시간 (랜덤하게 뽑음)
        private float _monitoringTimeMark; // 감시시작 시간
        private bool _isMonitoring;

        public override Result Invoke()
        {
            // 1. 타겟이 있으면 감시 성공
            // 2. 감시중일 때
            // 시간 초과하면 -> 감시 실패
            // 시간 경과중 -> 감시 중
            // 3. 감시 안 하고 있을 때
            // 타겟 감지 되면 감시 성공
            // 타겟 감지 안 되면 감시 시작

            if (blackboard.target)
            {
                _isMonitoring = false;
                return Result.Success;
            }

            if (_isMonitoring)
            {
                float elapsedTime = Time.time - _monitoringTimeMark;

                if (elapsedTime > _monitoringTime)
                {
                    _isMonitoring = false;
                    return Result.Failure;
                }
                else
                {
                    if (TryDetectTarget(out Transform target))
                    {
                        blackboard.target = target;
                        _isMonitoring = false;
                        return Result.Success;
                    }
                    else
                    {
                        return Result.Running;
                    }
                }
            }
            else
            {
                if (TryDetectTarget(out Transform target))
                {
                    blackboard.target = target;
                    _isMonitoring = false;
                    return Result.Success;
                }
                else
                {
                    _monitoringTime = Random.Range(_monitoringMinTime, _monitoringMaxTime);
                    _monitoringTimeMark = Time.time;
                    _isMonitoring = true;
                    return Result.Running;
                }
            }
        }
    }
}