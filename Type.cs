using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Type
    {
        public Type()
        {
            Items = new HashSet<Item>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public string EnglishName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; }
        
    }
}
