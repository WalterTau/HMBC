using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IShipsService
    {
        Task<Ship> Add(Ship model);
        Task<IEnumerable<Ship>> GetAllBoats();
        Task<Ship> GetById(string Id);
        Task<Ship> Delete();
        Task<Ship> Update();
    } 
}
