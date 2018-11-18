using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationKernel;
using Moq;

namespace BetingSystem.Tests
{
    public class TransactionMock
    {
        public TransactionMock()
        {
            var transactionMock = new Mock<IDatabaseTransaction>();

            var inserted = new List<object>();
            var updated = new List<object>();
            var deleted = new List<object>();

            PendingChanges = new TransactionMockPendingChanges(inserted, updated, deleted);
            Transaction = transactionMock.Object;
            
            transactionMock.Setup(t => t.Insert(It.IsAny<object>()))
                .Returns(transactionMock.Object)
                .Callback<object>(o => inserted.Add(o));

            transactionMock.Setup(t => t.Update(It.IsAny<object>()))
                .Returns(transactionMock.Object)
                .Callback<object>(o => updated.Add(o));

            transactionMock.Setup(t => t.Delete(It.IsAny<object>()))
                .Returns(transactionMock.Object)
                .Callback<object>(o => deleted.Add(o));

            transactionMock.Setup(t => t.Commit())
                .Callback(() => PendingChanges.Commited = true)
                .Returns(Task.CompletedTask);
        }

        public IDatabaseTransaction Transaction { get; set; }
        public TransactionMockPendingChanges PendingChanges { get; set; }
    }

    public class TransactionMockPendingChanges
    {
        public TransactionMockPendingChanges(
            IReadOnlyCollection<object> inserted, IReadOnlyCollection<object> updated, IReadOnlyCollection<object> deleted)
        {
            Inserted = inserted;
            Updated = updated;
            Deleted = deleted;
        }

        public IReadOnlyCollection<object> Inserted { get; }
        public IReadOnlyCollection<object> Updated { get; }
        public IReadOnlyCollection<object> Deleted { get; }
        public bool Commited { get; set; }
    }
}
