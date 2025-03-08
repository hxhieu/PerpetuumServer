
namespace Perpetuum.Mapper
{
    internal class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            //Id = record.GetValue<int>("accountid"),
            //Password = record.GetValue<string>("password"),
            //SteamId = record.GetValue<string>("steamId"),
            //Creation = record.GetValue<DateTime>("creation"),
            //AccessLevel = (AccessLevel)record.GetValue<int>("accLevel"),
            //EmailConfirmed = record.GetValue<bool>("emailConfirmed"),
            //Email = record.GetValue<string>("email"),
            //BanTime = record.GetValue<DateTime?>("banTime"),
            //BanLength = TimeSpan.FromSeconds(record.GetValue<int>("banLength")),
            //BanNote = record.GetValue<string>("banNote"),
            //TwitchAuthToken = record.GetValue<string>("twitchAuthToken"),
            //State = (AccountState)record.GetValue<int>("state"),
            //ValidUntil = record.GetValue<DateTime?>("validuntil"),
            //PayingCustomer = record.GetValue<bool>("payingcustomer"),
            //IsActive = record.GetValue<bool>("isactive"),
            //FirstCharacterDate = record.GetValue<DateTime?>("firstcharacter"),
            //IsLoggedIn = record.GetValue<bool>("isloggedin"),
            //LastLoggedIn = record.GetValue<DateTime?>("lastloggedin"),
            //TotalOnlineTime = TimeSpan.FromMinutes(record.GetValue<int>("totalminsonline")),
            //Credit = record.GetValue<int>("credit")

            CreateMap<DataContext.Models.Account, Accounting.Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.AccessLevel, opt => opt.MapFrom(src => (AccessLevel)src.AccLevel))
                .ForMember(dest => dest.FirstCharacterDate, opt => opt.MapFrom(src => src.Firstcharacter))
                .ForMember(dest => dest.TotalOnlineTime, opt => opt.MapFrom(src => src.TotalMinsOnline))
            ;
        }
    }
}
