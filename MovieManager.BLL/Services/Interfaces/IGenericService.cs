using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Services.Interfaces
{
    public interface IGenericService<TModel> where TModel : class
    {
        Task<TModel?> GetByIdAsync(int id);
        Task<IReadOnlyList<TModel>> GetAllAsync();
        Task<TModel> CreateAsync(TModel model);
        Task<bool> UpdateAsync(TModel model);
        Task<bool> DeleteAsync(int id);
    }
}
