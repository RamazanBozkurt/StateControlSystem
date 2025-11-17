namespace StateControlSystem.Models.Responses
{
    public class ServiceResponsePropertyModel
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public sealed class ServiceResponse<T> : ServiceResponsePropertyModel where T : class
    {
        public T? Data { get; set; }

        public ServiceResponse()
        {
        }

        private ServiceResponse(bool isSuccess, T? data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static ServiceResponse<T> Success(T data) => new ServiceResponse<T>(true, data, null);

        public static ServiceResponse<T> Success() => new ServiceResponse<T>(true, default, null);

        public static ServiceResponse<T?> Fail(string errorMessage) => new ServiceResponse<T?>(false, default, errorMessage);

        public static ServiceResponse<T?> Fail(Exception exception) => new ServiceResponse<T?>(false, default, $"{exception.Message} : {exception.InnerException}");
    }
}