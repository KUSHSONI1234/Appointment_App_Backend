using System;
using System.ComponentModel.DataAnnotations;

namespace RegisterAPI.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }
        // public byte[]? ProfileImage { get; set; }
    }
}
