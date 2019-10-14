namespace PTB.Core
{
    public class PTBSettings
    {
        public string HomeDirectory
        {
            get
            {
                if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                {
                    return UnixHomeDirectory;
                }
                else
                {
                    return WindowsHomeDirectory;
                }
            }
        }

        public string WindowsHomeDirectory;
        public string UnixHomeDirectory;
        public string FileExtension;
        public char FileDelimiter;
        public PTB.Core.Logging.LoggingLevel LoggingLevel;
    }
}