using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Guia;

namespace SistemaReservasExcursiones.Contract
{
    public interface IGuiaService
    {
        ServiceResult CrearGuia(GuiaCreateDto dto);
        ServiceResult ActualizarGuia(int id, GuiaUpdateDto dto);
        ServiceResult EliminarGuia(int id);
        ServiceResult ObtenerGuiaPorId(int id);
        ServiceResult ObtenerGuias();
    }
}