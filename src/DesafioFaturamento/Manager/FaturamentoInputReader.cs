using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesafioFaturamento.Domain;

namespace DesafioFaturamento.Manager
{
    public static class FaturamentoInputReader
    {
        /// <summary>
        /// Le e chama a validacao linha do arquivo de entrada, e tem como saida os eventos validos agrupados por maquina
        /// </summary>
        /// <param name="input">Nome do arquivo com as entradas</param>
        /// <returns>List de maquinas com seus eventos validos</returns>
        public static List<MachineEvents> ReadVestsFromFile(InputParameters inputParameters)
        {
            string fileName = inputParameters.FileName;
            List<MachineEvents> result = new List<MachineEvents>();

            bool skipFirstLine = true;
            foreach (string line in File.ReadLines(fileName,System.Text.Encoding.Unicode))
            {
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    continue;
                }
                var invoiceInput = FaturamentoValidator.ValidateInputLine(line);
                if (invoiceInput != null)
                {
                    MachineEvents machineEvents = result.FirstOrDefault(x => string.Equals(x.Id, invoiceInput.Id, StringComparison.CurrentCultureIgnoreCase));

                    if (machineEvents == null)
                    {
                        machineEvents = new MachineEvents
                        {
                            Id = invoiceInput.Id,
                        };
                        result.Add(machineEvents);
                    }
                    machineEvents.Events.Add(new InvoiceEvents { EventBeginDate = invoiceInput.EventBeginDate, EventEndDate = invoiceInput.EventEndDate, Type = invoiceInput.Type, Value = invoiceInput.Value });
                }
            }

            //Validate machine events, if the validation fails, all machine data will be ignored.
            FaturamentoValidator.ValidateMachineEvents(result);
            return result;
        }
    }
}
