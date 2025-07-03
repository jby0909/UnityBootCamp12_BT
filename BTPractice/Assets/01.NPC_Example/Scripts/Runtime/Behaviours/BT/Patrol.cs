using UnityEngine;
using UnityEngine.AI;

namespace NPC_Example.Behaviours.BT
{
    public class Patrol : TargetDetection
    {
        public Patrol(BehaviourTree tree,
                      float angle,
                      float radius,
                      float height,
                      float maxDistance,
                      LayerMask targetMask,
                      LayerMask obstacleMask,
                      float patrolRadius,
                      LayerMask groundMask)
            : base(tree, angle, radius, height, maxDistance, targetMask, obstacleMask)
        {
            _patrolRadius = patrolRadius;
            _groundMask = groundMask;
            _positionSpawned = blackboard.transform.position;
        }


        float _patrolRadius;
        LayerMask _groundMask;
        Vector3 _positionSpawned;
        bool _isMoving;

        public override Result Invoke()
        {
            // 1. 타겟이 있으면 정찰 성공
            // 2. 에이전트가 목적지 도착시
            //  타겟 감지
            //      있으면 정찰 성공,
            //      없으면 정찰 실패
            // 3. 에이전트가 목적지 이동중
            //  타겟 감지
            //      있으면 정찰 성공,
            //      없으면 정찰 중

            if (blackboard.target)
            {
                _isMoving = false;
                blackboard.agent.ResetPath();
                return Result.Success;
            }

            if (blackboard.agent.hasPath == false ||
                blackboard.agent.remainingDistance <= blackboard.agent.stoppingDistance)
            {
                // 이전 틱에서 움직이다가 멈췄다 -> 정찰중 타겟 발견 못함
                if (_isMoving)
                {
                    _isMoving = false;
                    return Result.Failure;
                }
                // 이번 틱에서 정찰 시작
                else
                {
                    if (TryMoveToRandomDestination())
                    {
                        _isMoving = true;
                        return Result.Running;
                    }
                }
            }
            else
            {
                // 정찰 중 타겟 발견
                if (TryDetectTarget(out Transform target))
                {
                    blackboard.target = target;
                    blackboard.agent.ResetPath();
                    _isMoving = false;
                    return Result.Success;
                }
                else
                {
                    return Result.Running;
                }
            }

            _isMoving = false;
            return Result.Failure;
        }

        bool TryMoveToRandomDestination()
        {
            int tryCount = 3;

            while (tryCount-- > 0)
            {
                Vector2 xz = Random.insideUnitCircle * _patrolRadius;
                Vector3 destination = new Vector3(xz.x + _positionSpawned.x,
                                                  blackboard.transform.position.y,
                                                  xz.y + _positionSpawned.z);
                Ray ray = new Ray(destination + Vector3.up * 10f, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask))
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 0.5f, NavMesh.AllAreas))
                    {
                        blackboard.agent.SetDestination(destination);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}