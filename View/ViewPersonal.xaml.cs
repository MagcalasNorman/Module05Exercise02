using Module05Exercise01.Service;
using Module05Exercise01.ViewModel;

namespace Module05Exercise01.View;

public partial class ViewPersonal : ContentPage
{
	public ViewPersonal()
	{
		InitializeComponent();
        var personalViewModel = new PersonalViewModel();
        BindingContext = personalViewModel;
    }

    private async void BackMenu (object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}