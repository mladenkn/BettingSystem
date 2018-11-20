using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BetingSystem.RestApi
{
    public interface ISafeRunner
    {
        Task<IActionResult> Run(Func<Task> run, Func<IActionResult> createActionResult);
        Task<IActionResult> Run<TResult>(Func<Task<TResult>> run, Func<TResult, IActionResult> mapToActionResult);
    }

    public class SafeRunner : ISafeRunner
    {
        public async Task<IActionResult> Run(Func<Task> run, Func<IActionResult> createActionResult)
        {
            try
            {
                await run();
            }
            catch (Exception e)
            {
                var actionResult = MapExceptionToActionResult(e);
                return actionResult;
            }

            return createActionResult();
        }

        public async Task<IActionResult> Run<TResult>(Func<Task<TResult>> run, Func<TResult, IActionResult> mapToActionResult)
        {
            TResult result;

            try
            {
                result = await run();
            }
            catch (Exception e)
            {
                var actionResult = MapExceptionToActionResult(e);
                return actionResult;
            }

            return mapToActionResult(result);
        }

        private IActionResult MapExceptionToActionResult(Exception e)
        {
            switch (e)
            {
                case ModelNotFound modelNotFound:
                    return new NotFoundObjectResult($"Model with of type {modelNotFound.WantedObjectType} has not been found.");

                case BetablePairsNotFound betablePairsNotFound:
                    var idsString = string.Join(",", betablePairsNotFound.NotFoundPairsIds);
                    return new NotFoundObjectResult($"Pairs with the ids of: {idsString} have not been found.");

                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
    }
}
