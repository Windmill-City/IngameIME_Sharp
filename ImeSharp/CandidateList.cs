using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeSharp
{
    public class CandidateList
    {
        public int Count;

        public int CurrentSel;

        public List<string> Cadidates = new LinkedList();
    }
}