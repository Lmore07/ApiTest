public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public PaginationInfo Pagination { get; set; }

    public ApiResponse(T data, string message = null, PaginationInfo pagination = null)
    {
        Data = data;
        Message = message;
        Pagination = pagination;
    }

    public ApiResponse(T data, string message = null)
    {
        Data = data;
        Message = message;
    }

    public ApiResponse(string message = null)
    {
        Message = message;
    }
}

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public PaginationInfo(int currentPage, int totalPages, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
