namespace MagicVilla.Api.Repository.Interface
{
    public interface IRepo<CLASS, IdDataType, RetDataType>
    {
        List<CLASS> Get();
        CLASS Get(IdDataType id);
        RetDataType Create(CLASS obj);
        RetDataType Update(CLASS obj);
        bool Delete(IdDataType id);
    }
}
