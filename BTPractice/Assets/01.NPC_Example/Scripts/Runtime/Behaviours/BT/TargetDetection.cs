using UnityEngine;

namespace NPC_Example.Behaviours.BT
{
    public class TargetDetection : Node
    {
        public TargetDetection(BehaviourTree tree,
                               float angle,
                               float radius,
                               float height,
                               float maxDistance,
                               LayerMask targetMask,
                               LayerMask obstacleMask
            ) : base(tree)
        {
            this.angle = angle;
            this.radius = radius;
            this.height = height;
            this.maxDistance = maxDistance;
            this.targetMask = targetMask;
            this.obstacleMask = obstacleMask;
        }

        protected float angle;
        protected float radius;
        protected float height;
        protected float maxDistance;
        protected LayerMask targetMask;
        protected LayerMask obstacleMask;

        public override Result Invoke()
        {
            if(TryDetectTarget(out Transform target))
            {
                return Result.Success;
            }

            return Result.Failure;
        }

        protected bool TryDetectTarget(out Transform target)
        {
            bool isDetected = false;
            Transform closest = null;
            Collider[] cols =
            Physics.OverlapCapsule(blackboard.transform.position,
                                   blackboard.transform.position + Vector3.up * height,
                                   radius,
                                   targetMask);
            if(cols.Length > 0)
            {
                float minDistance = 0;
               
                for(int i = 0; i < cols.Length; i++)
                {
                    if (IsInsight(cols[i].transform))
                    {
                        float distance = Vector3.Distance(cols[i].transform.position, blackboard.transform.position);

                        if(closest)
                        {
                            if(distance < minDistance)
                            {
                                minDistance = distance;
                                closest = cols[i].transform;
                            }
                        }
                        else
                        {
                            minDistance = distance;
                            closest = cols[i].transform;
                            isDetected = true;
                        }
                    }
                }
            }

            target = closest;
            blackboard.target = target;
            return isDetected;
        }

        bool IsInsight(Transform target)
        {
            Vector3 origin = blackboard.transform.position;
            Vector3 forward = blackboard.transform.forward;
            Vector3 lookDir = (target.position - origin).normalized;
            float theta = Mathf.Acos(Vector3.Dot(forward, lookDir)) * Mathf.Rad2Deg;

            //시야각 내
            if(theta <= angle / 2f)
            {
                // 사이에 장애물 있는지
                if(Physics.Raycast(origin + Vector3.up * height, lookDir, out RaycastHit hit, Vector3.Distance(target.position, origin),obstacleMask))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
