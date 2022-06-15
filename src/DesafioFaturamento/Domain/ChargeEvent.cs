using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFaturamento.Domain
{
    public class ChargeEvent
    {
        public int Day { get; set; } = 0;
        public bool IsActive { get; set; } = false;
        public MachineEventTypeEnum Type { get; set; } = MachineEventTypeEnum.NONE;
        public decimal Value { get; set; }
    }
}
