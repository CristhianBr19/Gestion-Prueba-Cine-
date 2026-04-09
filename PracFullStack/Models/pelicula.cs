using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PracFullStack.Models;

public partial class pelicula
{

    [Key] 
    public int id { get; set; }

    [Required(ErrorMessage ="El nombre de la pelicula es obligatorio")]
    [StringLength(100,ErrorMessage ="El nombre no puede exeder los 100 caracteres ")]
    public string nombre { get; set; } = null!;

    [Required]
    [Range(1,600, ErrorMessage ="La duracion debe tener entre 1 y 600 minutos")]
    public int duracion { get; set; }

    public bool active { get; set; }

    [Required(ErrorMessage ="La fecha de publicacion es obligatoria")]
    public DateTime? fecha_publicacion { get; set; }


    [JsonIgnore]

    public virtual ICollection<pelicula_salacine> pelicula_salacines { get; set; } = new List<pelicula_salacine>();
}
