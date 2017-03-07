using System;

namespace PassManager.Data.FileModule
{
	internal interface IFileModule
	{
		PassCollection OpenFile(string path);
		bool SaveFile(string path, IDataModule passCollection);
	}
}
