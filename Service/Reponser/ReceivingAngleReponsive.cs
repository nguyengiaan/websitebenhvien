using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class ReceivingAngleReponsive : IReceivingAngle
    {
        private readonly MyDbcontext _myDbcontext;
        private readonly IMapper _mapper;
        private readonly Uploadfile _uploadfile;

        public ReceivingAngleReponsive(MyDbcontext myDbcontext, IMapper mapper, Uploadfile uploadfile)
        {
            // Constructor logic if needed

            // Initialize the database context and mapper
            _myDbcontext = myDbcontext;
            _mapper = mapper;
            _uploadfile = uploadfile;
        }
        public async Task<bool> CreateReceivingAngleAsync(ReceivingAngleVM receivingAngle)
        {
            try
            {
                if (receivingAngle.formFile != null)
                {
                    var uploadResult = await _uploadfile.SaveMedia(receivingAngle.formFile);
                    if (uploadResult.Item1==1)
                    {
                        receivingAngle.image = uploadResult.Item2;
                    }
                    else
                    {
                        return false;
                    }   
                   
                }
                var entity = _mapper.Map<ReceivingAngle>(receivingAngle);
                await _myDbcontext.Receivingangles.AddAsync(entity);
                await _myDbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // Log the exception or handle it as needed
            }
        }

       

        public async Task<bool> DeleteReceivingAngleAsync(int id)
        {
            try
            {
                var entity = await _myDbcontext.Receivingangles.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                // Delete associated image file if exists
                if (!string.IsNullOrEmpty(entity.image))
                {
                    _uploadfile.DeleteMedia(entity.image);
                }

                _myDbcontext.Receivingangles.Remove(entity);
                await _myDbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Log the exception or handle it as needed
            }
        }

        public async Task<List<ReceivingAngleVM>> GetAllReceivingAnglesAsync(int page, int pageSize, string searchString)
        {
            try
            {
                var query = _myDbcontext.Receivingangles.AsQueryable();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(x => x.title.Contains(searchString) || 
                                           (x.description != null && x.description.Contains(searchString)));
                }

                // Apply pagination
                var entities = await query
                    .OrderByDescending(x => x.created_at)
                    .ThenBy(x => x.order)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<List<ReceivingAngleVM>>(entities);
            }
            catch
            {
                return new List<ReceivingAngleVM>(); // Return empty list on error
            }
        }

        public async Task<ReceivingAngleVM?> GetReceivingAngleByIdAsync(int id)
        {
            try
            {
                var entity = await _myDbcontext.Receivingangles.FindAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<ReceivingAngleVM>(entity);
            }
            catch
            {
                return null; // Return null on error
            }
        }

        public async Task<bool> UpdateReceivingAngleAsync(ReceivingAngleVM receivingAngle)
        {
            try
            {
                var existingEntity = await _myDbcontext.Receivingangles.FindAsync(receivingAngle.id);
                if (existingEntity == null)
                {
                    return false;
                }

                // Handle image upload if new file is provided
                if (receivingAngle.formFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingEntity.image))
                    {
                        _uploadfile.DeleteMedia(existingEntity.image);
                    }

                    // Upload new image
                    var uploadResult = await _uploadfile.SaveMedia(receivingAngle.formFile);
                    if (uploadResult.Item1 == 1)
                    {
                        receivingAngle.image = uploadResult.Item2;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Keep existing image if no new file uploaded
                    receivingAngle.image = existingEntity.image;
                }

                // Map updated values to existing entity
                _mapper.Map(receivingAngle, existingEntity);
                
                // Update the modified date (you might want to add this field to your model)
                // existingEntity.updated_at = DateTime.Now;

                await _myDbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Log the exception or handle it as needed
            }
        }

        // Additional helper method to get total count for pagination
        public async Task<int> GetTotalCountAsync(string? searchString = null)
        {
            try
            {
                var query = _myDbcontext.Receivingangles.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(x => x.title.Contains(searchString) || 
                                           (x.description != null && x.description.Contains(searchString)));
                }

                return await query.CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<ReceivingAngleVM>> GetAllReceivingAnglesAsync()
        {
            try
            {
                var entities = await _myDbcontext.Receivingangles.ToListAsync();
                return _mapper.Map<List<ReceivingAngleVM>>(entities);
            }
            catch
            {
                return new List<ReceivingAngleVM>(); // Return empty list on error
            }
        }
    }
}