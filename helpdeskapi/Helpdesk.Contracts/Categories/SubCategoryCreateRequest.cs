namespace Helpdesk.Contracts.Categories;

public record SubCategoryCreateRequest(int CatId, string SubCatName, int EntryBy);
