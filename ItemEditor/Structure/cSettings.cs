using System.Collections.Generic;

namespace ItemEditor.Structure
{
    public enum ShowItemIconInList { ALL, ONLY_CURRENT, NO }

    internal class cSettings
    {
        public List<cDatabaseConfig> CONNECTION;
        public List<cToolConfig> SETTINGS;
    }

    internal class cDatabaseConfig
    {
        public string Name,
            Note, 
            Host, 
            Database, 
            Username, 
            Password;

        public int Port;
    }

    internal class cToolConfig
    {
        public string ClientDirectory;
        public bool UseTextureFilesFromClient, LogToFile;

        public ShowItemIconInList ShowItemIconInList;
    }

    internal class cTempVariables
    {
        public static int lastTexID;
    }
}
