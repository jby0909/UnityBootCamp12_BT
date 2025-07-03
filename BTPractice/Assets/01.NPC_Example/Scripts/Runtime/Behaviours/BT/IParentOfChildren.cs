using System.Collections.Generic;

namespace NPC_Example.Behaviours.BT
{
    interface IParentOfChildren : IParent
    {
        List<Node> children { get; set; }
    }
}
