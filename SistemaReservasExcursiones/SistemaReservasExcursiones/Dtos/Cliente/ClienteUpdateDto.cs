namespace SistemaReservasExcursiones.Dtos.Cliente
{
    public class ClienteUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}