using System;

namespace DB.Models.Classes
{
    public interface ISoftDeletable
    {
        DateTime? DeletedAt { get; set; }
        Guid? DeletedByUserId { get; set; }
    }
}