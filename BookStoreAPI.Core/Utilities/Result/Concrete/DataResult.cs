using BookStoreAPI.Core.Utilities.Result.Abstract;

namespace BookStoreAPI.Core.Utilities.Result.Concrete
{
    // Result class'ından miras alınmış, IDataResult interface'ini implement etmiş bir class
    public class DataResult<T> : Result, IDataResult<T>
    {
        // T Data adlı özelliğ (property), əməliyyatın nəticəsində əldə edilmiş verini təmsil edir
        public T Data { get; }

        // Constructor metodları, DataResult'ın müxtəlif hallarını yaratmaq üçün istifadə olunur
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }
    }
}
