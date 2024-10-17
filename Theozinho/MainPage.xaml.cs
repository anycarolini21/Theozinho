namespace Theozinho;

public partial class MainPage : ContentPage
{


	public MainPage()
	{
		InitializeComponent();
	}

	private async void BotaoIniciar(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new JogoP());
	}
}
