using UnityEngine;

namespace NPC_Example.Behaviours.BT
{
    public class Seek : TargetDetection
    {
        public Seek(BehaviourTree tree, float angle, float radius, float height, float maxDistance, LayerMask targetMask, LayerMask obstacleMask) : base(tree, angle, radius, height, maxDistance, targetMask, obstacleMask)
        {
        }

        public override Result Invoke()
        {
            //1. 타겟이 있는지 확인
            //  ㄴ 있으면 
            //      ㄴ타겟에 도착했나? -> success
            //      ㄴ터갯울 쫓는 중인가? -> running
            //      ㄴ타겟을 놓쳤나? -> failure
            //  ㄴ 없으면
            //      ㄴ타겟 감지 성공 ? -> running
            if(blackboard.target)
            {
                float distance = Vector3.Distance(blackboard.transform.position, blackboard.target.position);

                //타겟에 도착
                if(distance <= blackboard.agent.stoppingDistance)
                {
                    blackboard.agent.ResetPath();
                    return Result.Success;
                }
                //타겟 쫓는 중
                else if(distance < maxDistance)
                {
                    blackboard.agent.SetDestination(blackboard.target.position);
                    return Result.Running;
                }
                // 타겟이 추적 범위를 벗어남
                else
                {
                    blackboard.target = null;
                    blackboard.agent.ResetPath();
                    return Result.Failure;
                }
            }
            else
            {
                if(TryDetectTarget(out Transform target))
                {
                    return Result.Running;
                }
            }

            return Result.Failure;
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Vector3 origin = blackboard.transform.position;
            Vector3 center = origin + Vector3.up * height / 2.0f; // 중심점 위치 (y 축 보정 포함)
            Vector3 forward = blackboard.transform.forward;

            Gizmos.color = blackboard.target ? new Color(1, 0, 0, 0.5f) : new Color(0, 1, 0, 0.5f);

            // Draw the arc (sector range)
            int segments = 50; // 부채꼴 세그먼트 개수
            float step = angle / segments; // 각도 간격
            Vector3 lastPoint = center + Quaternion.Euler(0, -angle / 2, 0) * forward * radius; // 초기 점

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -angle / 2 + step * i;
                Vector3 nextPoint = center + Quaternion.Euler(0, currentAngle, 0) * forward * radius;

                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }

            // Draw lines to the edges of the arc
            Vector3 leftEdge = center + Quaternion.Euler(0, -angle / 2, 0) * forward * radius;
            Vector3 rightEdge = center + Quaternion.Euler(0, angle / 2, 0) * forward * radius;
            Gizmos.DrawLine(center, leftEdge);
            Gizmos.DrawLine(center, rightEdge);
        }
    }
}
