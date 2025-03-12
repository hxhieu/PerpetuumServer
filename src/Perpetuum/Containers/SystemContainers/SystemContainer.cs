using Perpetuum.Data;
using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;
using System.Collections.Generic;

namespace Perpetuum.Containers.SystemContainers
{
    public class SystemContainer : Container
    {
        private static readonly IDictionary<string, long> _entityStorage;

        static SystemContainer()
        {
            _entityStorage = Database.CreateCache<string, long, Entitystorage>(
                x => x.StorageName,
                x => x.Eid ?? 0
            );
        }

        public override void AcceptVisitor(IEntityVisitor visitor)
        {
            if (!TryAcceptVisitor(this, visitor))
                base.AcceptVisitor(visitor);
        }

        public static SystemContainer GetByName(string name)
        {
            var eid = _entityStorage[name];
            return (SystemContainer) Repository.LoadOrThrow(eid);
        }
    }
}