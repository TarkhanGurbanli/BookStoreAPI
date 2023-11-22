namespace BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult
{
    // Result class'ından miras alınmış bir class
    public class ErrorResult : Result
    {
        // Constructor metodları, ErrorResult'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        public ErrorResult(string message) : base(false, message)
        {
        }

        public ErrorResult() : base(false)
        {

        }
    }
}
