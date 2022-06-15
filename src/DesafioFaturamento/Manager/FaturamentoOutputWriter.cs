using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFaturamento.Domain;

namespace DesafioFaturamento.Manager
{
    public static class FaturamentoOutputWriter
    {

        public static void PrintToConsole(List<InvoiceOutput> vestings, int precision = 2)
        {
            Console.WriteLine("id, valor_faturado");
            foreach (var item in vestings.OrderBy(x => x.Id))
            {
                string outDecimalValue = FaturamentoUtils.ToStringWithDecimalPlaces(item.Value, precision);
                Console.WriteLine($"{item.Id}, {outDecimalValue}");
            }
        }
    }
}
