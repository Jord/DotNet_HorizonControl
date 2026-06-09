using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL
{
    public class UserDAL : IUserDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public UserDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await _context.Users.AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return _mapper.Map<List<UserModel>>(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await _context.Users
                            .FirstOrDefaultAsync(j => j.GuidId == id);

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity == null ? null : _mapper.Map<UserModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserModel?> GetExistingUserAsync(UserDTO model)
        {
            const string methodName = nameof(GetExistingUserAsync);
            try
            {
                var existingEntity = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => 
                        (model.WindowsUserName != null && s.WindowsUserName == model.WindowsUserName) ||
                        (model.Email != null && s.Email == model.Email));

                if (existingEntity != null)
                {
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Existing User found at {Time}", DateTime.Now);
                }

                return existingEntity == null ? null : _mapper.Map<UserModel>(existingEntity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<(UserModel result, string message)> CreateAsync(UserDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var usernameExists = model.WindowsUserName != null &&
                    await _context.Users.AnyAsync(s => s.WindowsUserName == model.WindowsUserName);

                var emailExists = model.Email != null &&
                    await _context.Users.AnyAsync(s => s.Email == model.Email);

                if (usernameExists || emailExists)
                {
                    string message;

                    if (usernameExists && emailExists)
                        message = "Username and Email already exist";
                    else if (usernameExists)
                        message = "Username already exists";
                    else
                        message = "Email already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message);
                }

                var entity = _mapper.Map<User>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.CreatedByUserId = userId;
                entity.LastUpdatedByUserId = userId;
                entity.IsActive = true;

                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (_mapper.Map<UserModel>(entity), "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserModel> UpdateAsync(Guid id, UserDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.Users
                    .FirstOrDefaultAsync(j => j.GuidId == id);

                if (entity == null)
                    throw new Exception("Record not found");

                if (model != null)
                {
                    _mapper.Map(model, entity);
                }

                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.LastUpdatedByUserId = userId;

                await _context.SaveChangesAsync();

                return _mapper.Map<UserModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);

            try
            {
                var entity = await _context.Users
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Soft delete
                entity.IsActive = false;
                entity.LastUpdatedByUserId = userId;
                entity.LastUpdatedDate = DateTime.UtcNow;

                _context.Users.Update(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Soft deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

                return 1; // Success
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);

            try
            {
                var entity = await _context.Users
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Permanent delete
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Permanently deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

                return 1; // Success
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
    }
}
