using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PassManager.ViewModel
{
	internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected ViewModelBase()
		{
			
		}

		public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		public void Dispose()
		{
			this.Dispose();
		}
	}
}
