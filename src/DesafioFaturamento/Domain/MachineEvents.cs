using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFaturamento.Domain
{
    public class MachineEvents
    {
        public string Id { get; set; }
        public List<InvoiceEvents> Events { get; set; } = new List<InvoiceEvents>();
    }
}
