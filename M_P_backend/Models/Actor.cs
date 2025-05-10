using System;
using System.Collections.Generic;

namespace M_P_backend.Models;

public partial class Actor
{
    public int ActorId { get; set; }

    public string ActorName { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
