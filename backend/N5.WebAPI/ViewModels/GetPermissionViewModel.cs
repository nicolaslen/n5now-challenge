using System;

namespace N5.WebAPI.ViewModels;

public class GetPermissionViewModel 
{
    public int Id { get; set; }
    public string NombreEmpleado { get; set; }
    public string ApellidoEmpleado { get; set; }
    public PermissionTypeViewModel Tipo { get; set; }
    public DateTime? FechaPermiso { get; set; }
}