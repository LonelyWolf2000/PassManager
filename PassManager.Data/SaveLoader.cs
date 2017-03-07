using PassManager.Data.FileModule;

namespace PassManager.Data
{
	public static class SaveLoader
	{
		public static PassCollection LoadFromFile(string path)
		{
			IFileModule fileModule = new FileModule.FileModule();
			return fileModule.OpenFile(path);
		}

		public static PassCollection LoadFromFile(string path, FileType fileType)
		{
			IFileModule fileModule = new FileModule.FileModule(fileType);
			return fileModule.OpenFile(path);
		}

		public static bool SaveFile(string path, IDataModule passCollection)
		{
			return SaveFile(path, passCollection, FileType.xml);
		}

		public static bool SaveFile(string path, IDataModule passCollection, FileType fileType)
		{
			IFileModule fileModule = new FileModule.FileModule(fileType);
			return fileModule.SaveFile(path, passCollection);
		}
	}
}