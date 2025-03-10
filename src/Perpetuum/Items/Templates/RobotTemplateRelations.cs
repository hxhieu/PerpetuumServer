using AutoMapper;
using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;
using System.Collections.Generic;

namespace Perpetuum.Items.Templates
{
    public class RobotTemplateRelations : IRobotTemplateRelations
    {
        private readonly IRobotTemplateReader _robotTemplateReader;
        private readonly IEntityDefaultReader _entityDefaultReader;
        private readonly IMapper _mapper;
        private readonly IDbRepository<Robottemplaterelation> _robotTemplateRelRepo;
        private readonly Dictionary<int,IRobotTemplateRelation> _relations = new Dictionary<int, IRobotTemplateRelation>();

        private RobotTemplate _equippedDefault;
        private RobotTemplate _unequippedDefault;

        public RobotTemplateRelations(
            IRobotTemplateReader robotTemplateReader,
            IEntityDefaultReader entityDefaultReader,
            IMapper mapper,
            IDbRepository<Robottemplaterelation> robotTemplateRelRepo
        )
        {
            _robotTemplateReader = robotTemplateReader;
            _entityDefaultReader = entityDefaultReader;
            _mapper = mapper;
            _robotTemplateRelRepo = robotTemplateRelRepo;
        }

        public void Init()
        {
            var records = _robotTemplateRelRepo.GetMany();

            foreach (var record in records)
            {
                var relation = _mapper.Map<RobotTemplateRelation>(record);
                relation.EntityDefault = _entityDefaultReader.Get(record.Definition);
                relation.Template = _robotTemplateReader.Get(record.Templateid);
                _relations[relation.EntityDefault.Definition] = relation;
            }
            _equippedDefault = _robotTemplateReader.GetByName("starter_master");
            _unequippedDefault = _robotTemplateReader.GetByName("arkhe_empty");
        }

        public RobotTemplate EquippedDefault => _equippedDefault;
        public RobotTemplate UnequippedDefault => _unequippedDefault;

        public IRobotTemplateRelation Get(int definition)
        {
            return _relations.GetOrDefault(definition);
        }

        public IEnumerable<IRobotTemplateRelation> GetAll()
        {
            return _relations.Values;
        }
    }
}