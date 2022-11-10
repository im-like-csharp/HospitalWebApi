namespace Hospital.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> TakePage<T>(this IQueryable<T> source, int? pageNumber, int? pageSize) where T : class
    {
        if (pageNumber is null || pageNumber < 1)
        {
            pageNumber = 1!;
        }

        if (pageSize is null || pageSize < 1)
        {
            pageSize = 100!;
        }

        return source.Skip(((int)pageNumber - 1) * (int)pageSize).Take((int)pageSize);
    }
}
