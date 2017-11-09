using BlueYonder.Companion.Client.DataModel;

namespace BlueYonder.Companion.Client.DataTransferObjects
{
    public static class DTOExtensions
    {

        public static Traveler ToObject(this TravelerDTO dto)
        {
            return new Traveler()
            {
                TravelerId = dto.TravelerId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MobilePhone = dto.MobilePhone,
                HomeAddress = dto.HomeAddress,
                Passport = dto.Passport,
                Email = dto.Email
            };
        }

        public static TravelerDTO ToDTO(this Traveler traveler)
        {
            return new TravelerDTO()
            {
                TravelerId = traveler.TravelerId,
                FirstName = traveler.FirstName,
                LastName = traveler.LastName,
                MobilePhone = traveler.MobilePhone,
                HomeAddress = traveler.HomeAddress,
                Passport = traveler.Passport,
                Email = traveler.Email
            };
        }

        public static File ToObject(this FileDTO dto)
        {
            return new File()
            {
                FileName = dto.FileName,
                Uri = dto.Uri,
            };
        }
    }
}
