namespace Courses.Application.Common.Models;

public class PagedList<T>
{
    public PagedList(IEnumerable<T> items, int page, int pageSize, int totalCount)
    {
        PageIndex = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        Items = items.ToList();
    }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => (PageIndex + 1) * PageSize < TotalCount;

    public IReadOnlyCollection<T> Items { get; set; }
}
