using System;

namespace Utils
{
    public interface ISessionContextService
    {
        Guid? CurrentUserId { get; }
    }
}