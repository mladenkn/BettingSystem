using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.DAL
{
    public interface IUnitOfWork : ApplicationKernel.IUnitOfWork
    {
        IBetablePairsRepository BetablePairs { get; }
        ITicketRepository Tickets { get; }
        IBetedPairRepository BetedPairs { get; }
    }
}
