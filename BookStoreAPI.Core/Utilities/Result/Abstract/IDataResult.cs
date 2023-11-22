namespace BookStoreAPI.Core.Utilities.Result.Abstract
{
    // IResult interface'indən implement edilmiş bir interface
    public interface IDataResult<T> : IResult
    {
        // T Data adlı özelliğ (property), əməliyyatın nəticəsində əldə edilmiş verini təmsil edir
        T Data { get; }
    }
}
