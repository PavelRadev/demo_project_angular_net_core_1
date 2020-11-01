using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models.Classes
{
    public class SoftDeletable: ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedByUserId { get; set; }
        
        [ForeignKey("DeletedByUserId")]
        public User DeletedByUser { get; set; }
    }
}