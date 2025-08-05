using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.Enitity
{
    public class MenuAdminUser
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    [ForeignKey("MenuAdmin")]
    public int MenuAdminId { get; set; }

    public MenuAdmin MenuAdmin { get; set; }



    }
}
