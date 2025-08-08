using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class PaginatedReceivingAngleVM
    {
        public List<ReceivingAngleVM> Items { get; set; } = new List<ReceivingAngleVM>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        
        public int StartPage
        {
            get
            {
                if (TotalPages <= 5)
                    return 1;
                
                if (CurrentPage <= 3)
                    return 1;
                
                if (CurrentPage + 2 >= TotalPages)
                    return TotalPages - 4;
                
                return CurrentPage - 2;
            }
        }
        
        public int EndPage
        {
            get
            {
                if (TotalPages <= 5)
                    return TotalPages;
                
                if (CurrentPage <= 3)
                    return 5;
                
                if (CurrentPage + 2 >= TotalPages)
                    return TotalPages;
                
                return CurrentPage + 2;
            }
        }
    }
}
