namespace xleguisamoExamen.Views;

public partial class Registro : ContentPage
{
    private readonly string usuarioConectado;
    private double pagoMensualCalculado = 0;
    private double pagoTotalCalculado = 0;
    private bool esCalculado = false;

    public Registro(string usuario)
    {
        InitializeComponent();
        usuarioConectado = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {usuarioConectado}";

        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;

        // Si cambian los datos que afectan el pago, se debe volver a calcular
        txtMontoInicial.TextChanged += (s, e) => InvalidarCalculo();
        swCasillero.Toggled += (s, e) => InvalidarCalculo();
    }

    private void InvalidarCalculo()
    {
        esCalculado = false;
        txtPagoMensual.Text = string.Empty;
    }

    private async Task<bool> ValidarCampos()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text))
        {
            await DisplayAlertAsync("Atención", "Por favor, llene los datos de Nombre, Apellido y Edad.", "OK");
            return false;
        }

        if (!int.TryParse(txtEdad.Text, out int edad) || edad < 16)
        {
            await DisplayAlertAsync("Atención", "La edad debe ser un número entero mayor o igual a 16.", "OK");
            return false;
        }

        if (pkPlan.SelectedIndex == -1)
        {
            await DisplayAlertAsync("Atención", "Seleccione un plan de entrenamiento.", "OK");
            return false;
        }

        if (!double.TryParse(txtMontoInicial.Text, out double montoInicial) || montoInicial < 120 || montoInicial > 600)
        {
            await DisplayAlertAsync("Atención", "El monto inicial debe estar entre 120 (10%) y 600 (50%).", "OK");
            return false;
        }

        return true;
    }

    private async void OnCalcularClicked(object? sender, EventArgs e)
    {
        if (!await ValidarCampos()) return;

        double montoInicial = double.Parse(txtMontoInicial.Text);
        double costoTotalMembresia = 1200;

        double saldoRestante = costoTotalMembresia - montoInicial;
        double cuotaBase = saldoRestante / 5;
        double recargo2Porciento = costoTotalMembresia * 0.02;
        double adicionalCasillero = swCasillero.IsToggled ? 10 : 0;

        pagoMensualCalculado = cuotaBase + recargo2Porciento + adicionalCasillero;
        pagoTotalCalculado = montoInicial + (pagoMensualCalculado * 5);

        txtPagoMensual.Text = pagoMensualCalculado.ToString("F2");
        esCalculado = true;
    }

    private async void OnVerResumenClicked(object? sender, EventArgs e)
    {
        if (!esCalculado)
        {
            await DisplayAlertAsync("Atención", "Primero debe calcular el pago mensual.", "OK");
            return;
        }

        // Los datos personales pudieron cambiar después de calcular
        if (!await ValidarCampos()) return;

        await Navigation.PushAsync(new Resumen(
            usuarioConectado,
            txtNombre.Text.Trim(),
            txtApellido.Text.Trim(),
            txtEdad.Text.Trim(),
            (dpFechaInicio.Date ?? DateTime.Today).ToString("dd/MM/yyyy"),
            pkPlan.SelectedItem?.ToString() ?? string.Empty,
            swCasillero.IsToggled ? "Sí" : "No",
            txtMontoInicial.Text.Trim(),
            pagoMensualCalculado.ToString("F2"),
            pagoTotalCalculado.ToString("F2")
        ));
    }
}
