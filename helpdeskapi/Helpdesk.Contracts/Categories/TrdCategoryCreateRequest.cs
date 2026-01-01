namespace Helpdesk.Contracts.Categories;

public record TrdCategoryCreateRequest(int CatId, int SubCatId, string TrdCatName, int EntryBy);
