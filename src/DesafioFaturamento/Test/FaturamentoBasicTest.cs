using DesafioFaturamento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFaturamento.Test
{
    public static class FaturamentoBasicTest
    {
        public static List<InvoiceOutput> ExpectedResult()
        {
            List<InvoiceOutput> result = new List<InvoiceOutput>();
            result.Add(new InvoiceOutput { Id = "1", Value = 20.00M });
            result.Add(new InvoiceOutput { Id = "2", Value = 10.00M });
            result.Add(new InvoiceOutput { Id = "3", Value = 16.00M });
            result.Add(new InvoiceOutput { Id = "4", Value = 12.00M });
            result.Add(new InvoiceOutput { Id = "5", Value = 8.78M });
            result.Add(new InvoiceOutput { Id = "6", Value = 0.00M });
            result.Add(new InvoiceOutput { Id = "7", Value = 4.00M });
            result.Add(new InvoiceOutput { Id = "8", Value = 7.5M });
            result.Add(new InvoiceOutput { Id = "9", Value = 0.00M });
            result.Add(new InvoiceOutput { Id = "10", Value = 0.00M });
            result.Add(new InvoiceOutput { Id = "11", Value = 0.00M });
            result.Add(new InvoiceOutput { Id = "12", Value = 10.00M });
            result.Add(new InvoiceOutput { Id = "14", Value = 20.00M });
            result.Add(new InvoiceOutput { Id = "15", Value = 20.24M });
            result.Add(new InvoiceOutput { Id = "16", Value = 20.12M });
            return result;
        }

        public static void TestBasicInput(List<InvoiceOutput> result)
        {
            bool found = true;
            StringBuilder sb = new StringBuilder();
            var expectedResult = ExpectedResult();
            foreach (var item in result)
            {
                var invoiceFound = expectedResult.FirstOrDefault(x => x.Id == item.Id);
                if (invoiceFound == null)
                {
                    sb.AppendLine($"item {item.Id} not found.");
                    found &= false;
                }
                if (invoiceFound != null && invoiceFound.Value != item.Value)
                {
                    sb.AppendLine($"item {item.Id} incorrect value {item.Value} !=  {invoiceFound.Value}.");
                    found &= false;
                }
            }
            ConsoleColor previousColor = Console.ForegroundColor;
            if (found)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("All Test OK !");

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tests FAILED !");
                Console.WriteLine(sb.ToString());
            }
            Console.ForegroundColor = previousColor;
        }

        public static InputParameters TestParameters()
        {
            return new InputParameters()
            {
                FileName = @".\data\testInput02.csv",
                IsTesting = true
            };
        }
    }
}
