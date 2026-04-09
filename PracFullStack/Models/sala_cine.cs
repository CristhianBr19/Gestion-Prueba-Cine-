using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


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


    [JsonIgnore]

    public virtual ICollection<pelicula_salacine> pelicula_salacines { get; set; } = new List<pelicula_salacine>();
}
