namespace BookStoreAPI.DataAccess.Settings
{
    public interface IDatabaseSettings
    {
        public string BookCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string BasketCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string RoleCollectionName { get; set; }
        public string AppUserRoleCollectionName { get; set; }
        public string OrderCollectionName { get; set; }
        public string WishListCollectionName { get; set; }
        public string PhotoCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
