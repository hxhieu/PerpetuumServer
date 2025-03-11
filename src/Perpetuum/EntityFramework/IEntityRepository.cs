using Perpetuum.ExportedTypes;
using System.Collections.Generic;

namespace Perpetuum.EntityFramework
{
    public interface IEntityRepository
    {
        void Insert(Entity entity);
        void Update(Entity entity);
        void Delete(Entity entity);

        Entity Load(long eid);
        Entity Load(DataContext.Entities.Entity entity);
        List<Entity> LoadByOwner(long rootEid, long? ownerEid);

        Entity LoadTree(long rootEid, long? ownerEid);
        Entity LoadRawTree(long rootEid);

        string GetName(long eid);
        int GetChildrenCount(long eid);
        long[] GetFirstLevelChildren(long eid);
        IEnumerable<Entity> GetFirstLevelChildrenByOwner(long parent, long owner);
        long[] GetFirstLevelChildrenByCategoryflags(long eid, CategoryFlags categoryFlags);

        Entity GetChildByDefinition(Entity parent, int childDefinition);

        IEnumerable<Entity> GetFirstLevelChildrenEntity(long eid);
    }
}