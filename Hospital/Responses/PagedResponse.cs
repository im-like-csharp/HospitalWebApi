namespace Hospital.Responses;

public class PagedResponse<T>
{
    public PagedResponse(T response, int? pageNumber, int? pageSize)
    {
        Data = response;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public T Data { get; set; }

    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}