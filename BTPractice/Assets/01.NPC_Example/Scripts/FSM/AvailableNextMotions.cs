using UnityEngine;

namespace NPC_Example.FSM
{
    public class AvailableNextMotions : ScriptableObject
    {
        [field: SerializeField] public State[] availables { get; private set; }
    }
}

