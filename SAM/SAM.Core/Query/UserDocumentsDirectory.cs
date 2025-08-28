using System;

namespace SAM 
 // namespace  SAM.Core
{
    public static partial class Query
    {
        public static string UserDocumentsDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}