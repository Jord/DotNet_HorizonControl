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
    public class SuiteDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public SuiteDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<SuiteModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await _context.Suites.AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return _mapper.Map<List<SuiteModel>>(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }

        }

        public async Task<SuiteModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await _context.Suites
                            .FirstOrDefaultAsync(j => j.GuidId == id); // ✅ FIX

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity == null ? null : _mapper.Map<SuiteModel>(entity);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }

        }

        //public async Task<SuiteModel> CreateAsync(SecurityPostModel model, int userId)
        //{
        //    const string methodName = nameof(CreateAsync);
        //    try
        //    {
        //        // ✅ Check for duplicate: if a record with the same SuiteName or SuiteCode already exists, skip the save
        //        bool isDuplicate = await _context.Suites.AnyAsync(s =>
        //            (model.SuiteName != null && s.SuiteName == model.SuiteName) ||
        //            (model.SuiteCode != null && s.SuiteCode == model.SuiteCode));

        //        if (isDuplicate)
        //        {
        //            Log.ForContext("classname", _className)
        //               .ForContext("method_name", methodName)
        //               .ForContext("user_id", userId)
        //               .ForContext("thrown_by", "Application")
        //               .Warning("Duplicate record detected in {MethodName}. Save operation skipped at {Time}", methodName, DateTime.Now);

        //            var existing = await _context.Suites.AsNoTracking().FirstOrDefaultAsync(s =>
        //                (model.SuiteName != null && s.SuiteName == model.SuiteName) ||
        //                (model.SuiteCode != null && s.SuiteCode == model.SuiteCode));

        //            return _mapper.Map<SuiteModel>(existing);
        //        }

        //        var entity = _mapper.Map<Suite>(model);

        //        entity.CreationDate = DateTime.UtcNow;
        //        entity.LastUpdatedDate = DateTime.UtcNow;
        //        //entity.CreatedByUserId = userId;
        //        //entity.LastUpdatedByUserId = userId;

        //        await _context.Suites.AddAsync(entity);
        //        await _context.SaveChangesAsync();               

        //        Log.ForContext("classname", _className)
        //           .ForContext("method_name", methodName)
        //           .ForContext("id", entity.Id)
        //           .ForContext("user_id", userId)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);
        //        return _mapper.Map<SuiteModel>(entity); // ✅ return full model
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ForContext("classname", _className)
        //       .ForContext("method_name", methodName)              
        //       .ForContext("user_id", userId)
        //       .Error(ex, "Error occurred in {MethodName}", methodName);
        //        throw;
        //    }

        //}
        public async Task<SuiteModel> PatchAsync(Guid id, JsonPatchDocument<Suite> patchDoc, int userId)
        {
            const string methodName = nameof(PatchAsync);

            try
            {
                var entity = await _context.Suites
                    .FirstOrDefaultAsync(j => j.GuidId == id);

                if (entity == null)
                    throw new Exception("Record not found");

                // ✅ Apply patch
                patchDoc.ApplyTo(entity);

                // ✅ audit fields
                entity.LastUpdatedByUserId = userId;
                entity.LastUpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return _mapper.Map<SuiteModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
        public async Task<(SuiteModel result, string message)> CreateAsync(SuitesDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var nameExists = model.SuiteName != null &&
             await _context.Suites.AnyAsync(s => s.SuiteName == model.SuiteName);

                var codeExists = model.SuiteCode != null &&
                    await _context.Suites.AnyAsync(s => s.SuiteCode == model.SuiteCode);

                if (nameExists || codeExists)
                {
                    string message;

                    if (nameExists && codeExists)
                        message = "Suite Name and Suite Code already exist";
                    else if (nameExists)
                        message = "Suite Name already exists";
                    else
                        message = "Suite Code already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message); // ✅ duplicate
                }
                var entity = _mapper.Map<Suite>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.IsActive = true;

                await _context.Suites.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (_mapper.Map<SuiteModel>(entity), "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
        public async Task<SuiteModel> UpdateAsync(Guid id, SuitesDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.Suites
                    .FirstOrDefaultAsync(j => j.GuidId == id);

                if (entity == null)
                    throw new Exception("Record not found");

                // ✅ map updated fields
                if (model != null)
                {
                    _mapper.Map(model, entity);
                }

                entity.LastUpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return _mapper.Map<SuiteModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            const string methodName = nameof(DeleteAsync);

            try
            {
                var entity = await _context.Suites
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ✅ Soft delete
                entity.IsActive = false;
                entity.LastUpdatedDate = DateTime.UtcNow;

                entity.DeActiveRemark = "Deleted";

                _context.Suites.Update(entity);
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
    }
}
