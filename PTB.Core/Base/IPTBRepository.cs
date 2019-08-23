﻿namespace PTB.Core.Base
{
    public interface IPTBRepository
    {
        BaseReadResponse Read(int index, int count);

        BaseUpdateResponse Update(int index, PTBRow row);

        BaseAppendResponse Append(PTBRow row);
    }
}