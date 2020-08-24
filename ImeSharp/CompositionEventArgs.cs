namespace ImeSharp
{
    public class CompositionEventArgs
    {
        public CompositionState state;
        public int caretPos;
        public string strComp;
        public string strCommit;

        public CompositionEventArgs()
        {
        }

        public CompositionEventArgs(string compStr, int caretPos)
        {
            state = CompositionState.Composing;
            this.strComp = compStr;
            this.caretPos = caretPos;
        }

        public CompositionEventArgs(string commitStr)
        {
            state = CompositionState.Commit;
            this.strCommit = commitStr;
        }

        public CompositionEventArgs(CompositionState state)
        {
            this.state = state;
        }
    }

    public enum CompositionState
    {
        StartComposition,
        Composing,
        Commit,
        EndComposition
    }
}