using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.GenXY;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perpetuum.Items.Templates
{
    public class RobotTemplateReader(IDbRepository<Robottemplate> robotTemplateRepo) : IRobotTemplateReader
    {
        public IEnumerable<RobotTemplate> GetAll()
        {
            return robotTemplateRepo.GetMany().Select(CreateRobotTemplateFromRecord).ToList();
        }

        [CanBeNull]
        public RobotTemplate Get(int templateID)
        {
            var record = robotTemplateRepo.GetOne(x => x.Id == templateID);
            if (record == null)
                return null;

            return CreateRobotTemplateFromRecord(record);
        }

        private static RobotTemplate CreateRobotTemplateFromRecord(Robottemplate entity)
        {
            var id = entity.Id;
            var name = entity.Name;
            var description = entity.Description;
            var dictionary = GenxyConverter.Deserialize(description);
            var template = RobotTemplate.CreateFromDictionary(name, dictionary);
            if (template == null)
            {
                return null;
            }

            template.ID = id;
            return template;
        }
    }
}