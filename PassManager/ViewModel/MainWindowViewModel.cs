using System.Windows;
using Microsoft.Win32;
using PassManager.Data;
using PassManager.Infrastructure;
using PassManager.Logic;

namespace PassManager.ViewModel
{
	enum MainWindowState
	{
		ViewState,
		AddingState
	}

	internal class MainWindowViewModel : ViewModelBase
	{
		private IPassManager _passManager;
		private PassContainer _currentPass;
		private MainWindowState _mainWindowState = MainWindowState.ViewState;
		private string _status = "";
		private string _savedStatus = "";
		private bool _isNameFieldValid = true;
		private bool _isNameFieldChanged;

		#region Props

		public bool IsCanselClosing { get; private set; }
		public MainWindowState MainWindowState
		{
			get { return _mainWindowState; }
			set
			{
				_mainWindowState = value;
				OnPropertyChanged();
			}
		}

		public IPassManager PasswordManager
		{
			get
			{
				if (_passManager == null)
					_passManager = new Logic.PassManager();
				return _passManager;
			}
		}

		public string NameField
		{
			get { return CurrentPass?.Name; }
			set
			{
				if(CurrentPass == null) return;

				if(MainWindowState == MainWindowState.ViewState)
					_isNameFieldChanged = true;

				IsFieldsChanged = true;
				CurrentPass.Name = value;
				OnPropertyChanged();
			}
		}

		public string ResourceField
		{
			get { return CurrentPass?.Resource; }
			set
			{
				if (CurrentPass == null) return;

				IsFieldsChanged = true;
				CurrentPass.Resource = value;
				OnPropertyChanged();
			}
		}

		public string LoginField
		{
			get { return CurrentPass?.Login; }
			set
			{
				if (CurrentPass == null) return;

				IsFieldsChanged = true;
				CurrentPass.Login = value;
				OnPropertyChanged();
			}
		}

		public string PassField
		{
			get { return CurrentPass?.Pass; }
			set
			{
				if (CurrentPass == null) return;

				IsFieldsChanged = true;
				CurrentPass.Pass = value;
				OnPropertyChanged();
			}
		}

		public string CommentField
		{
			get { return CurrentPass?.Comment; }
			set
			{
				if (CurrentPass == null) return;

				IsFieldsChanged = true;
				CurrentPass.Comment = value;
				OnPropertyChanged();
			}
		}

		public string DateField
		{
			get { return CurrentPass?.DateUpdatePass; }
			//set
			//{
			//	if (CurrentPass == null) return;

			//	IsFieldsChanged = true;
			//	CurrentPass.DateUpdatePass = value;
			//	OnPropertyChanged();
			//}
		}

		public bool IsFieldsChanged { get; private set; }
		public bool IsNameFieldValid
		{
			get { return _isNameFieldValid; }
			private set
			{
				_isNameFieldValid = value;
				OnPropertyChanged();
			}
		}

		public PassContainer CurrentPass
		{
			get
			{
				if (_currentPass == null)
					_currentPass = PasswordManager.CurrentPass;

				return _currentPass;
			}
			set
			{
				if (value?.Name.Length > 0)
				{
					PasswordManager.SelectPass(value.Name);
					_currentPass = PasswordManager.CurrentPass;
				}
				else
					_currentPass = value;

				IsFieldsChanged = false;
				IsNameFieldValid = true;
				Status = _savedStatus;
				
				OnPropertyChanged();
				_CallFieldsChangedEvent();
			}
		}

		public string Status
		{
			get { return _status; }
			private set
			{
				_status = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region OpenFile_Command

		RelayCommand _openFileCommand;
		public RelayCommand OpenFileCommand
		{
			get
			{
				if(_openFileCommand == null)
					_openFileCommand = new RelayCommand(OpenFileExecute);
				return _openFileCommand;
			}
		}

		private void OpenFileExecute(object sender)
		{
			OpenFileDialog fileDialog = new OpenFileDialog
			{
				DefaultExt = ".xml",
				Filter = "(.xml)|*.xml"
			};

			if (fileDialog.ShowDialog() == true)
			{
				PasswordManager.OpenFile(fileDialog.FileName);
				OnPropertyChanged("PasswordManager");
			}
		}

		#endregion

		#region SaveFile_Command

		RelayCommand _saveFileCommand;
		public RelayCommand SaveFileCommand
		{
			get
			{
				if (_saveFileCommand == null)
					_saveFileCommand = new RelayCommand(SaveFileExecute, CanSaveFileExecute);
				return _saveFileCommand;
			}
		}

		private void SaveFileExecute(object sender)
		{
			if (PasswordManager.CurrentFilePath != "")
			{
				PasswordManager.SaveFile(PasswordManager.CurrentFilePath);
				return;
			}

			SaveFileDialog fileDialog = new SaveFileDialog
			{
				DefaultExt = ".xml",
				Filter = "(.xml)|*.xml"
			};

			if (fileDialog.ShowDialog() == true)
			{
				_StatusChange(Status, PasswordManager.SaveFile(fileDialog.FileName) ? "Файл сохранен" : "Ошибка: не удалось сохранить файл");
			}
		}

		private bool CanSaveFileExecute(object parameter)
		{
			if (_passManager == null)
				return false;

			return !_passManager.IsSaved;
		}
		#endregion

		#region NewFile_Command

		RelayCommand _newFileCommand;
		public RelayCommand NewFileCommand
		{
			get
			{
				if (_newFileCommand == null)
					_newFileCommand = new RelayCommand(NewFileExecute, CanNewFileExecute);
				return _newFileCommand;
			}
		}

		private void NewFileExecute(object sender)
		{
			_passManager = new Logic.PassManager();
			OnPropertyChanged("PasswordManager");
		}

		private bool CanNewFileExecute(object parameter)
		{
			return PasswordManager?.PassCollection.Count > 0;
		}
		#endregion

		#region AddNote_Command

		RelayCommand _addNoteCommand;
		public RelayCommand AddNoteCommand
		{
			get
			{
				if (_addNoteCommand == null)
					_addNoteCommand = new RelayCommand(AddNoteExecute);
				return _addNoteCommand;
			}
		}

		private void AddNoteExecute(object sender)
		{
			CurrentPass = new PassContainer {Name = "", Resource = "", Login = "", Pass = "", Comment = "", DateUpdatePass = ""};
			MainWindowState = MainWindowState.AddingState;
		}
		#endregion

		#region EditNote_Command

		RelayCommand _editNoteCommand;
		public RelayCommand EditNoteCommand
		{
			get
			{
				if (_editNoteCommand == null)
					_editNoteCommand = new RelayCommand(EditNoteExecute, CanEditNoteExecute);
				return _editNoteCommand;
			}
		}

		private void EditNoteExecute(object sender)
		{
			if (_isNameFieldChanged && !_ValidateNameField())
				return;

			PasswordManager.EditPass();
			IsFieldsChanged = false;
			_isNameFieldChanged = false;
			_StatusChange(Status, "Изменения сохранены");
		}

		private bool CanEditNoteExecute(object parameter)
		{
			return CurrentPass != null && IsFieldsChanged;
		}
		#endregion

		#region SaveNote_Command

		RelayCommand _saveNoteCommand;
		public RelayCommand SaveNoteCommand
		{
			get
			{
				if (_saveNoteCommand == null)
					_saveNoteCommand = new RelayCommand(SaveNoteExecute, CanSaveNoteExecute);
				return _saveNoteCommand;
			}
		}

		private void SaveNoteExecute(object sender)
		{
			PasswordManager.AddNewPass(CurrentPass);
			MainWindowState = MainWindowState.ViewState;
			CurrentPass = PasswordManager.CurrentPass;
			IsFieldsChanged = false;
			_StatusChange(Status, "Новая запись добавлена");
		}

		private bool CanSaveNoteExecute(object parameter)
		{
			return MainWindowState == MainWindowState.AddingState && _ValidateNameField();
		}
		#endregion

		#region RemoveNote_Command

		RelayCommand _removeNoteCommand;
		public RelayCommand RemoveNoteCommand
		{
			get
			{
				if (_removeNoteCommand == null)
					_removeNoteCommand = new RelayCommand(RemoveNoteExecute, CanRemoveNoteExecute);
				return _removeNoteCommand;
			}
		}

		private void RemoveNoteExecute(object sender)
		{
			PasswordManager.DeleteSelectedPass();
			CurrentPass = PasswordManager.CurrentPass;
			_StatusChange(Status, "Запись удалена");
		}

		private bool CanRemoveNoteExecute(object parameter)
		{
			return CurrentPass != null && !IsFieldsChanged && PasswordManager.PassCollection.SearchByName(NameField) != -1;
		}
		#endregion

		#region CanselAdd_Command

		RelayCommand _canselAddCommand;
		public RelayCommand CanselAddCommand
		{
			get
			{
				if (_canselAddCommand == null)
					_canselAddCommand = new RelayCommand(CanselAdd_CommandExecute);
				return _canselAddCommand;
			}
		}

		private void CanselAdd_CommandExecute(object sender)
		{
			MainWindowState = MainWindowState.ViewState;
			CurrentPass = PasswordManager.CurrentPass;
			IsNameFieldValid = true;
			_StatusChange("", "");
		}
		#endregion

		#region Close_Command

		RelayCommand _closeCommand;
		public RelayCommand CloseCommand
		{
			get
			{
				if (_closeCommand == null)
					_closeCommand = new RelayCommand(Close_CommandExecute);
				return _closeCommand;
			}
		}

		private void Close_CommandExecute(object sender)
		{
			MessageBoxResult result;
			IsCanselClosing = false;

			if (IsFieldsChanged)
			{
				result = MessageBox.Show("Запись редактировалась и не была сохнанена. Все равно закрыть?", "Внимание!", MessageBoxButton.YesNo);

				if (result == MessageBoxResult.No)
					IsCanselClosing = true;

				return;
			}


			if (PasswordManager.IsSaved) return;

			result = MessageBox.Show("Сохранить ли файл перед закрытием?", "Данные не сохранены", MessageBoxButton.YesNoCancel);

			switch (result)
			{
				case MessageBoxResult.Yes:
					PasswordManager.SaveFile(PasswordManager.CurrentFilePath);
					IsCanselClosing = false;
					break;
				case MessageBoxResult.No:
					IsCanselClosing = false;
					break;
				case MessageBoxResult.Cancel:
					IsCanselClosing = true;
					break;
			}
		}
		#endregion

		private bool _ValidateNameField()
		{
			if (NameField?.Length == 0)
			{
				IsNameFieldValid = false;
				_StatusChange(Status, "Ошибка: поле с именем не может быть пустым");
				return false;
			}

			if (NameField != null && PasswordManager.PassCollection.SearchByName(NameField) != -1)
			{
				IsNameFieldValid = false;
				_StatusChange(Status, "Ошибка: запись с таким именем уже существует");
				return false;
			}

			_StatusChange("", _savedStatus);
			IsNameFieldValid = true;
			return true;
		}

		private void _StatusChange(string oldStatus, string newStatus)
		{
			_savedStatus = oldStatus;
			Status = newStatus;
		}

		private void _CallFieldsChangedEvent()
		{
			OnPropertyChanged("NameField");
			OnPropertyChanged("ResourceField");
			OnPropertyChanged("LoginField");
			OnPropertyChanged("PassField");
			OnPropertyChanged("DateField");
			OnPropertyChanged("CommentField");
		}
	}
}
