using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DB.Models.Classes;
using Newtonsoft.Json;

namespace DB.Models
{
    [Table("Users")]
    public class User : SoftDeletable, IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [JsonIgnore]
        public string HashedPassword { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public bool IsGlobalAdmin { get; set; }

        public string FullName => string.Concat(FirstName, " ", LastName);
    }
}