using DesafioFaturamento.Domain;
using DesafioFaturamento.Exceptions;
using DesafioFaturamento.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesafioFaturamento
{
    internal class Program
    {
        static void PrintErrorMessage(InvalidInputException inputException)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            StringBuilder output = new StringBuilder();
            if (!string.IsNullOrEmpty(inputException.Message))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(inputException.Message);
                Console.WriteLine();
                Console.ForegroundColor = previousColor;
            }

            output.AppendLine("Le o arquivo de entrada com os eventos de faturamento e gera o faturamento mensal para cada maquinha.");
            output.AppendLine("");
            output.AppendLine("dotnet run [drive:][path]filename");
            output.AppendLine("");
            output.AppendLine("\t[drive:][path][filename]\n\tDEfine o drive, diretorio, and arquivo a ser processado.");
            Console.WriteLine(output.ToString());
        }

        static List<InvoiceOutput> ExpectedResult()
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
            return result;
        }

        static void TestBasicInput(List<InvoiceOutput> result)
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

        static void Main(string[] args)
        {
            bool debug = true;
            try
            {
                if (debug)
                {
                    args = new string[] { @"H:\Melim\ProjetosPessoais\CodeChallengesInterviews\stone\DesafioFaturamento\DesafioFaturamento\data\testInput02.csv" };
                }
                //Read and check the command line parameters
                var validInputFile = FaturamentoValidator.ValidateInputCommandPrameters(args);
                //Read all file at once ignoring invalid lines
                var vestingInputs = FaturamentoInputReader.ReadVestsFromFile(validInputFile);
                //Do the vesting calculation based on the requirements
                var result = FaturamentoCalculator.ProcessInvoicings(vestingInputs);
                if (debug)
                {
                    TestBasicInput(result);
                }
                //Print the result
                FaturamentoOutputWriter.PrintToConsole(result);
            }
            catch (InvalidInputException iie)
            {
                PrintErrorMessage(iie);
            }

        }
    }
}
