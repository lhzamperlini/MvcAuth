﻿using Microsoft.EntityFrameworkCore;
using MvcAuth.Domain.Interfaces.Repositories;
using MvcAuth.Domain.Models;
using MvcAuth.Repository.Data;
using MvcAuth.Repository.Repositories.common;

namespace MvcAuth.Repository.Repositories;
public class UsuarioRepository : CrudRepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(MainDbContext context) : base(context)
    {
    }

    public async Task<bool> ObterPorEmail(string email) => await Query().AnyAsync(x => x.Email.Equals(email));
    
}
