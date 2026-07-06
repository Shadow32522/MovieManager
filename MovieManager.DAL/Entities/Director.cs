using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MovieManager.DAL.Entities
{
    public class Director
    {
        public int Id {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateOnly? BirthDate {  get; set; }
        public string? Country {  get; set; }
        public string? Biography {  get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
