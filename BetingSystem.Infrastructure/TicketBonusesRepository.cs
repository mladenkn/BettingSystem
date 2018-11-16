using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BetingSystem.Infrastructure
{
    public class TicketBonusesRepository : ITicketBonusesRepository
    {
        private readonly List<ITicketBonus> _bonuses;

        public TicketBonusesRepository(IConfiguration config, IHostingEnvironment env)
        {
            var fileName = config.GetValue<string>("TicketBonusesFile");
            var file = Path.Combine(env.ContentRootPath, fileName);
            var json = File.ReadAllText(file);
            var bonuses = JsonConvert.DeserializeObject<TicketBonuses>(json);
            _bonuses = new List<ITicketBonus> {bonuses.AllSportsBonus, bonuses.VariousSportsBonus};
        }

        public Task<IEnumerable<ITicketBonus>> AllActive() => Task.FromResult(_bonuses.Where(b => b.IsActive));
    }

    public class TicketBonuses
    {
        public VariousSportsBonus VariousSportsBonus { get; set; }
        public AllSportsBonus AllSportsBonus { get; set; }
    }
}
