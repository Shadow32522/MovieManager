using MovieManager.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Services.Interfaces
{
    public interface IMovieActorService 
    {
        Task<MovieActorModel?> GetByIdsAsync(int movieId, int actorId);
        Task<IReadOnlyList<MovieActorModel>> GetByMovieIdAsync(int movieId);
        Task<MovieActorModel> AddActorToMovieAsync(MovieActorModel model);
        Task<bool> RemoveActorFromMovieAsync(int movieId, int actorId);
    }
}
