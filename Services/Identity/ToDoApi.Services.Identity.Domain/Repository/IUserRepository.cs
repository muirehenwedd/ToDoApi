using System;
using ToDoApi.Common.Domain;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Domain.Repository;

public interface IUserRepository : IRepository<User, Guid>
{
}