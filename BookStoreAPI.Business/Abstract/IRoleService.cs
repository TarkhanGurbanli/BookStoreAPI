using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Concrete;

namespace BookStoreAPI.Business.Abstract
{
    public interface IRoleService
    {
        Task<IResult> CreateRole(AppRole role);
        Task<IResult> DeleteRole(Guid roleId);
        Task<IResult> UpdateRole(AppRole role);
        Task<IDataResult<List<AppRole>>> GetAllRoles();
        Task<IResult> AddRoleWithUser(string userId, Guid roleId);
        Task<IResult> DeleteRoleWithUser(string userId, Guid roleId);
    }
}
