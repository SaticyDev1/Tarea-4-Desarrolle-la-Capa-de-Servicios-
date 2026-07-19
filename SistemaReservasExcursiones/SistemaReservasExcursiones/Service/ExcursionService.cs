using SistemaReservasExcursiones.Contract;
using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Excursion;
using SistemaReservasExcursiones.Models;

namespace SistemaReservasExcursiones.Service
{
    public class ExcursionService : IExcursionService
    {
        public ServiceResult CrearExcursion(ExcursionCreateDto dto)
        {
            var errores = ValidarExcursion(dto.Nombre, dto.Descripcion, dto.Fecha, dto.Precio, dto.CupoMaximo, dto.GuiaId);

            var guia = SistemaData.Guias.FirstOrDefault(g => g.Id == dto.GuiaId);

            if (guia == null)
                errores.Add("El guía seleccionado no existe.");
            else if (!guia.Activo)
                errores.Add("El guía seleccionado no está activo.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo crear la excursión.", errores);

            var excursion = new Excursion
            {
                Id = SistemaData.SiguienteExcursionId++,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Fecha = dto.Fecha,
                Precio = dto.Precio,
                CupoMaximo = dto.CupoMaximo,
                CuposDisponibles = dto.CupoMaximo,
                GuiaId = dto.GuiaId,
                Activa = true
            };

            SistemaData.Excursiones.Add(excursion);

            return ServiceResult.Ok("Excursión creada correctamente.", ConvertirADto(excursion));
        }

        public ServiceResult ActualizarExcursion(int id, ExcursionUpdateDto dto)
        {
            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == id);

            if (excursion == null)
                return ServiceResult.Fail("Excursión no encontrada.");

            var errores = ValidarExcursion(dto.Nombre, dto.Descripcion, dto.Fecha, dto.Precio, dto.CupoMaximo, dto.GuiaId);

            var guia = SistemaData.Guias.FirstOrDefault(g => g.Id == dto.GuiaId);

            if (guia == null)
                errores.Add("El guía seleccionado no existe.");
            else if (!guia.Activo)
                errores.Add("El guía seleccionado no está activo.");

            var participantesReservados = SistemaData.Reservas
                .Where(r => r.ExcursionId == id && !r.Cancelada)
                .Sum(r => r.CantidadParticipantes);

            if (dto.CupoMaximo < participantesReservados)
                errores.Add("El cupo máximo no puede ser menor a la cantidad de participantes ya reservados.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo actualizar la excursión.", errores);

            excursion.Nombre = dto.Nombre;
            excursion.Descripcion = dto.Descripcion;
            excursion.Fecha = dto.Fecha;
            excursion.Precio = dto.Precio;
            excursion.CupoMaximo = dto.CupoMaximo;
            excursion.CuposDisponibles = dto.CupoMaximo - participantesReservados;
            excursion.GuiaId = dto.GuiaId;
            excursion.Activa = dto.Activa;

            return ServiceResult.Ok("Excursión actualizada correctamente.", ConvertirADto(excursion));
        }

        public ServiceResult EliminarExcursion(int id)
        {
            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == id);

            if (excursion == null)
                return ServiceResult.Fail("Excursión no encontrada.");

            var tieneReservasActivas = SistemaData.Reservas.Any(r => r.ExcursionId == id && !r.Cancelada);

            if (tieneReservasActivas)
                return ServiceResult.Fail("No se puede eliminar la excursión porque tiene reservas activas.");

            SistemaData.Excursiones.Remove(excursion);

            return ServiceResult.Ok("Excursión eliminada correctamente.");
        }

        public ServiceResult ObtenerExcursionPorId(int id)
        {
            var excursion = SistemaData.Excursiones.FirstOrDefault(e => e.Id == id);

            if (excursion == null)
                return ServiceResult.Fail("Excursión no encontrada.");

            return ServiceResult.Ok("Excursión encontrada.", ConvertirADto(excursion));
        }

        public ServiceResult ObtenerExcursiones()
        {
            var excursiones = SistemaData.Excursiones.Select(ConvertirADto).ToList();

            return ServiceResult.Ok("Listado de excursiones obtenido correctamente.", excursiones);
        }

        public ServiceResult ObtenerExcursionesActivas()
        {
            var excursionesActivas = SistemaData.Excursiones
                .Where(e => e.Activa)
                .Select(ConvertirADto)
                .ToList();

            return ServiceResult.Ok("Listado de excursiones activas obtenido correctamente.", excursionesActivas);
        }

        private static List<string> ValidarExcursion(
            string nombre,
            string descripcion,
            DateTime fecha,
            decimal precio,
            int cupoMaximo,
            int guiaId)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add("El nombre de la excursión es obligatorio.");

            if (string.IsNullOrWhiteSpace(descripcion))
                errores.Add("La descripción de la excursión es obligatoria.");

            if (fecha == default)
                errores.Add("La fecha de la excursión es obligatoria.");
            else if (fecha.Date < DateTime.Today)
                errores.Add("La fecha de la excursión no puede ser anterior al día actual.");

            if (precio <= 0)
                errores.Add("El precio de la excursión debe ser mayor que cero.");

            if (cupoMaximo <= 0)
                errores.Add("El cupo máximo debe ser mayor que cero.");

            if (guiaId <= 0)
                errores.Add("Debe seleccionar un guía válido.");

            return errores;
        }

        private static ExcursionDto ConvertirADto(Excursion excursion)
        {
            return new ExcursionDto
            {
                Id = excursion.Id,
                Nombre = excursion.Nombre,
                Descripcion = excursion.Descripcion,
                Fecha = excursion.Fecha,
                Precio = excursion.Precio,
                CupoMaximo = excursion.CupoMaximo,
                CuposDisponibles = excursion.CuposDisponibles,
                GuiaId = excursion.GuiaId,
                Activa = excursion.Activa
            };
        }
    }
}