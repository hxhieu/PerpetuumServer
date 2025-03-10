using AutoMapper;
using Perpetuum.Data;
using Perpetuum.DataContext;
using Perpetuum.Services.ExtensionService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.EntityFramework
{
    public class EntityDefaultReader : IEntityDefaultReader
    {
        private readonly IExtensionReader _extensionReader;
        private readonly IMapper _mapper;
        private readonly IDbRepository<DataContext.Entities.Definitionconfig> _definitionConfigRepo;
        private readonly IDbRepository<DataContext.Entities.Entitydefault> _entityDefaultRepo;
        private Dictionary<int, EntityDefault> _entityDefaults;

        public EntityDefaultReader(
            IExtensionReader extensionReader,
            IMapper mapper,
            IDbRepository<DataContext.Entities.Definitionconfig> definitionConfigRepo,
            IDbRepository<DataContext.Entities.Entitydefault> entityDefaultRepo
        )
        {
            _extensionReader = extensionReader;
            _mapper = mapper;
            _definitionConfigRepo = definitionConfigRepo;
            _entityDefaultRepo = entityDefaultRepo;
        }

        public void Init()
        {
            _entityDefaults = LoadAll();
        }

        public bool Exists(int definition)
        {
            return Get(definition) != EntityDefault.None;
        }

        public EntityDefault Get(int definition)
        {
            return _entityDefaults.GetOrDefault(definition, EntityDefault.None);
        }

        public EntityDefault GetByEid(long eid)
        {
            var definition = Db.Query().CommandText("select definition from entities where eid = @eid")
                .SetParameter("@eid", eid)
                .ExecuteScalar<int>();

            return Get(definition);
        }

        public bool TryGet(int definition, out EntityDefault entityDefault)
        {
            entityDefault = Get(definition);
            return entityDefault != EntityDefault.None;
        }

        public IEnumerable<EntityDefault> GetAll()
        {
            return _entityDefaults.Values;
        }

        public int CountNonEnabledDefinitions()
        {
            var count = Db.Query().CommandText("select count(*) from entities where definition in (select definition from entitydefaults where enabled=0)").ExecuteScalar<int>();
            return count;
        }

        private Dictionary<int, EntityDefault> LoadAll()
        {
            var entities = _entityDefaultRepo.GetMany(x => x.Enabled, TimeSpan.FromHours(1)); // Can cache for a very long time?

            var definitionConfigs = LoadDefinitionConfigs();

            var result = new Dictionary<int, EntityDefault>();

            foreach (var e in entities)
            {
                var definition = e.Definition;

                // initial object
                var entityDefault = new EntityDefault
                {
                    _descriptionToken = e.Descriptiontoken,
                    _hidden = e.Hidden,
                    Config = definitionConfigs.GetOrDefault(definition, DefinitionConfig.None),
                    EnablerExtensions = GetEnablerAndRequiredExtensions(definition)
                };

                // Map the rest
                entityDefault = _mapper.Map(e, entityDefault);

                result[definition] = entityDefault;
            }

            return result;
        }

        private Dictionary<Extension, Extension[]> GetEnablerAndRequiredExtensions(int definition)
        {
            return _extensionReader.GetEnablerExtensions(definition).ToDictionary(e => e, e => _extensionReader.GetRequiredExtensions(e.id).ToArray());
        }

        private Dictionary<int, DefinitionConfig> LoadDefinitionConfigs()
        {
            var entities = _definitionConfigRepo.GetMany(_ => true, TimeSpan.FromHours(1)); // Can cache this for very long time?
            var records = entities.Select(e =>
            {
                // Private fields
                var definitionConfig = new DefinitionConfig(e);
                // Map the rest
                return _mapper.Map(e, definitionConfig);
            });
            return records.ToDictionary(r => r.definition, r => r);
        }
    }
}