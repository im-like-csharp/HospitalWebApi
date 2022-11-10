namespace Hospital.Responses;

public class PagedResponse<T> : Response<T>
{
    public PagedResponse(T data, int? pageNumber, int? pageSize) : base(data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int? PageNumber { get; }

    public int? PageSize { get; }
}