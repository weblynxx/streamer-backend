using AutoMapper;
using streamer.db.Database.DataModel;

namespace streamer.Features.StreamerService
{
    public class MappingStreamerService : Profile
    {
        public MappingStreamerService()
        {
            CreateMap<StreamerServiceAdd.StreamerServiceAddCommand, StreamerServiceDm>(MemberList.Source);
        }
    }
}
