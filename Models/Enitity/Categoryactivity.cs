using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.Enitity
{
    [Table("Categoryactivity")]
    public class Categoryactivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id_Categoryactivity { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Link_alias { get; set; } = "";
        
        public int Sort { get; set; } = 0;
        
        public bool Status { get; set; } = true;
        
        public DateTime Createat { get; set; } = DateTime.Now;
        
        public DateTime? Updateat { get; set; }
        
        // Navigation properties
        public virtual ICollection<Postactivity>? Postactivities { get; set; }
    }
}