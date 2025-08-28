using System.IO.Compression;

//namespace SAM
namespace SAM.Core
{
    public static partial class Create
    {
        public static ZipArchiveInfo ZipArchiveInfo(this ZipArchive zipArchive)
        {
            if (zipArchive == null)
                return null;

            ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry(ZipArchiveInfo.EntryName);
            if (zipArchiveEntry == null)
                return null;

            return IJSAMObject<ZipArchiveInfo>(zipArchiveEntry);
        }
    }
}