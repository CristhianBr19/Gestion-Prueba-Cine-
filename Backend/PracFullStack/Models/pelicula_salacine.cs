using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracFullStack.Models;

public partial class pelicula_salacine
{
    [Key]
    public int id { get; set; }

    [Required]
    public int? id_pelicula { get; set; }

    [Required]
    public int? id_sala { get; set; }


    public DateOnly fecha_publicacion { get; set; }

    public DateOnly? fecha_fin { get; set; }

    public bool? active { get; set; }

    [ForeignKey("id_pelicula")]
    public virtual pelicula? id_peliculaNavigation { get; set; }
    [ForeignKey("id_sala")]
    public virtual sala_cine? id_salaNavigation { get; set; }
}
