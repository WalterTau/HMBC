using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IBoatsService
    {
        Task<Boats> Add(Boats model);
        Task<IEnumerable<Boats>> GetAllBoats();
        Task<Boats> GetById(string Id);
        Task<Boats> Delete();
        Task<Boats> Update();
    }
}
