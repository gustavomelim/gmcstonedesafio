using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFaturamento.Domain
{
    public class EventsInputLine
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public MachineEventTypeEnum Type { get; set; }
        public DateTime EventBeginDate { get; set; }
        public DateTime EventEndDate { get; set; }

    }
}
