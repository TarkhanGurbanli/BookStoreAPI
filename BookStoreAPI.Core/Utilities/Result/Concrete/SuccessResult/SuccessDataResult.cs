namespace BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult
{
    // DataResult class'ından miras alınmış bir class
    public class SuccessDataResult<T> : DataResult<T>
    {
        // Constructor metodları, SuccessDataResult'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
        }

        public SuccessDataResult(T data) : base(data, true)
        {
        }

        public SuccessDataResult(string message) : base(default, true, message)
        {
        }

        public SuccessDataResult() : base(default, true)
        {
        }
    }
}
