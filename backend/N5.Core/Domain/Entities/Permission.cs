using N5.Core.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N5.Core.Domain.Entities
{
    public class Permission : Entity
    {

        [Required]
        public string NombreEmpleado { get; set; }

        [Required]
        public string ApellidoEmpleado { get; set; }

        [Required]
        public int TipoPermiso { get; set; }

        [ForeignKey(nameof(TipoPermiso))]
        public PermissionType Tipo { get; set; }

        [Required]
        public DateTime FechaPermiso { get; set; }
    }
}