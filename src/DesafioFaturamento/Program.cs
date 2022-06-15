using DesafioFaturamento.Domain;
using DesafioFaturamento.Exceptions;
using DesafioFaturamento.Manager;
using DesafioFaturamento.Test;
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


        static void Main(string[] args)
        {
            try
            {
                //Read and check the command line parameters
                var validInputParameters = FaturamentoValidator.ValidateInputCommandPrameters(args);
                //Read all file at once ignoring invalid lines
                var vestingInputs = FaturamentoInputReader.ReadVestsFromFile(validInputParameters);
                //Do the vesting calculation based on the requirements
                var result = FaturamentoCalculator.ProcessInvoicings(vestingInputs);
                if (validInputParameters.IsTesting)
                {
                    FaturamentoBasicTest.TestBasicInput(result);
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
