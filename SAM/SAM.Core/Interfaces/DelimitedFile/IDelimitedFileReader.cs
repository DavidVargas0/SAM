using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Core
{
    public interface IDelimitedFileReader
    {
        char Separator { get; }

        bool Read(DelimitedFileRow DelimitedFileRow);

        List<DelimitedFileRow> Read();
    }
}