using AutoMapper;
using BU.OnlineShop.FileService.API.Dtos;
using BU.OnlineShop.FileService.Domain.FileInformations;

namespace BU.OnlineShop.FileService.API
{
    public class FileServiceProfile : Profile
    {
        public FileServiceProfile()
        {
            CreateMap<FileInformation, FileInformationDto>();
        }
    }
}
