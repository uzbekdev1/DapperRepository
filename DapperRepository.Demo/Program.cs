using System;
using System.Configuration;
using DapperRepository.Attributes;
using DapperRepository.Demo.Helpers;
using DapperRepository.Drapper;
using DapperRepository.MSSQL;

namespace DapperRepository.Demo
{
    internal class Program
    {
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
    }
}