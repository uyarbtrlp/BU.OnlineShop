using AutoMapper;
using BU.OnlineShop.FileService.API.Dtos;
using BU.OnlineShop.FileService.Domain.FileInformations;
using BU.OnlineShop.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.FileService.API.Controllers
{
    [Route("api/file-service/file-informations")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FileInformationController : ControllerBase
    {
        private readonly IFileInformationRepository _fileInformationRepository;
        private readonly IFileInformationManager _fileInformationManager;
        private readonly IMapper _mapper;

        public FileInformationController(IFileInformationRepository fileInformationRepository, IFileInformationManager fileInformationManager, IMapper mapper)
        {
            _fileInformationRepository = fileInformationRepository;
            _fileInformationManager = fileInformationManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<FileInformationDto> CreateAsync([FromForm]CreateFileInformationInput input)
        {
            var content = await input.File.GetBytes();

            var fileInformation = await _fileInformationManager.CreateAsync(
                    name: input.Name != null ? input.Name : input.File.FileName,
                    mimeType: input.File.ContentType,
                    size: (int)input.File.Length,
                    content: content
                );

            await _fileInformationRepository.InsertAsync(
                  fileInformation, true);


            return _mapper.Map<FileInformationDto>(fileInformation);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<FileInformationDto> GetAsync(Guid id)
        {

            var fileInformation = await _fileInformationRepository.GetAsync(id);

            return _mapper.Map<FileInformation, FileInformationDto>(fileInformation);
        }

        [HttpGet]
        public async Task<List<FileInformationDto>> GetListAsync([FromQuery] GetFileInformationsInput input)
        {
            var fileInformations = await _fileInformationRepository.GetListAsync(
                    name: input.Name
                );

            return _mapper.Map<List<FileInformation>, List<FileInformationDto>>(fileInformations.ToList());
        }

        [HttpGet]
        [Route("{id}/download")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadAsync(Guid id)
        {

            var fileInformation = await _fileInformationRepository.GetAsync(id);

            return File(fileInformation.Content, fileInformation.MimeType, fileInformation.Name);
        }

        [HttpPut]
        [Route("{id}/change-name")]
        public async Task<FileInformationDto> ChangeNameAsync(Guid id, ChangeNameInput input)
        {

            var fileInformation = await _fileInformationRepository.GetAsync(id);

            fileInformation = await _fileInformationManager.ChangeNameAsync(fileInformation, input.Name);

            await _fileInformationRepository.UpdateAsync(
                  fileInformation, true);

            return _mapper.Map<FileInformation, FileInformationDto>(fileInformation);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            var product = await _fileInformationRepository.GetAsync(id);

            await _fileInformationRepository.DeleteAsync(product, true);
        }

    }
}
