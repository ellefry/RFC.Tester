using BHSW2_2.Pinion.DataService.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BHSW2_2.Pinion.DataService.Domains;
using System;

namespace BHSW2_2.Pinion.DataService
{
    public class SapSwitcher : ISapSwitcher
    {
        private readonly SapConnectorContext _dbContext;
        private const string SwitcherId = "63b33382-d29f-11ec-9d64-0242ac120002";

        public SapSwitcher(SapConnectorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task EnableSwitcher(bool isEnabled)
        {
            var switcher = await _dbContext.SapConnectionSwitchers.FirstOrDefaultAsync(s => s.Id == SwitcherId);
            if (switcher == null)
            {
                await CreateSwitcher(isEnabled);
            }
            else
            {
                switcher.IsEnabled = isEnabled;
                switcher.UpdatedAt = DateTimeOffset.Now;
                _dbContext.Entry(switcher).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> GetSwitcherStatus()
        {
            var switcher = await _dbContext.SapConnectionSwitchers.FirstOrDefaultAsync(s=>s.Id == SwitcherId);
            if (switcher == null)
            {
                await CreateSwitcher(true);
                return true;
            }
            else
            {
                return switcher.IsEnabled;
            }
        }

        private async Task CreateSwitcher(bool isEnabled)
        {
            var newSwitcher = new SapConnectionSwitcher
            {
                Id = SwitcherId,
                IsEnabled = isEnabled
            };
            await _dbContext.SapConnectionSwitchers.AddAsync(newSwitcher);
            await _dbContext.SaveChangesAsync();
        }
    }
}