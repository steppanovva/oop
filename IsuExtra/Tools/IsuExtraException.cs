namespace IsuExtra.Tools
{
    public class IsuExtraException : Isu.Tools.IsuException
    {
        public IsuExtraException()
        { }

        public IsuExtraException(string message)
            : base(message) { }
    }
}