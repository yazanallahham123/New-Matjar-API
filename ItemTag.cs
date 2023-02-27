using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class ItemTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int TagId { get; set; }
        [Required]
        public int ItemId { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual Item Item { get; set; }

        

    }
}
