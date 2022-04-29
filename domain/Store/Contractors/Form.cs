using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public class Form
    {
        public string UniqueCode { get; }

        public int OrderId { get; }

        public int Step { get; }

        public bool IsFinal { get; }

        public IReadOnlyList<Field> Fields { get; }

        public Form(string code, int orderID, int step, bool isFinal, IReadOnlyList<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            if (step < 0)
                throw new ArgumentOutOfRangeException(nameof(step));

            if(fields == null)
                throw new ArgumentNullException(nameof(fields));

            this.UniqueCode = code;
            this.OrderId = orderID;
            this.Step = step;
            this.IsFinal = isFinal;
            this.Fields = fields.ToArray();
        }
    }
}
