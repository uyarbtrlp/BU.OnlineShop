using AutoMapper;
using BU.OnlineShop.CatalogService.API.Dtos.Categories;
using BU.OnlineShop.CatalogService.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.CatalogService.API.Controllers
{
    [Route("api/catalog-service/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryManager _categoryManager;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, ICategoryManager categoryManager, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<CategoryDto> CreateAsync(CreateCategoryInput input)
        {

            var category = await _categoryManager.CreateAsync(input.Name, description: input.Description);
            

            await _categoryRepository.InsertAsync(
                  category, true);


            return _mapper.Map<CategoryDto>(category);
        }

        [HttpGet]
        public async Task<List<CategoryDto>> GetListAsync()
        {
            var categories = await _categoryRepository.GetListAsync();

            return _mapper.Map<List<Category>, List<CategoryDto>>(categories.ToList());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryInput input)
        {
            var category = await _categoryRepository.GetAsync(id);

            category.SetName(input.Name);
            category.SetDescription(input.Description);

            category = await _categoryRepository.UpdateAsync(category, true);

            return _mapper.Map<CategoryDto>(category);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);

            await _categoryRepository.DeleteAsync(category, true);
        }
    }
}
