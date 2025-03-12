using AutoMapper;
using Perpetuum.Accounting.Characters;
using Perpetuum.Data;
using Perpetuum.DataContext;
using Perpetuum.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Perpetuum.Services.ExtensionService
{
    public class ExtensionReader : IExtensionReader
    {
        private readonly Lazy<IEntityDefaultReader> _entityDefaultReader;
        private readonly IMapper _mapper;
        private readonly IDbRepository<DataContext.Entities.Extension> _extRepo;
        private readonly IDbRepository<DataContext.Entities.Extensionprerequire> _extPrerequireRepo;
        private readonly IDbRepository<DataContext.Entities.Enablerextension> _enablerExtRepo;
        private ILookup<int, Extension> _enablerExtensions;
        private ImmutableDictionary<int, ExtensionInfo> _extensions;
        private ILookup<int, ExtensionBonus> _robotComponentExtensionBonuses;

        public ExtensionReader(
            Lazy<IEntityDefaultReader> entityDefaultReader,
            IMapper mapper,
            IDbRepository<DataContext.Entities.Extension> extRepo,
            IDbRepository<DataContext.Entities.Extensionprerequire> extPrerequireRepo,
            IDbRepository<DataContext.Entities.Enablerextension> enablerExtRepo
        )
        {
            _entityDefaultReader = entityDefaultReader;
            _mapper = mapper;
            _extRepo = extRepo;
            _extPrerequireRepo = extPrerequireRepo;
            _enablerExtRepo = enablerExtRepo;
        }

        private ILookup<int, Extension> GetEnablerExtensions()
        {
            if (_enablerExtensions == null)
            {
                var extensions = GetExtensions();

                _enablerExtensions = _enablerExtRepo.GetMany(cacheTime: TimeSpan.FromHours(1))  // Can cache for a very long time?
                    .Select(e => new
                    {
                        definition = e.Definition,
                        extension = new Extension(e.Extensionid, e.Extensionlevel)
                    })
                    .Where(x => extensions.ContainsKey(x.extension.id))
                    .Distinct()
                    .ToLookup(x => x.definition, x => x.extension);
            }

            return _enablerExtensions;
        }

        public Extension[] GetEnablerExtensions(int definition)
        {
            return GetEnablerExtensions().GetOrEmpty(definition);
        }

        public ImmutableDictionary<int, ExtensionInfo> GetExtensions()
        {
            if (_extensions == null)
            {
                var extensions = _extRepo.GetMany(x => x.Active, TimeSpan.FromHours(1))  // Can cache for a very long time?
                    .Select(e =>
                    {
                        // Private fields
                        var ext = new ExtensionInfo(e);
                        // Map the rest
                        return _mapper.Map(e,ext);
                    })
                    .ToDictionary(e => e.id);

                var requiredExtensions = _extPrerequireRepo.GetMany(cacheTime: TimeSpan.FromHours(1))  // Can cache for a very long time?
                    .Select(e =>
                    {
                        var id = e.Requiredextension;
                        var level = e.Requiredlevel;
                        return new
                        {
                            extensionID = e.Extensionid,
                            requiredExtension = new Extension(id, level)
                        };
                    })
                    .Where(r => extensions.ContainsKey(r.requiredExtension.id))
                    .ToLookup(r => r.extensionID, r => r.requiredExtension);

                foreach (var info in extensions.Values)
                {
                    info.RequiredExtensions = requiredExtensions.GetOrEmpty(info.id);
                }

                _extensions = extensions.ToImmutableDictionary();
            }

            return _extensions;
        }

        public Extension[] GetCharacterDefaultExtensions(Character character)
        {
            return GetAllRaceExtensions(character.RaceId)
                .Concat(GetAllSchoolExtensions(character.SchoolId))
                .Concat(GetAllMajorExtensions(character.MajorId))
                .Concat(GetAllSparkExtensions(character.SparkId))
                .Concat(GetAllCorporationExtensions(character.DefaultCorporationEid))
                .GroupBy(e => e.id)
                .Select(grp => new Extension(grp.Key, grp.Sum(g => g.level))).ToArray();
        }

        public ExtensionBonus[] GetRobotComponentExtensionBonus(int robotComponentDefinition)
        {
            if (_robotComponentExtensionBonuses == null)
            {
                var extensions = GetExtensions();
                _robotComponentExtensionBonuses = Database.CreateLookupCache<int, ExtensionBonus, DataContext.Entities.Chassisbonu>(
                    x => x.Definition,
                    x => new ExtensionBonus(x),
                    x => _entityDefaultReader.Value.Exists(x.Definition) && extensions.ContainsKey(x.Extension)
                );
            }

            return _robotComponentExtensionBonuses.GetOrEmpty(robotComponentDefinition);
        }

        private IEnumerable<Extension> GetAllRaceExtensions(int raceId)
        {
            return GetCharacterDefaultExtensions("race", "raceId", raceId);
        }

        private IEnumerable<Extension> GetAllSchoolExtensions(int schoolId)
        {
            return GetCharacterDefaultExtensions("school", "schoolId", schoolId);
        }

        private IEnumerable<Extension> GetAllMajorExtensions(int majorId)
        {
            return GetCharacterDefaultExtensions("major", "majorId", majorId);
        }

        private IEnumerable<Extension> GetAllSparkExtensions(int sparkId)
        {
            return GetCharacterDefaultExtensions("spark", "sparkId", sparkId);
        }

        private IEnumerable<Extension> GetAllCorporationExtensions(long corporationEid)
        {
            return GetCharacterDefaultExtensions("corporation", "corporationEID", corporationEid);
        }

        private IEnumerable<Extension> GetCharacterDefaultExtensions(string table, string idName, object id)
        {
            var extensions = GetExtensions();

            return Db.Query().CommandText("select * from cw_" + table + "_extension where " + idName + " = @id")
                .SetParameter("@id", id)
                .Execute()
                .Select(r => new Extension(r.GetValue<int>("extensionid"), r.GetValue<int>("levelincrement")))
                .Where(e => extensions.ContainsKey(e.id));
        }
    }
}