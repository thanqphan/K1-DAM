using AutoMapper;
using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.Api.DTOs.Responses.Folder;
using DAM.DAM.DAL.Entities;

namespace DAM.DAM.Api.DTOs.Profiles
{
    public class FolderProfile : Profile
    {
        public FolderProfile()
        {
            CreateMap<FolderRequest, Folder>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrEmpty(str))));

            CreateMap<Folder, FolderResponse>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrEmpty(str))));

            CreateMap<Folder, FolderDetailsResponse>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string str && string.IsNullOrEmpty(str))));
        }
    }
}
