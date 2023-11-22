using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BookStoreAPI.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly IMongoCollection<AppRole> _roleCollection;
        private readonly IMongoCollection<AppUserRole> _appUserRoleCollection;
        private readonly IMapper _mapper;

        public RoleManager(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //MongoDb ile ConnectionString elaqesini qururuq
            var client = new MongoClient(databaseSettings.ConnectionString);
            //MongoDb ile DatabaseName elaqesini qururuq
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _roleCollection = database.GetCollection<AppRole>(databaseSettings.RoleCollectionName);
            _appUserRoleCollection = database.GetCollection<AppUserRole>(databaseSettings.AppUserRoleCollectionName);
            _mapper = mapper;
        }

        public async Task<IResult> AddRoleWithUser(string userId, Guid roleId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || roleId == Guid.Empty)
                    return new ErrorResult("User ID and Role ID cannot be null or empty");

                var role = await _roleCollection.Find(x => x.Id == roleId).FirstOrDefaultAsync();
                if (role == null)
                    return new ErrorResult($"Role with ID '{roleId}' not found");

                var userRoleExists = await _appUserRoleCollection
                    .Find(ur => ur.UserId == userId && ur.RoleId == role.Id)
                    .AnyAsync();

                if (userRoleExists)
                    return new ErrorResult($"User already assigned to the role with ID '{roleId}'");

                var userRole = new AppUserRole
                {
                    UserId = userId,
                    RoleId = role.Id
                };

                await _appUserRoleCollection.InsertOneAsync(userRole);
                return new SuccessResult($"User added to the role with ID '{roleId}' successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while adding the user to the role: {ex.Message}");
            }
        }


        public async Task<IResult> CreateRole(AppRole role)
        {
            try
            {
                if (role == null)
                    return new ErrorResult("Role data is null");

                if (string.IsNullOrWhiteSpace(role.RoleName))
                    return new ErrorResult("RoleName cannot be empty");

                var existingRole = await _roleCollection.Find(x => x.RoleName == role.RoleName).FirstOrDefaultAsync();
                if (existingRole != null)
                    return new ErrorResult($"Role with the same name '{role.RoleName}' already exists");


                await _roleCollection.InsertOneAsync(role);
                return new SuccessResult("Role created successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while creating the role: {ex.Message}");
            }
        }


        public async Task<IResult> DeleteRole(Guid roleId)
        {
            try
            {
                if (roleId == Guid.Empty)
                    return new ErrorResult("RoleId cannot be empty");

                var existingRole = await _roleCollection.Find(x => x.Id == roleId).FirstOrDefaultAsync();
                if (existingRole == null)
                    return new ErrorResult($"Role with ID '{roleId}' not found");

                var usersWithRole = await _appUserRoleCollection.Find(x => x.RoleId == roleId).AnyAsync();
                if (usersWithRole)
                    return new ErrorResult($"Role with ID '{roleId}' cannot be deleted because there are users associated with this role");

                var deleteResult = await _roleCollection.DeleteOneAsync(x => x.Id == roleId);
                if (deleteResult.DeletedCount > 0)
                    return new SuccessResult("Role deleted successfully");

                return new ErrorResult("Role not found or cannot be deleted");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while deleting the role: {ex.Message}");
            }
        }

        public async Task<IResult> DeleteRoleWithUser(string userId, Guid roleId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || roleId == Guid.Empty)
                    return new ErrorResult("UserId and RoleId cannot be empty");

                var existingRole = await _roleCollection.Find(x => x.Id == roleId).FirstOrDefaultAsync();
                if (existingRole == null)
                    return new ErrorResult($"Role with ID '{roleId}' not found");

                var deleteResult = await _appUserRoleCollection.DeleteOneAsync(x => x.UserId == userId && x.RoleId == roleId);
                if (deleteResult.DeletedCount > 0)
                    return new SuccessResult($"User removed from the role with ID '{roleId}' successfully");

                return new ErrorResult($"User not found in the role with ID '{roleId}' or cannot be removed");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while removing the user from the role: {ex.Message}");
            }
        }


        public async Task<IDataResult<List<AppRole>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleCollection.Find(r => true).ToListAsync();
                return new SuccessDataResult<List<AppRole>>(roles, "Roles retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<AppRole>>($"An error occurred while getting all roles: {ex.Message}");
            }
        }

        public async Task<IResult> UpdateRole(AppRole role)
        {
            try
            {
                if (role == null)
                    return new ErrorResult("Role data is null");

                var existingRole = await _roleCollection.Find(x => x.Id == role.Id).FirstOrDefaultAsync();
                if (existingRole == null)
                    return new ErrorResult($"Role with ID '{role.Id}' not found");

                var updateResult = await _roleCollection.ReplaceOneAsync(x => x.Id == role.Id, role);
                if (updateResult.ModifiedCount > 0)
                    return new SuccessResult("Role updated successfully");

                return new ErrorResult("Role not found or cannot be updated");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while updating the role: {ex.Message}");
            }
        }

    }
}
