using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;
using Microsoft.EntityFrameworkCore;



namespace XRayJournal.DAL
{
    public class CabinetRepository : ICabinetRepository
    {
        private readonly DataContext _dataContext;
        public CabinetRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<CabinetDTO>> GetAllAsync()
        {
            var result = await _dataContext.Cabinets.OrderBy(p => p.Id).ToListAsync();
            return result;
        }

        public async Task<CabinetDTO?> GetByIdAsync(int id)
        {
            var result = await _dataContext.Cabinets.SingleAsync(p => p.Id == id);
            return result;
        }

    }
}
