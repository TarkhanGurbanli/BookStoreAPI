namespace BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult
{
    // Result class'ından miras alınmış bir class
    public class SuccessResult : Result
    {
        // Constructor metodları, SuccessResult'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        public SuccessResult(string message) : base(true, message)
        {
        }

        public SuccessResult() : base(true)
        {

        }
    }
}
