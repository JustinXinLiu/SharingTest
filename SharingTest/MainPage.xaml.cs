using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SharingTest
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

		private DataTransferManager dataTransferManager;
		private StorageFile _file;

		public MainPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// Register the current page as a share source.
			this.dataTransferManager = DataTransferManager.GetForCurrentView();
			this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);

			// Request to be notified when the user chooses a share target app.
			// this.dataTransferManager.TargetApplicationChosen += OnTargetApplicationChosen;
		}

		// When share is invoked (by the user or programatically) the event handler we registered will be called to populate the datapackage with the
		// data to be shared.
		private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
		{
			// Register to be notified if the share operation completes.
			//e.Request.Data.ShareCompleted += OnShareCompleted;

			// Call the scenario specific function to populate the datapackage with the data to be shared.
			var request = e.Request;
			var data = request.Data;

			if (_file != null)
			{

				data.Properties.Title = _file.DisplayName;
				data.SetStorageItems(new[] { _file }, true);
			}
			else
			{
				request.FailWithDisplayText("Unable to share file.");
			}
		}

		private async void BrowseFile_OnClick(object sender, RoutedEventArgs e)
		{
			var filePicker = new FileOpenPicker
			{
				ViewMode = PickerViewMode.List,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				FileTypeFilter = { "*" }
			};

			_file = await filePicker.PickSingleFileAsync();

			ShareButton.IsEnabled = true;

		}

		private async void ShareFile_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				// If the user clicks the share button, invoke the share flow programatically.
				DataTransferManager.ShowShareUI();
			}
			catch (Exception ex)
			{
				var dialog = new MessageDialog($"Error Message : {ex.Message}", "Error");
				await dialog.ShowAsync();
			}
		}
	}
}
