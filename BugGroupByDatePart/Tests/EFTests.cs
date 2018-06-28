using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BugGroupByDatePart.Classes;
using BugGroupByDatePart.Classes.Models;
using Microsoft.Extensions.Logging;
using NLog;
using Xunit;

namespace BugGroupByDatePart.Tests
{
    [Collection(DemoDbContextFixture.ID)]
    public class EFTests
    {
        private readonly DemoDbContext _context;
        private readonly Logger _logger;

        public EFTests(DemoDbContextFixture DemoDbContextFixture)
        {
            _context = DemoDbContextFixture.Context;
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug("EFTests start");
        }

        // the buggy code, see comment inside
        private IQueryable<ConsumptionPerMonth> BugGroupByQuery()
        {
            long clientId = 0;
            var userIds = from user in _context.Users where user.ClientId == clientId select user.Id;
            var transfers = _context.MoneyTransfers
                .Where(mt =>
                    (
                        mt is MoneyTransferClient
                        &&
                        ((MoneyTransferClient)mt).Client.ID == clientId)
                    ||
                    (
                        mt is MoneyTransferUser
                        &&
                        userIds.Contains(((MoneyTransferUser)mt).User.Id)
                    )
                )
                .Where(mt => mt.Event is OrderThings)
                .GroupBy(mt => new
                {
                    mt.Author.IsActive,
                    mt.Author.UserName,
                    mt.Author.LastName,
                    mt.Author.FirstName,
                    mt.Date.Year,
                    mt.Date.Month
                })
                .Select(g => new ConsumptionPerMonth()
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    User = new ConsumptionPerMonth.Author()
                    {
                        IsActive = g.Key.IsActive,
                        Login = g.Key.UserName,
                        LastName = g.Key.LastName,
                        FirstName = g.Key.FirstName
                    },
                    Amount = g.Sum(m => m.Amount)
                });

            // --- this is what makes the bug happen --- //
            Expression<Func<ConsumptionPerMonth, object>>[] expressions = {
                c => c.Year,
                c => c.Month,
                c => c.User.LastName,
                c => c.User.FirstName
            };
            transfers = transfers
               .OrderBy(expressions[0])
               .ThenBy(expressions[1])
               .ThenBy(expressions[2])
               .ThenBy(expressions[3]);
            // --- ---------- --- //

            return transfers
                .Skip(0)
                .Take(20);
        }

        [Fact]
        public void TestGroupByDatePart()
        {
            var bugQuery = BugGroupByQuery();
            var list = bugQuery.ToList(); // BAM

            _logger.Info("Success!!");
        }
    }
}
