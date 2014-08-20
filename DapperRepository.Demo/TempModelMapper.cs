using DapperRepository.Drapper.Mapper;

namespace DapperRepository.Demo
{
    public class TempModelMapper : ClassMapper<TempModel>
    {
        public TempModelMapper()
        {
            Table("BusinessView");

            Map(f => f.ID).Column("Id").Key(KeyType.Identity); 
            Map(f => f.Name).Column("Name");
            Map(f => f.CreatedDate).Column("CreatedDate");
            Map(f => f.LastModifiedDate).Column("LastModifiedDate");

            Schema("");
        }
    }
}