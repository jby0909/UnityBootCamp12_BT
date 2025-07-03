namespace NPC_Example.Behaviours.BT
{
    /// <summary>
    /// 성공한 자식을 선택해서 처리하기 위한 컴포지트
    /// 자식 하나라도 성공하면 탐색 종료 후 성공 반환.
    /// </summary>
    public class Selector : Composite
    {
        public Selector(BehaviourTree tree) : base(tree)
        {
        }

        public override Result Invoke()
        {
            Result result = Result.Failure;

            for (int i = currentChildIndex; i < children.Count; i++)
            {
                result = children[i].Invoke();

                switch (result)
                {
                    case Result.Failure:
                        {
                            currentChildIndex++;
                        }
                        break;
                    case Result.Success:
                        {
                            currentChildIndex = 0;
                            return result;
                        }
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
