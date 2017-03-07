namespace PassManager.Data.FileModule
{
	internal class FileModule : IFileModule
	{
		private IFileModule FileInterface { get; }

		public FileModule() : this(FileType.xml)
		{
			
		}

		public FileModule(FileType fileType)
		{
			switch (fileType)
			{
				case FileType.xml:
					FileInterface = new XMLMod();
					break;
			}
		}

		public PassCollection OpenFile(string path)
		{
			return FileInterface.OpenFile(path);
		}

		public bool SaveFile(string path, IDataModule passCollection)
		{
			return FileInterface.SaveFile(path, passCollection);
		}
	}
}
