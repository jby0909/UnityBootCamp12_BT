namespace NPC_Example.Behaviours.BT
{
    /// <summary>
    /// 자식들을 모두 수행하고,
    /// 정책에 따라 결과를 반환
    /// </summary>
    public class Parallel : Composite
    {
        public Parallel(BehaviourTree tree, int successCountRequired) : base(tree) 
        {
            _successCountRequired = successCountRequired;
        }

        private int _successCountRequired;
        private int _successCount;

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
                        }
                        break;
                    case Result.Success:
                        {
                            _successCount++;
                        }
                        break;
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        throw new System.Exception("Invalid result code" + result);
                }

                currentChildIndex++;
            }

            currentChildIndex = 0;
            result = _successCount >= _successCountRequired ? Result.Success : Result.Failure;
            _successCount = 0;
            return result;
        }
    }
}
