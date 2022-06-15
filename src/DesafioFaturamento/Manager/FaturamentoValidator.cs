using DesafioFaturamento.Exceptions;
using System;
using System.Globalization;
using System.IO;
using DesafioFaturamento.Domain;
using System.Collections.Generic;
using System.Linq;
using DesafioFaturamento.Test;

namespace DesafioFaturamento.Manager
{
    public static class FaturamentoValidator
    {
        private static DateTime MIN_ALLOWED_DATE = new DateTime(2021, 01, 01, 00, 00, 00);
        private static DateTime MAX_ALLOWED_DATE = new DateTime(2021, 01, 31, 23, 59, 59);

        public static EventsInputLine ValidateInputLine(string line)
        {
            //Todas as validacoes que falham retornam null e nao lancam uma excecao
            //Dessa forma o programa continua ignorando a linha que nao passou na validacao
            string[] data = line.Split(',');

            if (data == null || data.Length != 5)
            {                
                return null;
            }

            string id = data[0].Trim();

            string valueStr = data[1].Trim();
            decimal eventValue = 0;
            if (!string.IsNullOrWhiteSpace(valueStr))
            {
                if (!decimal.TryParse(valueStr, NumberStyles.Float, FaturamentoUtils.GLOBAL_CULTURE, out eventValue))
                {
                    return null;
                }
            }

            string command = data[2].Trim();
            MachineEventTypeEnum faturamentoType;
            try
            {
                faturamentoType = FaturamentoUtils.ParseFaturamentoType(command);
            }
            catch
            {
                return null;
            }

            string beginDateStr = data[3].Trim();
            if (!DateTime.TryParseExact(beginDateStr, "dd-MM-yyyy", FaturamentoUtils.GLOBAL_CULTURE, FaturamentoUtils.GLOBAL_DATE_STYLE, out DateTime eventBeginDate))
            {
                return null;
            }

            string endDateStr = data[4].Trim();
            DateTime eventEndDate = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(endDateStr))
            {
                if (!DateTime.TryParseExact(endDateStr, "dd-MM-yyyy", FaturamentoUtils.GLOBAL_CULTURE, FaturamentoUtils.GLOBAL_DATE_STYLE, out eventEndDate))
                {
                    return null;
                }
            }

            var result = new EventsInputLine
            {
                Id = id,                
                Value = FaturamentoUtils.Round(eventValue,2),
                Type = faturamentoType,
                EventBeginDate = eventBeginDate,
                EventEndDate =  eventEndDate,
            };
            //return result;

            return ValidateInputRules(result) ? result : null;
        }

        private static bool ValidateInputRules(EventsInputLine inputEvent)
        {
            //A data_final é obrigatoriamente maior ou igual à data_inicial.
            if (inputEvent.EventEndDate > DateTime.MinValue && inputEvent.EventEndDate <= inputEvent.EventBeginDate)
            {
                return false;
            }
            
            //A data_final máxima é 31-01-2021.A data_inicial mínima é 01-01-2021.
            if (inputEvent.EventEndDate > MAX_ALLOWED_DATE)
            {
                return false;
            }
            if (inputEvent.EventBeginDate < MIN_ALLOWED_DATE || inputEvent.EventBeginDate > MAX_ALLOWED_DATE)
            {
                return false;
            }

            //Somente eventos de Período Promocional possuem data_final.
            if (inputEvent.Type == MachineEventTypeEnum.PROMOCAO && inputEvent.EventEndDate == MIN_ALLOWED_DATE)
            {
                return false;
            }
            if (inputEvent.Type != MachineEventTypeEnum.PROMOCAO && inputEvent.EventEndDate > MIN_ALLOWED_DATE)
            {
                return false;
            }

            //Somente eventos de Desativação não possuem preço.
            if (inputEvent.Type == MachineEventTypeEnum.DESATIVACAO && inputEvent.Value != 0)
            {
                return false;
            }

            //O preço só poderar ter valor igual a zero em caso de evento do tipo Período Promocional
            if (inputEvent.Value == 0 && !(inputEvent.Type == MachineEventTypeEnum.DESATIVACAO || inputEvent.Type == MachineEventTypeEnum.PROMOCAO))
            {
                return false;
            }
            return true;
        }

        public static void ValidateMachineEvents(List<MachineEvents> machineEvents)
        {
            //Validacoes desse tipo ignoram todos os eventos,
            //ja que nao daria para precisar com certeza a precisao dos eventos e qual seria o evento valido

            //Não podem existir duas datas iniciais iguais para um mesmo id.
            //Múltiplos eventos de Período Promocional não podem ter interseção entre o período de suas datas.
            //Uma maquininha só poderá ter um evento de Desativação após uma Ativação.
            foreach (var item in machineEvents)
            {
                var orderedEvents  = item.Events.OrderBy(x => x.EventBeginDate).ToList();
                DateTime previousDate = DateTime.MinValue;
                bool activationReceived = false;
                bool valid = true;
                for (int i = 0; i < orderedEvents.Count(); i++)
                {
                    if (orderedEvents[i].Type == MachineEventTypeEnum.ATIVACAO)
                    {
                        activationReceived = true;
                    }
                    if (orderedEvents[i].EventBeginDate == previousDate)
                    {
                        valid = false;
                        break;
                    }
                    if (orderedEvents[i].Type == MachineEventTypeEnum.DESATIVACAO && !activationReceived)
                    {
                        valid = false;
                        break;
                    }
                }
                var promotionsEvents = item.Events.Where(x => x.Type == MachineEventTypeEnum.PROMOCAO).OrderBy(x => x.EventBeginDate).ToList();
                for (int i = 1; i < promotionsEvents.Count(); i++)
                {
                    if (promotionsEvents[i].EventBeginDate <= promotionsEvents[i-1].EventEndDate)
                    {
                        valid = false;
                        break;
                    }
                }
                if (!valid)
                {
                    item.Events = new List<InvoiceEvents>();
                }
            }
        }

        public static InputParameters ValidateInputCommandPrameters(string[] args)
        {
            //expected input: example3.csv
            //First argument should be a valid file
            //Validations fail will throw an exception and the program will abort 
            if (args.Length == 1 && args[0] == "--helpRun")
            {
                throw new InvalidInputException("");
            }
            if (args.Length != 1)
            {
                throw new InvalidInputException("Numero de parametros invalidos.");
            }

            InputParameters result;
            if (!bool.TryParse(args[0], out _))
            {
                string inputFile = args[0];
                if (!File.Exists(inputFile))
                {
                    throw new InvalidInputException($"Arquivo {inputFile} nao encontrado.");
                }
                result = new InputParameters()
                {
                    FileName = inputFile,
                    IsTesting = false,
                };
            }
            else
            {
                result = FaturamentoBasicTest.TestParameters();
            }
            return result;
        }

    }
}
