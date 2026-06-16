
public class BaseListResponse<T> : BaseResponse
{
    public int TotalRecords { get; set; }

    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}
