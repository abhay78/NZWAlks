using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> Createasync(Walk walk);
        Task<List<Walk>> GetWAlkAsync(string? filterOn=null,string? filterQuery=null,string? sortBy=null,bool isAscending=true,int pageNumber=1,int pageSize=100);
        Task<Walk> GetByIdAsync(Guid id);
        Task<Walk>UpdateAsync(Guid id,Walk walk);
        Task<Walk> DeleteAsync(Guid id);
    }
    
}
