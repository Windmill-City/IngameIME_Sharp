using ImeSharp.Native;
using msctfLib;
using System;

namespace ImeSharp
{
    public class TF_TextEditSink : ITfTextEditSink
    {
        public static readonly GUID IID_ITfTextEditSink = new GUID(0x8127d409, 0xccd3, 0x4683, 0x96, 0x7a, 0xb4, 0x3d, 0x5b, 0x48, 0x2b, 0xf7);
        public static readonly GUID GUID_PROP_COMPOSING = new GUID(0xe12ac060, 0xaf15, 0x11d2, 0xaf, 0xc5, 0x00, 0x10, 0x5a, 0x27, 0x99, 0xb5);

        private ITfContext m_ctx;
        private TF_TextStore m_textStore;
        private uint m_cookie;

        public TF_TextEditSink(ITfContext ctx, TF_TextStore textStore)
        {
            m_ctx = ctx;
            m_textStore = textStore;
            ITfSource source = m_ctx as ITfSource;
            source.AdviseSink(IID_ITfTextEditSink, this, out m_cookie);
        }

        ~TF_TextEditSink()
        {
            if (m_cookie != msctf.TF_INVALID_COOKIE)
            {
                ITfSource source = m_ctx as ITfSource;
                source.UnadviseSink(m_cookie);
            }
        }

        public void OnEndEdit(ITfContext pic, uint ecReadOnly, ITfEditRecord pEditRecord)
        {
            m_textStore.m_fhasEdited = true;
            m_textStore.m_Composing = false;
            m_textStore.m_Commit = false;
            m_textStore.m_CompStart = m_textStore.m_CompEnd = 0;

            GUID[] rgGuids = new GUID[] { GUID_PROP_COMPOSING };

            ITfReadOnlyProperty TrackProperty;
            ITfRange Start2EndRange;
            ITfRange EndRange;

            pic.TrackProperties(rgGuids, 2, (IntPtr)0, 0, out TrackProperty);

            pic.GetStart(ecReadOnly, out Start2EndRange);

            pic.GetEnd(ecReadOnly, out EndRange);
            Start2EndRange.ShiftEndToRange(ecReadOnly, EndRange, TfAnchor.TF_ANCHOR_END);

            IEnumTfRanges Ranges;
            TrackProperty.EnumRanges(ecReadOnly, out Ranges, Start2EndRange);

            while (true)
            {
                ITfRange Range;
                uint cFetched;

                Ranges.Next(1, out Range, out cFetched);
                if (cFetched == 0)
                    break;
                object var;
                Ole.VariantInit(out var);
                IEnumTfPropertyValue EnumPropValue;

                TrackProperty.GetValue(ecReadOnly, Range, out var);

                EnumPropValue = var as IEnumTfPropertyValue;

                Ole.VariantClear(out var);

                TF_PROPERTYVAL PropValue;
                bool IsComposing = false;

                EnumPropValue.Next(1, out PropValue, out cFetched);
                while (cFetched != 0)
                {
                    if (PropValue.guidId == GUID_PROP_COMPOSING)
                    {
                        IsComposing = (bool)PropValue.varValue == true;
                        break;
                    }
                    Ole.VariantClear(out PropValue.varValue);
                }

                ITfRangeACP RangeACP;
                RangeACP = Range as ITfRangeACP;
                int AcpStart, Len;
                RangeACP.GetExtent(out AcpStart, out Len);

                if (IsComposing)
                {
                    if (!m_textStore.m_Composing)
                    {
                        m_textStore.m_Composing = true;
                        m_textStore.m_CompStart = m_textStore.m_CompEnd = AcpStart;
                    }
                    m_textStore.m_CompEnd += Len;
                }
                else
                {
                    m_textStore.m_CommitStart = AcpStart;
                    m_textStore.m_CommitEnd = AcpStart + Len;
                }
                m_textStore.m_Commit = m_textStore.m_CommitEnd - m_textStore.m_CommitStart > 0;
            }
        }
    }
}