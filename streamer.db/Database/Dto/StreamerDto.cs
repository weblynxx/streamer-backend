using System.Collections.Generic;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.Dto
{
    public class StreamerDto : StreamerDm
    {
        public List<StreamerPartnerDto> Partners { get; set; }
        public List<StreamerServiceDto> Services { get; set; }
    } 




}
