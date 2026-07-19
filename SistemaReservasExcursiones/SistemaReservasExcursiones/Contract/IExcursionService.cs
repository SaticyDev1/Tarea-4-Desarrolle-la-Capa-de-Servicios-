using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Excursion;

namespace SistemaReservasExcursiones.Contract
{
    public interface IExcursionService
    {
        ServiceResult CrearExcursion(ExcursionCreateDto dto);
        ServiceResult ActualizarExcursion(int id, ExcursionUpdateDto dto);
        ServiceResult EliminarExcursion(int id);
        ServiceResult ObtenerExcursionPorId(int id);
        ServiceResult ObtenerExcursiones();
        ServiceResult ObtenerExcursionesActivas();
    }
}