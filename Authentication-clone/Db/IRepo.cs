namespace Authentication_clone.Db
{
    public interface IRepo<T>
    {
        public Task<T?> GetById(int Id);
        public Task<T?> Delete(int Id);
        public Task<T?> Update(T newData);
        public Task<T> Add(T obj);
    }
}
