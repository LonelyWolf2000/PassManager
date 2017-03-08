using System.ComponentModel;
using System.Windows;
using PassManager.ViewModel;

namespace PassManager
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel _viewModel;
		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new MainWindowViewModel();
			this.DataContext = _viewModel;

			//if(viewModel.IsCanselClosing == null)
			//{
			//	viewModel.IsCanselClosing = new Action(() =>
			//	{
			//		Closing += OnClosingHandler();
			//		OnClosed(new EventArgs());
			//		//OnClosing(new CancelEventArgs(true));
			//	});
			//}
		}

		//Костыль, ибо я хз как
		private void OnClosingHandler(object sender, CancelEventArgs e)
		{
			_viewModel.CloseCommand.Execute(null);
			e.Cancel = _viewModel.IsCanselClosing;
		}
	}
}
