using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IBoatsService
    {
        Task<Boat> Add(Boat model);
        Task<IEnumerable<Boat>> GetAllBoats();
        Task<Boat> GetById(string Id);
        Task<Boat> Delete();
        Task<Boat> Update();
    }
}
