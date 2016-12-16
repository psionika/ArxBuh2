using NDatabase.Api;

namespace ArxBuh2.Data.Entity
{
    public interface IBaseEntity
    {
        long ObjectId { get; }
    }

    public class baseClass : OID, IBaseEntity
    {
        [OID]
        private readonly long _oid;

        public long ObjectId => _oid;

        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            var oid = obj as OID;
            return CompareTo(oid);
        }

        public int CompareTo(OID oid)
        {
            if (!(oid is baseClass))
                return -1000;

            var otherOid = oid;
            return (int)(ObjectId - otherOid.ObjectId);
        }
    }
}
