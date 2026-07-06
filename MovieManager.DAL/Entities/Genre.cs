using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MovieManager.DAL.Entities
{
    public class Genre
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public string? Description { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
