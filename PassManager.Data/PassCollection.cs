using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Data
{
	public class PassCollection : IDataModule
	{
		private int _position = -1;
		private int _count;
		private List<PassContainer> _passwordList;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public int Count
		{
			get { return _count; }
		}
		public PassContainer this[int index]
		{
			get { return _passwordList[index]; }
			set { _passwordList[index] = value; }
		}

		#region Constructors
		public PassCollection() : this(new List<PassContainer>())
		{
			
		}

		public PassCollection(List<PassContainer> passwordsList)
		{
			_passwordList = passwordsList;
			_count = _passwordList.Count;
		}
		#endregion


		public bool AddPassContainer(PassContainer passContainer)
		{
			if (passContainer == null || passContainer.Name.Length < 1)
				return false;

			if (SearchByName(passContainer.Name) > -1)
				return false;

			_passwordList.Add(passContainer);
			_count++;
			Reset();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			return true;
		}

		public bool RemovePassContainer(PassContainer passContainer)
		{
			if (passContainer == null)
				return false;

			int index = SearchByName(passContainer.Name);
			return RemovePassContainerAt(index);
		}

		public bool RemovePassContainerAt(int index)
		{
			if (index < 0 || index > _passwordList.Count -1)
				return false;

			_passwordList.RemoveAt(index);
			_count--;
			Reset();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			return true;
		}

		public int SearchByName(string name)
		{
			return _passwordList.FindIndex(x => x.Name == name);
		}

		#region IEnumerable & IEnumerator

		public object Current
		{
			get { return _passwordList[_position]; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _passwordList.GetEnumerator();
		}

		bool IEnumerator.MoveNext()
		{
			if (_position < _passwordList.Count - 1)
			{
				_position++;
				return true;
			}

			Reset();
			return false;
		}

		public void Reset()
		{
			_position = -1;
		}

		#endregion
	}
}
