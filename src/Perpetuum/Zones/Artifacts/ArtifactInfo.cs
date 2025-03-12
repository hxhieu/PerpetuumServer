using Perpetuum.ExportedTypes;

namespace Perpetuum.Zones.Artifacts
{
    /// <summary>
    /// Describes one artifact on the zone
    /// </summary>
    public class ArtifactInfo 
    {
        public readonly ArtifactType type;
        public readonly int goalRange;
        public readonly int? npcPresenceId;
        public readonly bool isPersistent;
        public readonly int minimumLoot;

        public ArtifactInfo(DataContext.Entities.Artifacttype entity)
        {
            type = (ArtifactType)entity.Id;
            goalRange = entity.Goalrange;
            npcPresenceId = entity.Npcpresenceid;
            isPersistent = entity.Persistent;
            minimumLoot = entity.Minimumloot;
        }

        public override string ToString()
        {
            return string.Format("ArtifactInfo type:" + type);
        }

        public static ArtifactInfo GenerateArtifactInfo(DataContext.Entities.Artifacttype entity)
        {
            var persistentArtifact = entity.Persistent;
            var isDynamic = entity.Dynamic;
            if (persistentArtifact)
            {
                //normal persistent artifact
                return new ArtifactInfo(entity);
            }

            if (isDynamic)
            {
                return new DynamicArtifactInfo(entity);
            }

            //mission artifact, oldschool
            return new NonPersistentArtifactInfo(entity);

        }

    }

    /// <summary>
    /// Mission artifactInfo, the oldschool version, with fixed npc presence
    /// </summary>
    public class NonPersistentArtifactInfo : ArtifactInfo
    {
        public NonPersistentArtifactInfo(DataContext.Entities.Artifacttype record) : base(record)
        {
           
        }
    }

    /// <summary>
    /// Dynamic artifact info, npcs and stuff comes from random missions
    /// </summary>
    public class DynamicArtifactInfo : NonPersistentArtifactInfo
    {
        public DynamicArtifactInfo(DataContext.Entities.Artifacttype record) : base(record)
        {
            
        }
    }


}