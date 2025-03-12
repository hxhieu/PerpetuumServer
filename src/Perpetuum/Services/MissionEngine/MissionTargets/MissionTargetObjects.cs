using Perpetuum.DataContext.Entities;
using System;

namespace Perpetuum.Services.MissionEngine.MissionTargets
{
    [Serializable]
    public abstract class MissionTargetRunsOnZone : MissionTarget
    {
        protected MissionTargetRunsOnZone(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTargetRunsOnZone(this);
        }
    }

    [Serializable]
    public abstract class MissionTargetProduction : MissionTarget
    {
        protected MissionTargetProduction(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTargetProduction(this);
        }
    }

    [Serializable]
    public class FetchItemMissionTarget : MissionTarget
    {
        public FetchItemMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_fetch_item(this);
        }
    }

    [Serializable]
    public class LootItemMissionTarget : MissionTargetRunsOnZone
    {
        public LootItemMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_loot_item(this);
        }
    }

    [Serializable]
    public class ReachPositionMissionTarget : MissionTargetRunsOnZone
    {
        public ReachPositionMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_reach_position(this);
        }
    }

    [Serializable]
    public class KillDefinitionMissionTarget : MissionTargetRunsOnZone
    {
        public KillDefinitionMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_kill_definition(this);
        }
    }

    [Serializable]
    public class ScanMineralMissionTarget : MissionTargetRunsOnZone
    {
        public ScanMineralMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_scan_mineral(this);
        }
    }

    [Serializable]
    public class ScanUnitMissionTarget : MissionTargetRunsOnZone
    {
        public ScanUnitMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_scan_unit(this);
        }
    }

    [Serializable]
    public class ScanContainerMissionTarget : MissionTargetRunsOnZone
    {
        public ScanContainerMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_scan_container(this);
        }
    }

    [Serializable]
    public class DrillMineralMissionTarget : MissionTargetRunsOnZone
    {
        public DrillMineralMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_drill_mineral(this);
        }
    }

    [Serializable]
    public class SubmitItemMissionTarget : MissionTargetRunsOnZone
    {
        public SubmitItemMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_submit_item(this);
        }
    }

    [Serializable]
    public class UseSwitchMissionTarget : MissionTargetRunsOnZone
    {
        public UseSwitchMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_use_switch(this);
        }
    }

    [Serializable]
    public class FindArtifactMissionTarget : MissionTargetRunsOnZone
    {
        public FindArtifactMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_find_artifact(this);
        }
    }

    [Serializable]
    public class DockInMissionTarget : MissionTarget
    {
        public DockInMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_dock_in(this);
        }
    }

    [Serializable]
    public class UseItemsupplyMissionTarget : MissionTargetRunsOnZone
    {
        public UseItemsupplyMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_use_itemsupply(this);
        }
    }

    [Serializable]
    public class PrototypeMissionTarget : MissionTargetProduction
    {
        public PrototypeMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_prototype(this);
        }
    }

    [Serializable]
    public class MassproduceMissionTarget : MissionTargetProduction
    {
        public MassproduceMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_massproduce(this);
        }
    }

    [Serializable]
    public class ResearchMissionTarget : MissionTargetProduction
    {
        public ResearchMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_research(this);
        }
    }

    [Serializable]
    public class TeleportMissionTarget : MissionTarget
    {
        public TeleportMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_teleport(this);
        }
    }

    [Serializable]
    public class HarvestPlantMissionTarget : MissionTargetRunsOnZone
    {
        public HarvestPlantMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_harvest_plant(this);
        }
    }

    [Serializable]
    public class SummonNpcEggMissionTarget : MissionTargetRunsOnZone
    {
        public SummonNpcEggMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_summon_npc_egg(this);
        }
    }

    [Serializable]
    public class PopNpcMissionTarget : MissionTargetRunsOnZone
    {
        public PopNpcMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_pop_npc(this);
        }
    }

    [Serializable]
    public class SpawnItemMissionTarget : MissionTarget
    {
        public SpawnItemMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_spawn_item(this);
        }
    }

    [Serializable]
    public class LockUnitMissionTarget : MissionTarget
    {

        public LockUnitMissionTarget(Missiontarget record) : base(record) { }

        public override void AcceptVisitor(MissionTargetVisitor visitor)
        {
            visitor.Visit_MissionTarget_lock_unit(this);
        }


    }

}
