using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public partial class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public bool Disabled { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }

        public virtual Type Type { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<ItemTag> ItemTags { get; set; }
        public ICollection<ItemVariation> ItemVariations { get; set; }
    }
}
