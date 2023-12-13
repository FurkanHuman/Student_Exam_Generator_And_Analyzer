namespace DataAccess.Core;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
