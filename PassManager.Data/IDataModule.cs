using System.Collections;
using System.Collections.Specialized;

namespace PassManager.Data
{
	public interface IDataModule : IEnumerable, IEnumerator, INotifyCollectionChanged
	{
		PassContainer this[int index] { get; set; }
		int Count { get; }

		bool AddPassContainer(PassContainer passContainer);
		bool RemovePassContainer(PassContainer passContainer);
		bool RemovePassContainerAt(int index);
		int SearchByName(string name);
		void UpdateCollection();
	}
}
