using System;
using System.Collections.Generic;

namespace MiEcommApp.Models;

public partial class Permiso
{
    public int Idpermiso { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Employee> Emails { get; set; } = new List<Employee>();
}
