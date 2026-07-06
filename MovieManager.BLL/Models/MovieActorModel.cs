using MovieManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Models
{
    public class MovieActorModel
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
        public string? CharacterName { get; set; }
        public bool IsLeadRole { get; set; }
        public int DisplayOrder { get; set; }
    }
}
