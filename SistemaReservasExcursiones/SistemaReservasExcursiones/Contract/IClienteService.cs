using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Cliente;

namespace SistemaReservasExcursiones.Contract
{
    public interface IClienteService
    {
        ServiceResult CrearCliente(ClienteCreateDto dto);
        ServiceResult ActualizarCliente(int id, ClienteUpdateDto dto);
        ServiceResult EliminarCliente(int id);
        ServiceResult ObtenerClientePorId(int id);
        ServiceResult ObtenerClientes();
    }
}
