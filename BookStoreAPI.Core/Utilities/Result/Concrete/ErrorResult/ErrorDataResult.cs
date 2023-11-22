namespace BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // Constructor metodları, ErrorDataResult'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
        }

        public ErrorDataResult(T data) : base(data, false)
        {
        }

        public ErrorDataResult(string message) : base(default, false, message)
        {
        }

        public ErrorDataResult() : base(default, false)
        {
        }
    }
}
