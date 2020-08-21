using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static ImeSharp.IMM32_IMEControl;

namespace ImeSharp
{
    public class CandidateList
    {
        public uint Count;

        public uint CurSel;

        public uint PageSize;

        public uint CurPage;

        public List<string> Candidates = new List<string>();

        public void Apply(byte[] pCandidate)
        {
            Reset();
            unsafe
            {
                fixed (byte* p = pCandidate)
                {
                    CANDIDATELIST* cl = (CANDIDATELIST*)p;
                    CurSel = cl->dwSelection;
                    CurPage = cl->dwPageStart;
                    PageSize = cl->dwPageSize;
                    Count = cl->dwCount;

                    int offset = (int)cl->dwOffset[0];
                    int len = pCandidate.Length - offset;
                    var str = Encoding.Unicode.GetString(pCandidate, offset, len);
                    str = Regex.Replace(str, @"\0+$", "");
                    var par = "\0".ToCharArray();
                    foreach (var item in str.Split(par))
                    {
                        Candidates.Add(item);
                    }
                }
            }
        }

        public void Reset()
        {
            Count = 0;
            CurSel = 0;
            PageSize = 0;
            CurPage = 0;
            Candidates.Clear();
        }
    }
}