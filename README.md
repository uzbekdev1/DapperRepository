Welcome to the DapperRepository wiki!

This Repository cloning and changed to Drapper ORM micro framework.

Dapper .NET source - https://github.com/elyor0529/dapper-dot-net
Dapper Extensions - https://github.com/elyor0529/Dapper-Extensions

And I this two libs in changes table , wrote here:

I fix to this issue list:

 Type Mapping
 Drop table
 Exists table
 Create table
 DbFactory


Demonstration

Model:

 public class TempModel
    {
        public TempModel()
        {
            LastModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }

        public long ID { get; set; }

        public string Version { get; set; }

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
            Map(f => f.Version).Column("Version");
            Map(f => f.Name).Column("Name");
            Map(f => f.CreatedDate).Column("CreatedDate");
            Map(f => f.LastModifiedDate).Column("LastModifiedDate");

            Schema("");
        }
    }

Connection Strings:

 <appSettings>
    <add key="ConnectionString" value="Data Source=localhost;Initial Catalog=TempDb;User Id=sa;Password=web@1234"/>
  </appSettings>

Migrations:

 var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

   RepositoryHelper.ConfigureRepository(new SqlServerManager(connectionString));
   MigrationHelper.Configure<TempModel>();

Test:


            using (var manager = RepositoryHelper.GetManager())
            {
                //exists
                if (manager.Database.Exists<TempModel>())
                {
                    //drop                    
                    manager.Database.Drop<TempModel>();
                }

                //create
                manager.Database.Create<TempModel>();

                //insert
                var bv = new TempModel
                {
                    Name = "Aji buji"
                };
                var bvId = manager.Database.Insert(bv);

                //get
                bv = manager.Database.Get<TempModel>(bvId);

                //update
                bv.Name = "Buji aji";
                bv.CreatedDate = DateTime.Now;
                bv.LastModifiedDate = DateTime.Now;
                manager.Database.Update(bv);

                //delete
                manager.Database.Delete(bv);

                //all 
                var bvs = manager.Database.GetList<TempModel>();
                foreach (var item in bvs)
                    Console.WriteLine(item.Name);

                //filter
                var predicate = Predicates.Field<TempModel>(model => model.Name, Operator.Like, "ji");
                bvs = manager.Database.GetList<TempModel>();
                foreach (var item in bvs)
                    Console.WriteLine(item.Name);

            }
