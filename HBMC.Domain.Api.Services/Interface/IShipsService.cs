using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IShipsService
    {
        Task<Ships> Add(Ships model);
        Task<IEnumerable<Ships>> GetAllBoats();
        Task<Ships> GetById(string Id);
        Task<Ships> Delete();
        Task<Ships> Update(string Id);
    } 
}
