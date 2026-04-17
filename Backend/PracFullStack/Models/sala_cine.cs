using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace PracFullStack.Models;

public partial class sala_cine
{
    [Key]
    public int id { get; set; }

    [Required(ErrorMessage ="El nombre de sala es requerido")]
    public string nombre { get; set; } = null!;

    [Required]
    public string estado { get; set; } = null!;

    public bool? active { get; set; }

    [NotMapped]
    public string? estadoReal { get; set; }


    [JsonIgnore]

    public virtual ICollection<pelicula_salacine> pelicula_salacines { get; set; } = new List<pelicula_salacine>();
}
