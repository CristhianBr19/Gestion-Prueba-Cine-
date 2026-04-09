using System;
using System.Collections.Generic;

namespace PracFullStack.Models;

public partial class usuario
{
    public int id_usuario { get; set; }

    public string username { get; set; } = null!;

    public string role { get; set; } = null!;
}
