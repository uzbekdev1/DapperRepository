Welcome to the DapperRepository wiki!

Simple SQL object mapper for ADO.NET 

Dapper .NET source - https://github.com/elyor0529/dapper-dot-net
Dapper Extensions - https://github.com/elyor0529/Dapper-Extensions

Latest modified by Elyor Latipov - mailto:elyor@outlook.com

I fix to this issue list:

	- Type Mapping
	- Drop table
	- Exists table
	- Create table
	- Create Schema
	- Create Database
	- PK , UQ , NN impleted to every dialects  


Model:

    public class TempModel
    {
        public TempModel()
        {
            LastModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }
        public long ID { get; set; } 
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }


Mapper:

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

Connection Strings:

<!--SQLite-->
<Repository>
  <DriverType>SQLite</DriverType>
  <ConnectionString>Data Source=QueryProcessorStorage.sqlite;Version=3;</ConnectionString>
  <Options>
    <CommandTimeOut>0</CommandTimeOut>
    <DefaultSchema/>
    <MigrationsEnabled>True</MigrationsEnabled>
    <MigrationDataLossAllowed>False</MigrationDataLossAllowed>
    <CreateDatabaseIfNotExists>True</CreateDatabaseIfNotExists>
  </Options> 
</Repository>
 
Test:

      private static IDrapperManager _manager;

            static Program()
            {
                _manager = RepositoryFactory.GetManager();

                RepositoryFactory.SetManager(_manager);
            }

            [Migration(typeof(TempModel))]

            private static void Main(string[] args)
            {
                using (_manager)
                {
                    _manager.Database.Open();

                    //insert
                    var bv = new TempModel
                    {
                        Name = "BV"
                    };
                    var bvId = _manager.Database.Insert(bv);

                    //get
                    bv = _manager.Database.Get<TempModel>(bvId);

                    //update
                    bv.Name = "Workfile1";
                    bv.CreatedDate = DateTime.Now;
                    bv.LastModifiedDate = DateTime.Now;
                    _manager.Database.Update(bv);

                    //delete
                    _manager.Database.Delete(bv);

                    //all 
                    var bvs = _manager.Database.GetList<TempModel>();
                    Console.WriteLine("All:");
                    foreach (var item in bvs)
                        Console.WriteLine(item.Name);

                    //filter
                    var predicate = Predicates.Field<TempModel>(a => a.Name, Operator.Like, "%a%");
                    bvs = _manager.Database.GetList<TempModel>(predicate);
                    Console.WriteLine("Filter(By 'like' operator):");
                    foreach (var item in bvs)
                        Console.WriteLine(item.Name);

                    _manager.Database.Close();
                }
            }
