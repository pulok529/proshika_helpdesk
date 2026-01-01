using System.Data;
using Helpdesk.Contracts.Categories;
using Helpdesk.Infrastructure.Legacy;
using Microsoft.Data.SqlClient;

namespace Helpdesk.Infrastructure.Services;

public class CategoryService
{
    private readonly LegacyDataAccess _dataAccess;

    public CategoryService(LegacyDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Category", Array.Empty<SqlParameter>(), cancellationToken);
        return dt.Rows.Cast<DataRow>()
            .Select(r => new CategoryDto(r.Field<int>("CatId"), r.Field<string?>("CatName")))
            .ToList();
    }

    public async Task<IReadOnlyList<SubCategoryDto>> GetSubCategoriesAsync(int catId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_SubCategory", new[] { new SqlParameter("@CatId", catId) }, cancellationToken);
        return dt.Rows.Cast<DataRow>()
            .Select(r => new SubCategoryDto(r.Field<int>("SubCatId"), r.Field<int>("CatId"), r.Field<string?>("SubCatName")))
            .ToList();
    }

    public async Task<IReadOnlyList<TrdCategoryDto>> GetTrdCategoriesAsync(int subCatId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_TrdCategory", new[] { new SqlParameter("@SubCatId", subCatId) }, cancellationToken);
        return dt.Rows.Cast<DataRow>()
            .Select(r => new TrdCategoryDto(r.Field<int>("TrdCatId"), r.Field<int>("SubCatId"), r.Field<string?>("TrdCatName")))
            .ToList();
    }

    public async Task<int> CreateCategoryAsync(CategoryCreateRequest request, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CatName", request.CatName),
            new("@EntryBy", request.EntryBy),
            new("@EntryDate", DateTime.Now)
        };
        return await _dataAccess.ExecuteScalarIntAsync("sp_Save_Category", parameters, cancellationToken);
    }

    public async Task<int> CreateSubCategoryAsync(SubCategoryCreateRequest request, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CatId", request.CatId),
            new("@SubCatName", request.SubCatName),
            new("@EntryBy", request.EntryBy),
            new("@EntryDate", DateTime.Now)
        };
        return await _dataAccess.ExecuteScalarIntAsync("sp_Save_SubCategory", parameters, cancellationToken);
    }

    public async Task<int> CreateTrdCategoryAsync(TrdCategoryCreateRequest request, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CatId", request.CatId),
            new("@SubCatId", request.SubCatId),
            new("@TrdCatName", request.TrdCatName),
            new("@EntryBy", request.EntryBy),
            new("@EntryDate", DateTime.Now)
        };
        return await _dataAccess.ExecuteScalarIntAsync("sp_Save_TrdCategory", parameters, cancellationToken);
    }
}
