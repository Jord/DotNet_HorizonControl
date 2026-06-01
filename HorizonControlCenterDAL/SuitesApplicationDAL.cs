using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL
{
    public class SuitesApplicationDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public SuitesApplicationDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<SuitesApplicationModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await _context.SuitesApplications.AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return _mapper.Map<List<SuitesApplicationModel>>(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<SuitesApplicationModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await _context.SuitesApplications
                            .FirstOrDefaultAsync(j => j.GuidId == id);

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity == null ? null : _mapper.Map<SuitesApplicationModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<(SuitesApplicationModel result, string message)> CreateAsync(SuitesApplicationDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var nameExists = model.ApplicationName != null &&
             await _context.SuitesApplications.AnyAsync(s => s.SuiteApplicationName == model.ApplicationName);

                if (nameExists)
                {
                    string message = "Application Name already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message);
                }

                var entity = _mapper.Map<SuitesApplication>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.IsActive = true;
                entity.CreateByUserId = userId;
                entity.LastUpdatedByUserId = userId;

                await _context.SuitesApplications.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (_mapper.Map<SuitesApplicationModel>(entity), "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<SuitesApplicationModel> UpdateAsync(Guid id, SuitesApplicationDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.SuitesApplications
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

                return _mapper.Map<SuitesApplicationModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<SuitesApplicationModel> PatchAsync(Guid id, JsonPatchDocument<SuitesApplication> patchDoc, int userId)
        {
            const string methodName = nameof(PatchAsync);

            try
            {
                var entity = await _context.SuitesApplications
                    .FirstOrDefaultAsync(j => j.GuidId == id);

                if (entity == null)
                    throw new Exception("Record not found");

                patchDoc.ApplyTo(entity);

                entity.LastUpdatedByUserId = userId;
                entity.LastUpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return _mapper.Map<SuitesApplicationModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            const string methodName = nameof(DeleteAsync);

            try
            {
                var entity = await _context.SuitesApplications
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                entity.IsActive = false;
                entity.LastUpdatedDate = DateTime.UtcNow;

                _context.SuitesApplications.Update(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Soft deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

                return 1; // Success
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<List<string>> GetDistinctApplicationTypesAsync()
        {
            const string methodName = nameof(GetDistinctApplicationTypesAsync);
            try
            {
                var applicationTypes = Enum.GetValues<HorizonControlCenterModels.Enums.ApplicationType>()
                    .Select(e => e.ToString())
                    .ToList();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return await Task.FromResult(applicationTypes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }
    }
}
