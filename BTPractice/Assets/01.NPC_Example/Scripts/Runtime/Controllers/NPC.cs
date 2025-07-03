using NPC_Example.Behaviours.BT;
using UnityEngine;
using UnityEngine.AI;

namespace NPC_Example.Controllers
{
    [RequireComponent(typeof(NavMeshAgent), typeof(BehaviourTree))]
    public class NPC : MonoBehaviour
    {
        private BehaviourTree _tree;

        [Header("Seek")]
        [SerializeField] float _seekAngle = 90f;
        [SerializeField] float _seekRadius = 5f;
        [SerializeField] float _seekHeight = 1f;
        [SerializeField] float _seekMaxDistance = 8f;
        [SerializeField] LayerMask _seekTargetMask;
        [SerializeField] LayerMask _seekObstacleMask;

        [Header("Patrol")]
        [SerializeField] float _patrolRadius = 10f;
        [SerializeField] LayerMask _groundMask;

        [Header("Monitor")]
        [SerializeField] float _monitorMinTime;
        [SerializeField] float _monitorMaxTime;

        private void Start()
        {
            _tree = GetComponent<BehaviourTree>();
            Builder builder = new Builder(_tree);
            builder.Root()
                .Selector()
                    .Sequence()
                        .Seek(_seekAngle, _seekRadius, _seekHeight, _seekMaxDistance, _seekTargetMask, _seekObstacleMask)
                     .CompleteCurrentComposite()
                     .RandomSelector()
                        .Patrol(_seekAngle, _seekRadius, _seekHeight, _seekMaxDistance, _seekTargetMask, _seekObstacleMask, _patrolRadius, _groundMask)
                        .Monitor(_seekAngle, _seekRadius, _seekHeight, _seekMaxDistance, _seekTargetMask, _seekObstacleMask, _monitorMinTime, _monitorMaxTime)
                        .CompleteCurrentComposite()
                    .CompleteCurrentComposite();


            //_tree = GetComponent<BehaviourTree>();
            //Selector selector1 = new Selector(_tree);
            //_tree.root.AttachChild(selector1);

            //Sequence sequence1 = new Sequence(_tree);
            //_tree.root.AttachChild(sequence1);

            //sequence1.AttachChild(new Seek(_tree,
            //                               _seekAngle,
            //                               _seekRadius,
            //                               _seekHeight,
            //                               _seekMaxDistance,
            //                               _seekTargetMask,
            //                               _seekObstacleMask));

            //selector1.AttachChild(sequence1);

            //RandomSelector randomSelector1 = new RandomSelector(_tree);
            //Patrol patrol1 = new Patrol(_tree,
            //                               _seekAngle,
            //                               _seekRadius,
            //                               _seekHeight,
            //                               _seekMaxDistance,
            //                               _seekTargetMask,
            //                               _seekObstacleMask,
            //                               _patrolRadius,
            //                               _groundMask);
            //randomSelector1.AttachChild(patrol1);

            //Monitor monitor1 = new Monitor(_tree,
            //                               _seekAngle,
            //                               _seekRadius,
            //                               _seekHeight,
            //                               _seekMaxDistance,
            //                               _seekTargetMask,
            //                               _seekObstacleMask,
            //                               _monitorMinTime,
            //                               _monitorMaxTime);
            //randomSelector1.AttachChild(monitor1);

            //selector1.AttachChild(randomSelector1);
        }
    }
}
