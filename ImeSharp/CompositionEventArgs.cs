using System.Runtime.InteropServices;

namespace ImeSharp
{
    public class CompositionEventArgs
    {
        public CompositionState state;
        public int caretPos;
        public string compStr;
        public string commitStr;

        public CompositionEventArgs(string compStr, int caretPos)
        {
            state = CompositionState.Composing;
            this.compStr = compStr;
            this.caretPos = caretPos;
        }

        public CompositionEventArgs(string commitStr)
        {
            state = CompositionState.Commit;
            this.commitStr = commitStr;
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