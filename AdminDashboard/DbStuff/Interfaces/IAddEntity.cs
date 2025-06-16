namespace AdminDashboard.DbStuff.Interfaces
{
    public interface IAddEntity<DbModel> where DbModel : IBaseModel
    {
        public int Add(DbModel entity);
    }
}
