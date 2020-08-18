namespace ImeSharp
{
    public class HResult
    {
        #region Constants

        public const int S_OK = 0x00000000;
        public const int S_FALSE = 0x00000001;

        public const int E_FAIL = unchecked((int)0x80004005);
        public const int E_INVALIDARG = unchecked((int)0x80070057);
        public const int E_NOTIMPL = unchecked((int)0x80004001);
        public const int E_UNEXPECTED = unchecked((int)0x8000FFFF);

        public const int SEVERITY_SUCCESS = 0;
        public const int SEVERITY_ERROR = 1;

        public const int FACILITY_ITF = 4;

        #endregion Constants

        #region StaticMethod

        public static int makeHResult(ulong sev, ulong fac, ulong code)
        {
            return (int)((sev << 31) | (fac << 16) | (code));
        }

        #endregion StaticMethod
    }
}