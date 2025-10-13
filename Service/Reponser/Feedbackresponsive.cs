using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Migrations;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class Feedbackresponsive : IFeedback
    {
        private readonly MyDbcontext _context;
        private readonly Uploadfile _uploadfile;

        public Feedbackresponsive(MyDbcontext context,Uploadfile uploadfile)
        {
            _context= context;
             _uploadfile = uploadfile;
        }
        public async Task<(string Message, bool Success)> AddFeedbackAsync(FeedbackVM model)
        {
            try
            {
                if (model.Id.HasValue)
                {
                    return await UpdateExistingFeedbackAsync(model);
                }
                else
                {
                    return await CreateNewFeedbackAsync(model);
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return (ex.Message, false);
            }
        }
        public async Task<(List<FeedbackVM> ds, int TotalPages)> GetAllFeedbacksAsync(string search, int page, int pagesize)
        {
            try
            {
                var query = _context.Feedbacks.AsNoTracking().AsQueryable();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(f => f.Title_Name.Contains(search) || 
                                           f.Content.Contains(search));
                }

                // Get total count for pagination calculation
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);

                // Apply pagination and select only needed fields for performance
                var feedbacks = await query
                    .OrderByDescending(f => f.Id)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(f => new FeedbackVM
                    {
                        Id = f.Id,
                        Title_Name = f.Title_Name,
                        Content = f.Content,
                        Thumnail = f.Thumnail
                    })
                    .ToListAsync();

                return (feedbacks, totalPages);
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return (new List<FeedbackVM>(), 0);
            }
        }
      
        private async Task<(string Message, bool Success)> UpdateExistingFeedbackAsync(FeedbackVM model)
        {
            var existingFeedback = await _context.Feedbacks.FindAsync(model.Id);
            if (existingFeedback == null)
            {
                return ("Không tìm thấy phản hồi để cập nhật", false);
            }

            existingFeedback.Title_Name = model.Title_Name;
            existingFeedback.Content = model.Content;

            if (model.formFile != null)
            {
                var fileResult = await _uploadfile.SaveMedia(model.formFile);
                if (fileResult.Item1 == 1)
                {
                    _uploadfile.DeleteMedia(model.Thumnail);
                    existingFeedback.Thumnail = fileResult.Item2;
                }
            }

            await _context.SaveChangesAsync();
            return ("Cập nhật thành công", true);
        }

        private async Task<(string Message, bool Success)> CreateNewFeedbackAsync(FeedbackVM model)
        {
            var newFeedback = new Feedback
            {
                Title_Name = model.Title_Name,
                Content = model.Content
            };

            if (model.formFile != null)
            {
                var fileResult = await _uploadfile.SaveMedia(model.formFile);
                if (fileResult.Item1 == 1)
                {
                    newFeedback.Thumnail = fileResult.Item2;
                }
            }

            await _context.Feedbacks.AddAsync(newFeedback);
            await _context.SaveChangesAsync();
            return ("Thêm thành công", true);
        }

        public async Task<FeedbackVM> GetFeedbackByIdAsync(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.AsNoTracking()
                    .Where(f => f.Id == id)
                    .Select(f => new FeedbackVM
                    {
                        Id = f.Id,
                        Title_Name = f.Title_Name,
                        Content = f.Content,
                        Thumnail = f.Thumnail
                    })
                    .FirstOrDefaultAsync();

                return feedback;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(string Message, bool Success)> UpdateFeedbackAsync(FeedbackVM model)
        {
            try
            {
                return await UpdateExistingFeedbackAsync(model);
            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            try
            {
                var data = await _context.Feedbacks.FindAsync(id);
                if (data == null) return false;
                _uploadfile.DeleteMedia(data.Thumnail);
                _context.Feedbacks.Remove(data);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
         
        }
    }
}
