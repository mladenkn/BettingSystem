using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.Models;
using Newtonsoft.Json;

namespace BetingSystem.DAL
{
    public class TicketBonusesRepository : ITicketBonusesRepository
    {
        private readonly List<ITicketBonus> _bonuses;

        public class Dependecies
        {
            public string FilePath { get; set; }
        }

        public TicketBonusesRepository(Dependecies deps)
        {
            var json = File.ReadAllText(deps.FilePath);
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
