using AutoMapper;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;
using MovieManager.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Services
{
    public class MovieActorService : IMovieActorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        private readonly IGenericRepository<MovieActor> _repository;

        public MovieActorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = _unitOfWork.Repository<MovieActor>();
        }

        public async Task<MovieActorModel?> GetByIdsAsync(int movieId, int actorId)
        {
            var results = await _repository.FindAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);

            if (results == null || results.Count == 0) return null;

            return _mapper.Map<MovieActorModel>(results[0]);
        }

        public async Task<IReadOnlyList<MovieActorModel>> GetByMovieIdAsync(int movieId)
        {
            var results = await _repository.FindAsync(ma => ma.MovieId == movieId);
            return _mapper.Map<IReadOnlyList<MovieActorModel>>(results);
        }

        public async Task<MovieActorModel> AddActorToMovieAsync(MovieActorModel model)
        {
            var entity = _mapper.Map<MovieActor>(model);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MovieActorModel>(entity);
        }

        public async Task<bool> RemoveActorFromMovieAsync(int movieId, int actorId)
        {
            var results = await _repository.FindAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
            if (results == null || results.Count == 0) return false;

            var entity = results[0];
            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
