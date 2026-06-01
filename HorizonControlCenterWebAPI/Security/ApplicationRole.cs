namespace HorizonControlCenterWebAPI.Security
{
    public static class ApplicationRole
    {
        public const string GLOBALS_READER = "GLOBALS_READER"; //Get Method
        public const string GLOBALS_AUTHOR = "GLOBALS_AUTHOR"; //Create,Update Methods
        public const string GLOBALS_ADMIN = "GLOBALS_ADMIN";
        public const string GLOBALS_EDITOR = "GLOBALS_EDITOR";
    }
}
