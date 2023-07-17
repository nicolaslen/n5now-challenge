using System.ComponentModel.DataAnnotations;

namespace N5.WebAPI.ViewModels
{
    public class RequestPermissionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del empleado es requerido.")]
        [StringLength(50)]
        public string NombreEmpleado { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido del empleado es requerido.")]
        [StringLength(50)]
        public string ApellidoEmpleado { get; set; }
        public int Tipo { get; set; }
    }
}
