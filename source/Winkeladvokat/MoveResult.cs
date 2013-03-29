namespace Winkeladvokat
{
    public class MoveResult
    {
        public MoveResult(string message)
        {
            this.IsValid = string.IsNullOrEmpty(message);
            this.Message = message;
        }

        public bool IsValid { get; private set; }

        public string Message { get; private set; }

        public static MoveResult CreateValidResult()
        {
            return new MoveResult(string.Empty);
        }

        public static MoveResult CreateInvalidResult(string message)
        {
            return new MoveResult(message);
        }
    }
}