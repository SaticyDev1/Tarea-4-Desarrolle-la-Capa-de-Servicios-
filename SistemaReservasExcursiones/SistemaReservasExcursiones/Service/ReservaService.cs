using SistemaReservasExcursiones.Contract;
using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Reserva;
using SistemaReservasExcursiones.Models;

namespace SistemaReservasExcursiones.Service
{
    public class ReservaService : IReservaService
    {
        public ServiceResult CrearReserva(ReservaCreateDto dto)
        {
            var errores = new List<string>();

            if (dto.ClienteId <= 0)
                errores.Add("Debe seleccionar un cliente válido.");

            if (dto.ExcursionId <= 0)
                errores.Add("Debe seleccionar una excursión válida.");

            if (dto.CantidadParticipantes <= 0)
                errores.Add("La cantidad de participantes debe ser mayor que cero.");

            var cliente = SistemaData.Clientes.FirstOrDefault(c => c.Id == dto.ClienteId);

            if (cliente == null)
                errores.Add("El cliente seleccionado no existe.");

            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == dto.ExcursionId);

            if (excursion == null)
            {
                errores.Add("La excursión seleccionada no existe.");
            }
            else
            {
                if (!excursion.Activa)
                    errores.Add("La excursión seleccionada no está activa.");

                if (excursion.Fecha.Date < DateTime.Today)
                    errores.Add("No se puede reservar una excursión con fecha pasada.");

                if (dto.CantidadParticipantes > excursion.CuposDisponibles)
                    errores.Add("No hay cupos suficientes para realizar la reserva.");
            }

            if (errores.Any())
                return ServiceResult.Fail("No se pudo crear la reserva.", errores);

            var reserva = new Reserva
            {
                Id = SistemaData.SiguienteReservaId++,
                ClienteId = dto.ClienteId,
                ExcursionId = dto.ExcursionId,
                CantidadParticipantes = dto.CantidadParticipantes,
                MontoTotal = excursion!.Precio * dto.CantidadParticipantes,
                Pagada = dto.Pagada,
                Cancelada = false,
                FechaReserva = DateTime.Now,
                FechaPago = dto.Pagada ? DateTime.Now : null,
                Observacion = dto.Observacion
            };

            excursion.CuposDisponibles -= dto.CantidadParticipantes;

            SistemaData.Reservas.Add(reserva);

            return ServiceResult.Ok("Reserva creada correctamente.", ConvertirADto(reserva));
        }

        public ServiceResult ActualizarReserva(int id, ReservaUpdateDto dto)
        {
            var reserva = SistemaData.Reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
                return ServiceResult.Fail("Reserva no encontrada.");

            if (reserva.Cancelada)
                return ServiceResult.Fail("No se puede actualizar una reserva cancelada.");

            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == reserva.ExcursionId);

            if (excursion == null)
                return ServiceResult.Fail("La excursión de la reserva no existe.");

            var errores = new List<string>();

            if (dto.CantidadParticipantes <= 0)
                errores.Add("La cantidad de participantes debe ser mayor que cero.");

            var diferenciaParticipantes = dto.CantidadParticipantes - reserva.CantidadParticipantes;

            if (diferenciaParticipantes > excursion.CuposDisponibles)
                errores.Add("No hay cupos suficientes para aumentar la cantidad de participantes.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo actualizar la reserva.", errores);

            excursion.CuposDisponibles -= diferenciaParticipantes;

            reserva.CantidadParticipantes = dto.CantidadParticipantes;
            reserva.MontoTotal = excursion.Precio * dto.CantidadParticipantes;
            reserva.Pagada = dto.Pagada;
            reserva.Cancelada = dto.Cancelada;
            reserva.Observacion = dto.Observacion;

            if (dto.Pagada && reserva.FechaPago == null)
                reserva.FechaPago = DateTime.Now;

            if (!dto.Pagada)
                reserva.FechaPago = null;

            if (dto.Cancelada)
                excursion.CuposDisponibles += reserva.CantidadParticipantes;

            return ServiceResult.Ok("Reserva actualizada correctamente.", ConvertirADto(reserva));
        }

        public ServiceResult CancelarReserva(int id)
        {
            var reserva = SistemaData.Reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
                return ServiceResult.Fail("Reserva no encontrada.");

            if (reserva.Cancelada)
                return ServiceResult.Fail("La reserva ya está cancelada.");

            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == reserva.ExcursionId);

            if (excursion != null)
                excursion.CuposDisponibles += reserva.CantidadParticipantes;

            reserva.Cancelada = true;

            return ServiceResult.Ok("Reserva cancelada correctamente.", ConvertirADto(reserva));
        }

        public ServiceResult MarcarReservaComoPagada(int id)
        {
            var reserva = SistemaData.Reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
                return ServiceResult.Fail("Reserva no encontrada.");

            if (reserva.Cancelada)
                return ServiceResult.Fail("No se puede pagar una reserva cancelada.");

            reserva.Pagada = true;
            reserva.FechaPago = DateTime.Now;

            return ServiceResult.Ok("Reserva marcada como pagada correctamente.", ConvertirADto(reserva));
        }

        public ServiceResult ObtenerReservaPorId(int id)
        {
            var reserva = SistemaData.Reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
                return ServiceResult.Fail("Reserva no encontrada.");

            return ServiceResult.Ok("Reserva encontrada.", ConvertirADto(reserva));
        }

        public ServiceResult ObtenerReservas()
        {
            var reservas = SistemaData.Reservas.Select(ConvertirADto).ToList();

            return ServiceResult.Ok("Listado de reservas obtenido correctamente.", reservas);
        }

        private static ReservaDto ConvertirADto(Reserva reserva)
        {
            return new ReservaDto
            {
                Id = reserva.Id,
                ClienteId = reserva.ClienteId,
                ExcursionId = reserva.ExcursionId,
                CantidadParticipantes = reserva.CantidadParticipantes,
                MontoTotal = reserva.MontoTotal,
                Pagada = reserva.Pagada,
                Cancelada = reserva.Cancelada,
                FechaReserva = reserva.FechaReserva,
                FechaPago = reserva.FechaPago,
                Observacion = reserva.Observacion
            };
        }
    }
}