namespace xleguisamoExamen.Views;

public partial class Resumen : ContentPage
{

    private readonly string usuarioActual;
    public Resumen(string usuario, string nombre, string apellido, string edad, string fecha, string plan, string casillero, string montoInicial, string pagoMensual, string pagoTotal)
    {
        InitializeComponent();

        usuarioActual = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {usuarioActual}";

        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad;
        lblFechaInicio.Text = fecha;
        lblPlan.Text = plan;
        lblCasillero.Text = casillero;
        lblMontoInicial.Text = $"${montoInicial}";
        lblPagoMensual.Text = $"${pagoMensual}";
        lblPagoTotal.Text = $"${pagoTotal}";
    }

    private async void OnNuevoRegistroClicked(object? sender, EventArgs e)
    {
        // Reemplaza el Registro anterior por uno limpio y regresa a él,
        // evitando que se acumulen páginas en la pila (Login -> Registro -> Resumen)
        var stack = Navigation.NavigationStack;
        if (stack.Count >= 2 && stack[^2] is Registro registroAnterior)
        {
            Navigation.InsertPageBefore(new Registro(usuarioActual), registroAnterior);
            Navigation.RemovePage(registroAnterior);
        }
        await Navigation.PopAsync();
    }

    private async void OnCerrarSesionClicked(object? sender, EventArgs e)
    {
        // Regresa a la vista principal de Login
        await Navigation.PopToRootAsync();
    }

}