using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PassManager.Data;


namespace PassManager.Logic
{
	public class PassManager : IPassManager
	{
		private IDataModule _dataModule;
		private PassContainer _currentPass;
		private int _currentIndex = -1;
		private string _currentFile = "";
		private FileType _currentFileType;
		private bool _isSaved = true;

		#region Constructors

		public PassManager()
		{
			_dataModule = new PassCollection();
			_currentPass = null;
		}
		public PassManager(string path) : this(path, FileType.xml)
		{

		}
		public PassManager(string path, FileType fileType)
		{
			OpenFile(path, fileType);
		}

		#endregion

		#region Props
		public PassContainer CurrentPass
		{
			get { return _currentPass; }
		}

		public IDataModule PassCollection
		{
			get { return _dataModule; }
		}

		public string CurrentFilePath
		{
			get { return _currentFile; }
		}

		public bool IsSaved
		{
			get { return _isSaved; }
		} 
		#endregion

		public bool OpenFile(string path)
		{
			return OpenFile(path, FileType.xml);
		}

		public bool OpenFile(string path, FileType fileType)
		{
			_dataModule = SaveLoader.LoadFromFile(path);
			if (_dataModule != null)
			{
				_SetCurrentFile(path, fileType);
				SelectPass(0);
			}

			return _dataModule != null;
		}

		public bool SaveFile(string path)
		{
			return SaveFile(path, FileType.xml);
		}

		public bool SaveFile(string path, FileType fileType)
		{
			_isSaved = SaveLoader.SaveFile(path, _dataModule, fileType);
			return _isSaved;
		}

		public void SelectPass(int index)
		{
			if (_dataModule.Count == 0)
			{
				_currentPass = null;
				return;
			}

			if (index > _dataModule.Count - 1 || index < 0)
				return;

			_currentIndex = index;
			_currentPass = new PassContainer
			{
				Name = _dataModule[index].Name,
				Resource = _dataModule[index].Resource,
				Login = _dataModule[index].Login,
				Pass = _dataModule[index].Pass,
				Comment = _dataModule[index].Comment
			};
		}

		public void SelectPass(string name)
		{
			int index = _dataModule.SearchByName(name);
			if (index > -1)
				SelectPass(index);
		}

		public void AddNewPass(PassContainer passContainer)
		{
			if (_dataModule.AddPassContainer(passContainer))
				_currentPass = passContainer;

			_isSaved = false;
		}

		public void DeleteSelectedPass()
		{
			if (_dataModule.RemovePassContainer(_currentPass))
				SelectPass(_currentIndex == 0 ? 0:_currentIndex - 1);
			else
				SelectPass(0);

			_isSaved = false;
		}

		public void EditPass()
		{
			if(_dataModule[_currentIndex].Name != _currentPass.Name && _dataModule.SearchByName(_currentPass.Name) > -1)
				return;
			
			_dataModule[_currentIndex].Name = _currentPass.Name;
			_dataModule[_currentIndex].Resource = _currentPass.Resource;
			_dataModule[_currentIndex].Login = _currentPass.Login;
			_dataModule[_currentIndex].Pass = _currentPass.Pass;
			_dataModule[_currentIndex].Comment = _currentPass.Comment;

			SaveFile(_currentFile, _currentFileType);
		}

		private void _SetCurrentFile(string path, FileType fileType)
		{
			_currentFile = path;
			_currentFileType = fileType;
		}
	}
}