using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookStoreRazor.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Range(0, 30, ErrorMessage = "Can only be between 0-30")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
