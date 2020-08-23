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
    }
}