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
        public int Count;

        public int CurSel;

        public int PageSize;

        public int CurPage;

        public List<string> Candidates = new List<string>();

        public void Apply(byte[] pCandidate)
        {
            Reset();
            unsafe
            {
                fixed (byte* p = pCandidate)
                {
                    CANDIDATELIST* cl = (CANDIDATELIST*)p;
                    CurSel = (int)cl->dwSelection;
                    CurPage = (int)cl->dwPageStart;
                    PageSize = (int)cl->dwPageSize;
                    Count = (int)cl->dwCount;

                    for (int i = CurPage; i < PageSize + CurPage; i++)
                    {
                        int offset = (int)cl->dwOffset[i];
                        var ptrSrc = (IntPtr)(p + offset);

                        Candidates.Add(Marshal.PtrToStringAuto(ptrSrc));
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