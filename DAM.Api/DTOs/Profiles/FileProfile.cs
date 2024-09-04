using AutoMapper;
using DAM.DAM.Api.DTOs.Requests.File;
using DAM.DAM.Api.DTOs.Responses.File;
using DAM.DAM.DAL.Enums;
using File = DAM.DAM.DAL.Entities.File;

namespace DAM.DAM.Api.DTOs.Profiles
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<FileRequest, File>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrEmpty(str))));

            CreateMap<File, FileResponse>()
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.ToString()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember) =>
                    srcMember != null || !string.IsNullOrEmpty(destMember?.ToString())));
        }
    }
}
