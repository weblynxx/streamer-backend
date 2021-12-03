using AutoMapper;
using streamer.db.Database.DataModel;

namespace streamer.Features.Partner
{
    public class MappingPartner : Profile
    {
        public MappingPartner()
        {
            CreateMap<PartnerAddUpdate.PartnerAddUpdateCommand, PartnerDm>(MemberList.Source);
        }
    }
}
