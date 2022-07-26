namespace Authentication_clone.Db
{
    public interface IRepo<T>
    {
        public Task<T?> GetById(int Id);
        public Task Delete(int Id);
        public Task<T> Update(int Id, T newData);
        public Task<T> Add(T obj);
    }
}
