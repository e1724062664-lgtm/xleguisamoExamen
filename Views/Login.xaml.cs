namespace xleguisamoExamen.Views;

public partial class Login : ContentPage
{
    private readonly string[,] usuariosMatriz = new string[4, 2]
{
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
};

    public Login()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        string user = (txtUsuario.Text ?? string.Empty).Trim();
        string pass = txtContrasena.Text ?? string.Empty;

        bool credencialesCorrectas = false;

        for (int i = 0; i < 4; i++)
        {
            if (usuariosMatriz[i, 0] == user && usuariosMatriz[i, 1] == pass)
            {
                credencialesCorrectas = true;
                break;
            }
        }

        if (credencialesCorrectas)
        {
            txtUsuario.Text = string.Empty;
            txtContrasena.Text = string.Empty;
            await Navigation.PushAsync(new Registro(user));
        }
        else
        {
            await DisplayAlertAsync("Error de Autenticación", "Usuario o contraseña incorrectos.", "OK");
        }
    }

    private async void OnAcercaDeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AcercaDe());
    }

}