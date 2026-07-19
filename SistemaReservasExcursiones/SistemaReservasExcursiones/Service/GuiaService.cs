using SistemaReservasExcursiones.Contract;
using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Guia;
using SistemaReservasExcursiones.Models;

namespace SistemaReservasExcursiones.Service
{
    public class GuiaService : IGuiaService
    {
        public ServiceResult CrearGuia(GuiaCreateDto dto)
        {
            var errores = ValidarGuia(dto.Nombre, dto.Documento, dto.Correo, dto.Telefono, dto.Especialidad);

            if (SistemaData.Guias.Any(g => g.Documento == dto.Documento))
                errores.Add("Ya existe un guia con ese documento.");

            if (SistemaData.Guias.Any(g => g.Correo == dto.Correo))
                errores.Add("Ya existe un guia con ese correo.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo crear el guia.", errores);

            var guia = new Guia
            {
                Id = SistemaData.SiguienteGuiaId++,
                Nombre = dto.Nombre,
                Documento = dto.Documento,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Especialidad = dto.Especialidad,
                Activo = true
            };

            SistemaData.Guias.Add(guia);

            return ServiceResult.Ok("Guia creado correctamente.", ConvertirADto(guia));
        }

        public ServiceResult ActualizarGuia(int id, GuiaUpdateDto dto)
        {
            var guia = SistemaData.Guias.FirstOrDefault(g => g.Id == id);

            if (guia == null)
                return ServiceResult.Fail("Guia no encontrado.");

            var errores = ValidarGuia(dto.Nombre, dto.Documento, dto.Correo, dto.Telefono, dto.Especialidad);

            if (SistemaData.Guias.Any(g => g.Documento == dto.Documento && g.Id != id))
                errores.Add("Ya existe otro guia con ese documento.");

            if (SistemaData.Guias.Any(g => g.Correo == dto.Correo && g.Id != id))
                errores.Add("Ya existe otro guia con ese correo.");

            if (errores.Any())
                return ServiceResult.Fail("No se pudo actualizar el guia.", errores);

            guia.Nombre = dto.Nombre;
            guia.Documento = dto.Documento;
            guia.Correo = dto.Correo;
            guia.Telefono = dto.Telefono;
            guia.Especialidad = dto.Especialidad;
            guia.Activo = dto.Activo;

            return ServiceResult.Ok("Guia actualizado correctamente.", ConvertirADto(guia));
        }

        public ServiceResult EliminarGuia(int id)
        {
            var guia = SistemaData.Guias.FirstOrDefault(g => g.Id == id);

            if (guia == null)
                return ServiceResult.Fail("Guia no encontrado.");

            var tieneExcursiones = SistemaData.Excursiones.Any(e => e.GuiaId == id && e.Activa);

            if (tieneExcursiones)
                return ServiceResult.Fail("No se puede eliminar el guia porque tiene excursiones activas asignadas.");

            SistemaData.Guias.Remove(guia);

            return ServiceResult.Ok("Guia eliminado correctamente.");
        }

        public ServiceResult ObtenerGuiaPorId(int id)
        {
            var guia = SistemaData.Guias.FirstOrDefault(g => g.Id == id);

            if (guia == null)
                return ServiceResult.Fail("Guia no encontrado.");

            return ServiceResult.Ok("Guia encontrado.", ConvertirADto(guia));
        }

        public ServiceResult ObtenerGuias()
        {
            var guias = SistemaData.Guias.Select(ConvertirADto).ToList();

            return ServiceResult.Ok("Listado de guias obtenido correctamente.", guias);
        }

        private static List<string> ValidarGuia(string nombre, string documento, string correo, string telefono, string especialidad)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add("El nombre del guia es obligatorio.");

            if (string.IsNullOrWhiteSpace(documento))
                errores.Add("El documento del guia es obligatorio.");

            if (string.IsNullOrWhiteSpace(correo))
                errores.Add("El correo del guia es obligatorio.");
            else if (!correo.Contains("@") || !correo.Contains("."))
                errores.Add("El correo del guia no tiene un formato valido.");

            if (string.IsNullOrWhiteSpace(telefono))
                errores.Add("El telefono del guia es obligatorio.");

            if (string.IsNullOrWhiteSpace(especialidad))
                errores.Add("La especialidad del guia es obligatoria.");

            return errores;
        }

        private static GuiaDto ConvertirADto(Guia guia)
        {
            return new GuiaDto
            {
                Id = guia.Id,
                Nombre = guia.Nombre,
                Documento = guia.Documento,
                Correo = guia.Correo,
                Telefono = guia.Telefono,
                Especialidad = guia.Especialidad,
                Activo = guia.Activo
            };
        }
    }
}