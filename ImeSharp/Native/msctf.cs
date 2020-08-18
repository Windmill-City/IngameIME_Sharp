using System;
using System.Runtime.InteropServices;

namespace ImeSharp.Native
{
    public partial class msctf
    {
        #region msctf_Desktop

        [DllImport("msctf.dll")]
        public static extern int TF_CreateThreadMgr(out msctfLib.ITfThreadMgr threadManager);

        [DllImport("msctf.dll")]
        public static extern int TF_GetThreadMgr(out msctfLib.ITfThreadMgr threadManager);

        [DllImport("msctf.dll")]
        public static extern int TF_CreateInputProcessorProfiles(out msctfLib.ITfInputProcessorProfiles profiles);

        [DllImport("msctf.dll")]
        public static extern int TF_CreateDisplayAttributeMgr(out msctfLib.ITfDisplayAttributeMgr dam);

        [DllImport("msctf.dll")]
        public static extern int TF_CreateCategoryMgr(out msctfLib.ITfCategoryMgr catmgr);

        #endregion msctf_Desktop

        //------------------------------------------------------
        //
        //  Constants
        //
        //------------------------------------------------------

        #region Constants

        public const int TF_E_LOCKED = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0500;
        public const int TF_E_STACKFULL = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0501;
        public const int TF_E_NOTOWNEDRANGE = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0502;
        public const int TF_E_NOPROVIDER = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0503;
        public const int TF_E_DISCONNECTED = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0504;
        public const int TF_E_INVALIDVIEW = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0505;
        public const int TF_E_ALREADY_EXISTS = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0506;
        public const int TF_E_RANGE_NOT_COVERED = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0507;
        public const int TF_E_COMPOSITION_REJECTED = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0508;
        public const int TF_E_EMPTYCONTEXT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0509;
        public const int TF_E_INVALIDPOS = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0200;
        public const int TF_E_NOLOCK = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0201;
        public const int TF_E_NOOBJECT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0202;
        public const int TF_E_NOSERVICE = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0203;
        public const int TF_E_NOINTERFACE = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0204;
        public const int TF_E_NOSELECTION = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0205;
        public const int TF_E_NOLAYOUT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0206;
        public const int TF_E_INVALIDPOINT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0207;
        public const int TF_E_SYNCHRONOUS = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0208;
        public const int TF_E_READONLY = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x0209;
        public const int TF_E_FORMAT = (HResult.SEVERITY_ERROR << 31) | (HResult.FACILITY_ITF << 16) | 0x020a;
        public const int TF_S_ASYNC = (HResult.SEVERITY_SUCCESS << 31) | (HResult.FACILITY_ITF << 16) | 0x0300;

        public const int TF_RCM_COMLESS = 0x00000001;
        public const int TF_RCM_VKEY = 0x00000002;
        public const int TF_RCM_HINT_READING_LENGTH = 0x00000004;
        public const int TF_RCM_HINT_COLLISION = 0x00000008;

        public const int TKB_ALTERNATES_STANDARD = 0x00000001;
        public const int TKB_ALTERNATES_FOR_AUTOCORRECTION = 0x00000002;
        public const int TKB_ALTERNATES_FOR_PREDICTION = 0x00000003;
        public const int TKB_ALTERNATES_AUTOCORRECTION_APPLIED = 0x00000004;

        public const int TF_INVALID_GUIDATOM = 0;
        public const int TF_CLIENTID_NULL = 0;

        public const int TF_TMAE_NOACTIVATETIP = 0x00000001;
        public const int TF_TMAE_SECUREMODE = 0x00000002;
        public const int TF_TMAE_UIELEMENTENABLEDONLY = 0x00000004;
        public const int TF_TMAE_COMLESS = 0x00000008;
        public const int TF_TMAE_WOW16 = 0x00000010;
        public const int TF_TMAE_NOACTIVATEKEYBOARDLAYOUT = 0x00000020;
        public const int TF_TMAE_CONSOLE = 0x00000040;

        public const int TF_TMF_NOACTIVATETIP = TF_TMAE_NOACTIVATETIP;
        public const int TF_TMF_SECUREMODE = TF_TMAE_SECUREMODE;
        public const int TF_TMF_UIELEMENTENABLEDONLY = TF_TMAE_UIELEMENTENABLEDONLY;
        public const int TF_TMF_COMLESS = TF_TMAE_COMLESS;
        public const int TF_TMF_WOW16 = TF_TMAE_WOW16;
        public const int TF_TMF_CONSOLE = TF_TMAE_CONSOLE;

        public const int TF_TMF_IMMERSIVEMODE = 0x40000000;
        public const uint TF_TMF_ACTIVATED = 0x80000000;
        public const int TF_MOD_ALT = 0x0001;
        public const int TF_MOD_CONTROL = 0x0002;
        public const int TF_MOD_SHIFT = 0x0004;
        public const int TF_MOD_RALT = 0x0008;
        public const int TF_MOD_RCONTROL = 0x0010;
        public const int TF_MOD_RSHIFT = 0x0020;
        public const int TF_MOD_LALT = 0x0040;
        public const int TF_MOD_LCONTROL = 0x0080;
        public const int TF_MOD_LSHIFT = 0x0100;
        public const int TF_MOD_ON_KEYUP = 0x0200;
        public const int TF_MOD_IGNORE_ALL_MODIFIER = 0x0400;
        public const int TF_US_HIDETIPUI = 0x00000001;
        public const int TF_DISABLE_SPEECH = 0x00000001;
        public const int TF_DISABLE_DICTATION = 0x00000002;
        public const int TF_DISABLE_COMMANDING = 0x00000004;
        public const string TF_PROCESS_ATOM = "_CTF_PROCESS_ATOM_";
        public const string TF_ENABLE_PROCESS_ATOM = "_CTF_ENABLE_PROCESS_ATOM_";
        public const int TF_INVALID_UIELEMENTID = -1;
        public const int TF_CLUIE_DOCUMENTMGR = 0x00000001;
        public const int TF_CLUIE_COUNT = 0x00000002;
        public const int TF_CLUIE_SELECTION = 0x00000004;
        public const int TF_CLUIE_STRING = 0x00000008;
        public const int TF_CLUIE_PAGEINDEX = 0x00000010;
        public const int TF_CLUIE_CURRENTPAGE = 0x00000020;
        public const int TF_RIUIE_CONTEXT = 0x00000001;
        public const int TF_RIUIE_STRING = 0x00000002;
        public const int TF_RIUIE_MAXREADINGSTRINGLENGTH = 0x00000004;
        public const int TF_RIUIE_ERRORINDEX = 0x00000008;
        public const int TF_RIUIE_VERTICALORDER = 0x00000010;
        public const int TF_CONVERSIONMODE_ALPHANUMERIC = 0x0000;
        public const int TF_CONVERSIONMODE_NATIVE = 0x0001;
        public const int TF_CONVERSIONMODE_KATAKANA = 0x0002;
        public const int TF_CONVERSIONMODE_FULLSHAPE = 0x0008;
        public const int TF_CONVERSIONMODE_ROMAN = 0x0010;
        public const int TF_CONVERSIONMODE_CHARCODE = 0x0020;
        public const int TF_CONVERSIONMODE_SOFTKEYBOARD = 0x0080;
        public const int TF_CONVERSIONMODE_NOCONVERSION = 0x0100;
        public const int TF_CONVERSIONMODE_EUDC = 0x0200;
        public const int TF_CONVERSIONMODE_SYMBOL = 0x0400;
        public const int TF_CONVERSIONMODE_FIXED = 0x0800;
        public const int TF_SENTENCEMODE_NONE = 0x0000;
        public const int TF_SENTENCEMODE_PLAURALCLAUSE = 0x0001;
        public const int TF_SENTENCEMODE_SINGLECONVERT = 0x0002;
        public const int TF_SENTENCEMODE_AUTOMATIC = 0x0004;
        public const int TF_SENTENCEMODE_PHRASEPREDICT = 0x0008;
        public const int TF_SENTENCEMODE_CONVERSATION = 0x0010;
        public const int TF_TRANSITORYEXTENSION_NONE = 0x0000;
        public const int TF_TRANSITORYEXTENSION_FLOATING = 0x0001;
        public const int TF_TRANSITORYEXTENSION_ATSELECTION = 0x0002;
        public const int TF_PROFILETYPE_INPUTPROCESSOR = 0x0001;
        public const int TF_PROFILETYPE_KEYBOARDLAYOUT = 0x0002;
        public const int TF_RIP_FLAG_FREEUNUSEDLIBRARIES = 0x00000001;
        public const int TF_IPP_FLAG_ACTIVE = 0x00000001;
        public const int TF_IPP_FLAG_ENABLED = 0x00000002;
        public const int TF_IPP_FLAG_SUBSTITUTEDBYINPUTPROCESSOR = 0x00000004;
        public const int TF_IPP_CAPS_DISABLEONTRANSITORY = 0x00000001;
        public const int TF_IPP_CAPS_SECUREMODESUPPORT = 0x00000002;
        public const int TF_IPP_CAPS_UIELEMENTENABLED = 0x00000004;
        public const int TF_IPP_CAPS_COMLESSSUPPORT = 0x00000008;
        public const int TF_IPP_CAPS_WOW16SUPPORT = 0x00000010;
        public const int TF_IPP_CAPS_IMMERSIVESUPPORT = 0x00010000;
        public const int TF_IPP_CAPS_SYSTRAYSUPPORT = 0x00020000;
        public const int TF_IPPMF_FORPROCESS = 0x10000000;
        public const int TF_IPPMF_FORSESSION = 0x20000000;
        public const int TF_IPPMF_FORSYSTEMALL = 0x40000000;
        public const int TF_IPPMF_ENABLEPROFILE = 0x00000001;
        public const int TF_IPPMF_DISABLEPROFILE = 0x00000002;
        public const int TF_IPPMF_DONTCARECURRENTINPUTLANGUAGE = 0x00000004;
        public const int TF_RP_HIDDENINSETTINGUI = 0x00000002;
        public const int TF_RP_LOCALPROCESS = 0x00000004;
        public const int TF_RP_LOCALTHREAD = 0x00000008;
        public const int TF_RP_SUBITEMINSETTINGUI = 0x00000010;
        public const int TF_URP_ALLPROFILES = 0x00000002;
        public const int TF_URP_LOCALPROCESS = 0x00000004;
        public const int TF_URP_LOCALTHREAD = 0x00000008;
        public const int TF_IPSINK_FLAG_ACTIVE = 0x0001;

        public const uint TF_INVALID_COOKIE = 0xffffffff;

        #endregion Constants
    }
}