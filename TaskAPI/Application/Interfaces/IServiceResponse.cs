namespace TaskAPI.Application.Interfaces
{
    public interface IServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}