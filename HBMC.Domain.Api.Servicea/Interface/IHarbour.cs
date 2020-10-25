using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IHarbour
    {
        Task<Harbor> Add(Harbor model);
        Task<IEnumerable<Boat>> GetAllHarbour();
        Task<Harbor> GetById(string id);
        Task<Harbor> Delete();
        Task<Harbor> Update();
    }
}
