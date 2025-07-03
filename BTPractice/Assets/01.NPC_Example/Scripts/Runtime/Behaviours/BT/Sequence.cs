namespace NPC_Example.Behaviours.BT
{
    /// <summary>
    /// 자식들을 일련의 과정으로 처리하기 위한 컴포지트.
    /// 자식 모두 성공해야 성공 반환.
    /// </summary>
    public class Sequence : Composite
    {
        public Sequence(BehaviourTree tree) : base(tree) 
        {
        }

        public override Result Invoke()
        {
            Result result = Result.Success;

            for(int i = currentChildIndex; i < children.Count; i++)
            {
                result = children[i].Invoke();

                switch(result)
                {
                    case Result.Failure:
                        {
                            currentChildIndex = 0;
                            return result;
                        }
                    case Result.Success:
                        {
                            currentChildIndex++;
                        }
                        break;
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        throw new System.Exception("Invalid result code" + result);
                }
            }

            currentChildIndex = 0;
            return result;
        }
    }
}
