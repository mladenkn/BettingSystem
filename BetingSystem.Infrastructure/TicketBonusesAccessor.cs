using System;
using System.Collections.Generic;
using System.IO;
using BetingSystem.DAL;
using BetingSystem.Models;
using BetingSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BetingSystem.Infrastructure
{
    public class TicketBonusesAccessor : ITicketBonusesAccessor, IDisposable
    {
        private readonly string _file;

        public TicketBonusesAccessor(IConfiguration config, IHostingEnvironment env)
        {
            var fileName = config.GetValue<string>("TicketBonusesFile");
            _file = Path.Combine(env.ContentRootPath, fileName);
            var json = File.ReadAllText(_file);
            Value = JsonConvert.DeserializeObject<TicketBonuses>(json);
        }

        public TicketBonuses Value { get; set; }

        public void Dispose()
        {
            var json = JsonConvert.SerializeObject(Value);
            File.WriteAllText(_file, json);
        }
    }
}
