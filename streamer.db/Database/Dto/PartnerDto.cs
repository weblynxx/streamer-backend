using streamer.db.Database.DataModel;

namespace streamer.db.Database.Dto
{
    public class PartnerDto :PartnerDm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DeliverName { get; set; }
        public DeliveryType DeliveryType { get; set; }
    } 




}
