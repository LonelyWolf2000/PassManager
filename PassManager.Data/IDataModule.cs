using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PassManager.Data
{
	public interface IDataModule : IEnumerable, IEnumerator, INotifyCollectionChanged
	{
		//List<PassContainer> PasswordsList { get; }
		PassContainer this[int index] { get; set; }
		int Count { get; }

		bool AddPassContainer(PassContainer passContainer);
		bool RemovePassContainer(PassContainer passContainer);
		bool RemovePassContainerAt(int index);
		int SearchByName(string name);



		//IEnumerator GetEnumerator();

		//bool OpenFile(string path);
		//bool OpenFile(string path, FileType fileType);
		//bool SaveFile(string path);
		//bool SaveFile(string path, FileType fileType);
	}
}
