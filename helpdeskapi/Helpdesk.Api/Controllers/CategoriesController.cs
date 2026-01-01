using Helpdesk.Contracts.Categories;
using Helpdesk.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Api.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var items = await _service.GetCategoriesAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{catId:int}/subcategories")]
    public async Task<IActionResult> GetSubCategories(int catId, CancellationToken cancellationToken)
    {
        var items = await _service.GetSubCategoriesAsync(catId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{catId:int}/subcategories/{subCatId:int}/trd-categories")]
    public async Task<IActionResult> GetTrdCategories(int catId, int subCatId, CancellationToken cancellationToken)
    {
        var items = await _service.GetTrdCategoriesAsync(subCatId, cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _service.CreateCategoryAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCategories), new { id }, new { id });
    }

    [HttpPost("{catId:int}/subcategories")]
    public async Task<IActionResult> CreateSubCategory(int catId, [FromBody] SubCategoryCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _service.CreateSubCategoryAsync(request with { CatId = catId }, cancellationToken);
        return CreatedAtAction(nameof(GetSubCategories), new { catId }, new { id });
    }

    [HttpPost("{catId:int}/subcategories/{subCatId:int}/trd-categories")]
    public async Task<IActionResult> CreateTrdCategory(int catId, int subCatId, [FromBody] TrdCategoryCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _service.CreateTrdCategoryAsync(request with { CatId = catId, SubCatId = subCatId }, cancellationToken);
        return CreatedAtAction(nameof(GetTrdCategories), new { catId, subCatId }, new { id });
    }
}
