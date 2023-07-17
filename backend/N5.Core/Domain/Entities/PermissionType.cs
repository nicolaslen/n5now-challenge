using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using N5.Core.Domain.Entities.Base;

namespace N5.Core.Domain.Entities;

public class PermissionType : Entity
{
    [Required]
    public string Description { get; set; }
}