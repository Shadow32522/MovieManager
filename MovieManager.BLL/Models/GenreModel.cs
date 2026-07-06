using Microsoft.EntityFrameworkCore.Metadata;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Models
{
    public class GenreModel : IModelWithId
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
