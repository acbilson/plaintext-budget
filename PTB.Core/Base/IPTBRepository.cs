using PTB.Core.FolderAccess;

namespace PTB.Core.Base
{
    public interface IPTBFileService
    {
        BaseReadResponse Read(BasePTBFile file, int index, int count);

        BaseUpdateResponse Update(BasePTBFile file, int index, PTBRow row);

        BaseAppendResponse Append(BasePTBFile file, PTBRow row);
    }
}