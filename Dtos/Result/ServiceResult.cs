using Microsoft.EntityFrameworkCore.Storage.Json;

namespace TeamTaskManager.Dtos.Result
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }

        public string? Message { get; set; }

        public ServiceError? ErrorType { get; set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                ErrorType = ServiceError.None,
                Data = data
            };
        }
        public static ServiceResult<T> NotFound(string message)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorType = ServiceError.NotFound,
                Message = message
            };
        }

        public static ServiceResult<T> Validation(string message)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorType = ServiceError.Validation,
                Message = message
            };
        }


    }
    
}
