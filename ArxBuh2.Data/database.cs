using System.Collections.Generic;
using System.Linq;
using ArxBuh2.Data.Entity;
using NDatabase;

namespace ArxBuh2.Data
{
    public class Database
    {
        private const string _dbName = "arxBuh2.db";

        public static IEnumerable<T> GetMany<T>()
        {
            using (var odb = OdbFactory.Open(_dbName))
            {
                return odb.AsQueryable<T>().ToList();
            }
        }

        public static void Create<T>(T item) where T : class, IBaseEntity
        {
            using (var odb = OdbFactory.Open(_dbName))
            {
                odb.Store(item);
                odb.Commit();
            }
        }

        public static void DeleteItem<T>(T item) where T : class, IBaseEntity
        {
            using (var odb = OdbFactory.Open(_dbName))
            {
                var oid = OIDFactory.BuildObjectOID(item.ObjectId);

                odb.Delete((T)odb.GetObjectFromId(oid));
            }
        }
    }
}
