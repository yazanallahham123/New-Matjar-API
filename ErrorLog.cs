using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace API
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public string? InnerMessage { get; set; }
        public string? Code { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
    }
}
