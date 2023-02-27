using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
        [Required]
        public bool Disabled { get; set; }
        public byte[] StoredSalt { get; set; }
        public string FirebaseToken { get; set; }
        [Required]
        public int Language { get; set; }
        [Required]
        public int RoleId { get; set; }
        public virtual UserRole Role { get; set; }
    }
}
