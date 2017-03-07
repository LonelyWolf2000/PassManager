using PassManager.Data;

namespace PassManager.Logic
{
	public interface IPassManager
	{
		PassContainer CurrentPass { get;}
		IDataModule PassCollection { get; }
		string CurrentFilePath { get; }
		bool IsSaved { get; }

		bool OpenFile(string path);
		bool OpenFile(string path, FileType fileType);
		bool SaveFile(string path);
		bool SaveFile(string path, FileType fileType);

		void SelectPass(int index);
		void SelectPass(string name);
		void AddNewPass(PassContainer passContainer);
		void DeleteSelectedPass();
		void EditPass();
	}
}
