namespace NPC_Example.Behaviours.BT
{
    interface IParentOfChild : IParent
    {
        Node child { get; set; }
    }
}
