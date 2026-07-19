using SistemaReservasExcursiones.Dtos.Cliente;
using SistemaReservasExcursiones.Dtos.Excursion;
using SistemaReservasExcursiones.Dtos.Guia;
using SistemaReservasExcursiones.Dtos.Reserva;
using SistemaReservasExcursiones.Service;

Console.WriteLine("SISTEMA DE RESERVAS PARA EXCURSIONES");
Console.WriteLine("------------------------------------");

var clienteService = new ClienteService();
var guiaService = new GuiaService();
var excursionService = new ExcursionService();
var reservaService = new ReservaService();

var resultadoGuia = guiaService.CrearGuia(new GuiaCreateDto
{
    Nombre = "Carlos Medina",
    Documento = "001-1234567-8",
    Correo = "carlos.medina@email.com",
    Telefono = "809-555-1234",
    Especialidad = "Excursiones de montaña"
});

MostrarResultado(resultadoGuia);

var resultadoCliente = clienteService.CrearCliente(new ClienteCreateDto
{
    Nombre = "Ana Pérez",
    Documento = "402-9876543-1",
    Correo = "ana.perez@email.com",
    Telefono = "829-555-9876"
});

MostrarResultado(resultadoCliente);

var resultadoExcursion = excursionService.CrearExcursion(new ExcursionCreateDto
{
    Nombre = "Excursión a Isla Saona",
    Descripcion = "Viaje turístico con transporte, comida y guía incluido.",
    Fecha = DateTime.Today.AddDays(10),
    Precio = 2500,
    CupoMaximo = 20,
    GuiaId = 1
});

MostrarResultado(resultadoExcursion);

var resultadoReserva = reservaService.CrearReserva(new ReservaCreateDto
{
    ClienteId = 1,
    ExcursionId = 1,
    CantidadParticipantes = 3,
    Pagada = false,
    Observacion = "Cliente solicita asiento delantero."
});

MostrarResultado(resultadoReserva);

var resultadoPago = reservaService.MarcarReservaComoPagada(1);
MostrarResultado(resultadoPago);

var resultadoReservas = reservaService.ObtenerReservas();
MostrarResultado(resultadoReservas);

Console.WriteLine();
Console.WriteLine("Prueba finalizada. Presiona cualquier tecla para salir.");
Console.ReadKey();

static void MostrarResultado(dynamic resultado)
{
    Console.WriteLine();
    Console.WriteLine($"Mensaje: {resultado.Message}");
    Console.WriteLine($"Correcto: {resultado.Success}");

    if (resultado.Errors != null && resultado.Errors.Count > 0)
    {
        Console.WriteLine("Errores:");

        foreach (var error in resultado.Errors)
        {
            Console.WriteLine($"- {error}");
        }
    }
}