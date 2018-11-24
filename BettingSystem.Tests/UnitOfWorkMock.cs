using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace BetingSystem.Tests
{
    public class UnitOfWorkMock
    {
        public UnitOfWorkMock()
        {
            var inserted = new List<object>();
            var updated = new List<object>();
            var deleted = new List<object>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            Object = unitOfWorkMock.Object;
            Changes = new UnitOfWorkChanges(inserted, updated, deleted);

            unitOfWorkMock.Setup(u => u.Add(It.IsAny<object>())).Callback<object>(o => inserted.Add(o));
            unitOfWorkMock.Setup(u => u.Update(It.IsAny<object>())).Callback<object>(o => updated.Add(o));
            unitOfWorkMock.Setup(u => u.Delete(It.IsAny<object>())).Callback<object>(o => deleted.Add(o));
            unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(Task.CompletedTask).Callback(() => Changes.SavedChanges = true);
        }

        public IUnitOfWork Object { get; }
        public UnitOfWorkChanges Changes { get; }
    }

    public class UnitOfWorkChanges
    {
        public UnitOfWorkChanges(IReadOnlyCollection<object> inserted, IReadOnlyCollection<object> updated, IReadOnlyCollection<object> deleted)
        {
            Inserted = inserted;
            Updated = updated;
            Deleted = deleted;
        }

        public IReadOnlyCollection<object> Inserted { get; }
        public IReadOnlyCollection<object> Updated { get; }
        public IReadOnlyCollection<object> Deleted { get; }
        public bool SavedChanges { get; set; }
    }
}
