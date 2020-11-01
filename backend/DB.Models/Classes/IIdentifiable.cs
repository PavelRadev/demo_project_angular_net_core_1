using System;

namespace DB.Models.Classes
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}