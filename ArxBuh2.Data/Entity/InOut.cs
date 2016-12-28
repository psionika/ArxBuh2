using System;

namespace ArxBuh2.Data.Entity
{
    public class InOut : BaseClass
    {
        public InOut()
        {
            CreatedAt = new DateTime(1753, 1, 1);
            Sum = 0.0m;
        }

        public string TypeOperation { get; set; }

        public string Category { get; set; }

        public DateTime? CreatedAt { get; set; }

        public decimal Sum { get; set; }

        public string Comment { get; set; }
    }
}
