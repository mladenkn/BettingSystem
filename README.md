# BettingSystem
Rest API for beting application build with ASP.NET Core.

Some things done in very elegant and inovative way, for an example:
..* Controllers call service methods using SafeRunner class, which catches all exceptions that could be thrown from the called service method, and if catches any, maps it to ActionResult.
..* Ticket disounts are applied using BonusApplier class which is fluent API.
..* DAL lib is implemented using DataProvider and UnitOfWork classes.
