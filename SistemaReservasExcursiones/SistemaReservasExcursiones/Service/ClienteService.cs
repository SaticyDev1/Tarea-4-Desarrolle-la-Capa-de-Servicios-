using SistemaReservasExcursiones.Contract;
using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Cliente;
using SistemaReservasExcursiones.Models;

namespace SistemaReservasExcursiones.Service
{
    public class ClienteService : IClienteService
    {
        public ServiceResult CrearCliente(ClienteCreateDto dto)
        {
            var errores = ValidarCliente(dto.Nombre, dto.Documento, dto.Correo, dto.Telefono);

            if (SistemaData.Clientes.Any(c => c.Documento == dto.Documento))
                errores.Add("Ya existe un cliente con ese documento.");

            if (SistemaData.Clientes.Any(c => c.Correo == dto.Correo))
                errores.Add("Ya existe un cliente con ese correo.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo crear el cliente.", errores);

            var cliente = new Cliente
            {
                Id = SistemaData.SiguienteClienteId++,
                Nombre = dto.Nombre,
                Documento = dto.Documento,
                Correo = dto.Correo,
                Telefono = dto.Telefono
            };

            SistemaData.Clientes.Add(cliente);

            return ServiceResult.Ok("Cliente creado correctamente.", ConvertirADto(cliente));
        }

        public ServiceResult ActualizarCliente(int id, ClienteUpdateDto dto)
        {
            var cliente = SistemaData.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return ServiceResult.Fail("Cliente no encontrado.");

            var errores = ValidarCliente(dto.Nombre, dto.Documento, dto.Correo, dto.Telefono);

            if (SistemaData.Clientes.Any(c => c.Documento == dto.Documento && c.Id != id))
                errores.Add("Ya existe otro cliente con ese documento.");

            if (SistemaData.Clientes.Any(c => c.Correo == dto.Correo && c.Id != id))
                errores.Add("Ya existe otro cliente con ese correo.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo actualizar el cliente.", errores);

            cliente.Nombre = dto.Nombre;
            cliente.Documento = dto.Documento;
            cliente.Correo = dto.Correo;
            cliente.Telefono = dto.Telefono;

            return ServiceResult.Ok("Cliente actualizado correctamente.", ConvertirADto(cliente));
        }

        public ServiceResult EliminarCliente(int id)
        {
            var cliente = SistemaData.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return ServiceResult.Fail("Cliente no encontrado.");

            var tieneReservas = SistemaData.Reservas.Any(r => r.ClienteId == id && !r.Cancelada);

            if (tieneReservas)
                return ServiceResult.Fail("No se puede eliminar el cliente porque tiene reservas activas.");

            SistemaData.Clientes.Remove(cliente);

            return ServiceResult.Ok("Cliente eliminado correctamente.");
        }

        public ServiceResult ObtenerClientePorId(int id)
        {
            var cliente = SistemaData.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return ServiceResult.Fail("Cliente no encontrado.");

            return ServiceResult.Ok("Cliente encontrado.", ConvertirADto(cliente));
        }

        public ServiceResult ObtenerClientes()
        {
            var clientes = SistemaData.Clientes.Select(ConvertirADto).ToList();

            return ServiceResult.Ok("Listado de clientes obtenido correctamente.", clientes);
        }

        private static List<string> ValidarCliente(string nombre, string documento, string correo, string telefono)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(documento))
                errores.Add("El documento del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(correo))
                errores.Add("El correo del cliente es obligatorio.");
            else if (!correo.Contains("@") || !correo.Contains("."))
                errores.Add("El correo del cliente no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(telefono))
                errores.Add("El teléfono del cliente es obligatorio.");

            return errores;
        }

        private static ClienteDto ConvertirADto(Cliente cliente)
        {
            return new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Documento = cliente.Documento,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono
            };
        }
    }
}