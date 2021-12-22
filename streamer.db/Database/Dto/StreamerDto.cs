using System.Collections.Generic;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.Dto
{
    public class StreamerDto : StreamerDm
    {
        public List<StreamerPartnerDto> PartnersFood { get; set; }
        public List<StreamerPartnerDto> PartnersClothes { get; set; }
        public List<StreamerServiceDto> Services { get; set; }
        public List<PreferenceDm> PreferencesFood { get; set; }
        public List<PreferenceDm> PreferencesClothes { get; set; }
    } 




}
