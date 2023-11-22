using BookStoreAPI.Core.Utilities.Result.Abstract;

namespace BookStoreAPI.Core.Utilities.Result.Concrete
{
    // IResult interface'indən implement edilmiş bir class
    public class Result : IResult
    {
        // IResult interface'ində təyin olunan Success və Message özəlliklərini implement edən özəlliklər
        public bool Success { get; }
        public string Message { get; }

        // Constructor metodları, Result'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        // this açar sözü, bir constructor-dan başqa bir constructor-u çağırmaq üçün istifadə olunur
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }
    }
}
