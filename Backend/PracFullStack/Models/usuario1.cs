using System;
using System.Collections.Generic;

namespace PracFullStack.Models;

public partial class usuario1
{
    public int id_usuario { get; set; }

    public string username { get; set; } = null!;

    public string password { get; set; } = null!;

    public string rol { get; set; } = null!;

    public bool? active { get; set; }
}
