using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public partial class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public string EnglishName { get; set; }
        public virtual Category ParentCategory { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}
