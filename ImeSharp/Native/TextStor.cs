namespace ImeSharp
{
    public class TextStor
    {
        public const int TS_E_INVALIDPOS = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0200;
        public const int TS_E_NOLOCK = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0201;
        public const int TS_E_NOOBJECT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0202;
        public const int TS_E_NOSERVICE = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0203;
        public const int TS_E_NOINTERFACE = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0204;
        public const int TS_E_NOSELECTION = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0205;
        public const int TS_E_NOLAYOUT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0206;
        public const int TS_E_INVALIDPOINT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0207;
        public const int TS_E_SYNCHRONOUS = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0208;
        public const int TS_E_READONLY = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0209;
        public const int TS_E_FORMAT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x020a;
        public const int TS_S_ASYNC = (HResult.SEVERITY_SUCCESS << 31) | (HResult.FACILITY_ITF << 16) | 0x0300;

        public const int TS_AS_TEXT_CHANGE = 0x1;

        public const int TS_AS_SEL_CHANGE = 0x2;

        public const int TS_AS_LAYOUT_CHANGE = 0x4;

        public const int TS_AS_ATTR_CHANGE = 0x8;

        public const int TS_AS_STATUS_CHANGE = 0x10;

        public const int TS_AS_ALL_SINKS = TS_AS_TEXT_CHANGE | TS_AS_SEL_CHANGE | TS_AS_LAYOUT_CHANGE | TS_AS_ATTR_CHANGE | TS_AS_STATUS_CHANGE;

        public const int TS_LF_SYNC = 0x1;

        public const int TS_LF_READ = 0x2;

        public const int TS_LF_READWRITE = 0x6;

        public const int TS_SD_READONLY = 0x1;

        public const int TS_SD_LOADING = 0x2;

        public const int TS_SD_RESERVED = 0x4;

        public const int TS_SD_TKBAUTOCORRECTENABLE = 0x8;

        public const int TS_SD_TKBPREDICTIONENABLE = 0x10;

        public const int TS_SD_UIINTEGRATIONENABLE = 0x20;

        public const int TS_SD_INPUTPANEMANUALDISPLAYENABLE = 0x40;

        public const int TS_SD_EMBEDDEDHANDWRITINGVIEW_ENABLED = 0x80;

        public const int TS_SD_EMBEDDEDHANDWRITINGVIEW_VISIBLE = 0x100;

        public const int TS_SS_DISJOINTSEL = 0x1;

        public const int TS_SS_REGIONS = 0x2;

        public const int TS_SS_TRANSITORY = 0x4;

        public const int TS_SS_NOHIDDENTEXT = 0x8;

        public const int TS_SS_TKBAUTOCORRECTENABLE = 0x10;

        public const int TS_SS_TKBPREDICTIONENABLE = 0x20;

        public const int TS_SS_UWPCONTROL = 0x40;

        public const int TS_SD_MASKALL = TS_SD_READONLY | TS_SD_LOADING;

        public const int TS_ST_CORRECTION = 0x1;

        public const int TS_IE_CORRECTION = 0x1;

        public const int TS_IE_COMPOSITION = 0x2;

        public const int TS_TC_CORRECTION = 0x1;

        public const int TS_IAS_NOQUERY = 0x1;

        public const int TS_IAS_QUERYONLY = 0x2;

        public const uint TS_DEFAULT_SELECTION = unchecked((uint)-1);

        public const int GXFPF_ROUND_NEAREST = 0x1;

        public const int GXFPF_NEAREST = 0x2;

        public const int TS_CHAR_EMBEDDED = 0xfffc;

        public const int TS_CHAR_REGION = 0;

        public const int TS_CHAR_REPLACEMENT = 0xfffd;

        public const int TS_ATTR_FIND_BACKWARDS = 0x1;

        public const int TS_ATTR_FIND_WANT_OFFSET = 0x2;

        public const int TS_ATTR_FIND_UPDATESTART = 0x4;

        public const int TS_ATTR_FIND_WANT_VALUE = 0x8;

        public const int TS_ATTR_FIND_WANT_END = 0x10;

        public const int TS_ATTR_FIND_HIDDEN = 0x20;
    }
}