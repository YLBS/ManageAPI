using AutoMapper;
using Entity.GoodBoss;
using Entity.Goodjob;
using Model;
using Model.SalesDepartment;

namespace Commons.Tool;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<被映射,映射成需要的>
        CreateMap<LoginUserInfo, ErmEmployee>();
        CreateMap<ErmEmployee, LoginUserInfo>();
        CreateMap<MemInfo, EffectCompanyInfo>();

        CreateMap<Entity.Goodjob.PubInformation, InputPubInformationDto>();
        CreateMap<InputPubInformationDto, Entity.Goodjob.PubInformation>();
    }
}
