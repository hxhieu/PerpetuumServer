using AutoMapper;
using Perpetuum.Data;
using Perpetuum.DataContext;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Services.Channels
{
    public class ChannelRepository(
        ChannelLoggerFactory channelLoggerFactory,
        IMapper mapper,
        IDbRepository<DataContext.Entities.Channel> channelRepo
    ) : IChannelRepository
    {
        public Channel Insert(Channel channel)
        {
            const string cmd = "insert into channels (name,type) values (@name,@type);select id from channels where id = scope_identity()";

            int id = Db.Query().CommandText(cmd)
                .SetParameter("@name", channel.Name)
                .SetParameter("@type", (int)channel.Type)
                .ExecuteScalar<int>().ThrowIfEqual(0, ErrorCodes.SQLInsertError);

            return channel.SetId(id);
        }

        public void Delete(Channel channel)
        {
            Db.Query().CommandText("delete channels where id = @channelId")
                .SetParameter("@channelId", channel.Id)
                .ExecuteNonQuery().ThrowIfZero(ErrorCodes.SQLDeleteError);
        }

        public void Update(Channel channel)
        {
            Db.Query().CommandText("update channels set topic = @topic, password = @password, type=@type where id = @channelid")
                .SetParameter("@topic", channel.Topic)
                .SetParameter("@password", channel.Password)
                .SetParameter("@type", channel.Type)
                .SetParameter("@channelid", channel.Id)
                .ExecuteNonQuery().ThrowIfEqual(0, ErrorCodes.SQLUpdateError);
        }

        public IEnumerable<Channel> GetAll()
        {
            return channelRepo.GetMany().Select(e =>
            {
                var type = (ChannelType)e.Type;
                var name = e.Name;
                var logger = channelLoggerFactory(name);
                var channel = new Channel(type, name, logger);
                return mapper.Map(e, channel);
            }).ToArray();
        }
    }
}