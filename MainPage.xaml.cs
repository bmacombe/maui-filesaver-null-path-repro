using System.Text;
using CommunityToolkit.Maui.Storage;

namespace FileSaverNullPathRepro;

public partial class MainPage : ContentPage
{
	private const string FileName = "file-saver-null-path-repro.txt";

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnNullInitialPathClicked(object? sender, EventArgs e)
	{
		await SaveAsync(useNullInitialPath: true);
	}

	private async void OnNoInitialPathClicked(object? sender, EventArgs e)
	{
		await SaveAsync(useNullInitialPath: false);
	}

	private async Task SaveAsync(bool useNullInitialPath)
	{
		try
		{
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes("FileSaver null-path reproduction."));
			StatusLabel.Text = "Invoking FileSaver...";

			var result = useNullInitialPath
				? await FileSaver.SaveAsync(null!, FileName, stream)
				: await FileSaver.SaveAsync(FileName, stream);

			if (result.Exception is not null)
			{
				StatusLabel.Text = result.Exception.ToString();
				return;
			}

			StatusLabel.Text = result.IsSuccessful
				? $"Saved to: {result.FilePath}"
				: "Save canceled.";
		}
		catch (Exception exception)
		{
			StatusLabel.Text = exception.ToString();
		}
	}
}
