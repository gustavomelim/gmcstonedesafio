using System;
using System.Collections.Generic;
using System.Linq;
using DesafioFaturamento.Domain;

namespace DesafioFaturamento.Manager
{
    public static class FaturamentoCalculator
    {
        private static ChargeEvent[] FillDaysDefaultValues()
        {
            ChargeEvent[] result = new ChargeEvent[31];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new ChargeEvent();
            };
            return result;
        }

        /// <summary>
        /// Realiza o calculo de faturamento para cada maquina
        /// </summary>
        /// <param name="input">Fonte dos eventos de faturamento para ser realziado o calculo</param>
        /// <param name="machineEventsList">Lista de eventos agrupados por maquina</param>
        /// <returns>Faturamento total agrupado por maquina</returns>
        public static List<InvoiceOutput> ProcessInvoicings(List<MachineEvents> machineEventsList)
        {
            List<InvoiceOutput> result = new List<InvoiceOutput>();

            foreach (var item in machineEventsList)
            {
                InvoiceOutput output = new InvoiceOutput
                {
                    Id = item.Id,
                    Value = 0
                };
                //Caso só tenha um evento, para o mes inteiro e esse seja de ativacao
                if (item.Events.Count == 1 && item.Events[0].Type == MachineEventTypeEnum.ATIVACAO && item.Events[0].EventBeginDate.Day == 1)
                {
                    output.Value = item.Events[0].Value;
                    result.Add(output);
                    continue;
                }
                ChargeEvent[] defaultValues = FillDaysDefaultValues();

                foreach (var invoicings in item.Events.OrderBy(x => x.EventBeginDate))
                {
                    if (invoicings.Type == MachineEventTypeEnum.ATIVACAO)
                    {
                        ProcessActivation(invoicings, defaultValues);
                        continue;
                    }

                    if (invoicings.Type == MachineEventTypeEnum.DESATIVACAO)
                    {
                        ProcessDeactivation(invoicings, defaultValues);
                        continue;
                    }

                    if (invoicings.Type == MachineEventTypeEnum.MUDANCA)
                    {
                        ProcessChange(invoicings, defaultValues);
                        continue;
                    }

                    if (invoicings.Type == MachineEventTypeEnum.PROMOCAO)
                    {
                        ProcessDiscount(invoicings, defaultValues);
                    }
                }
                decimal[] debugData = defaultValues.Select(x => x.Value).ToArray();

                decimal sumOfValues = defaultValues.Where(x => x.IsActive).Sum(x => x.Value);
                sumOfValues /= 30;
                output.Value = FaturamentoUtils.RoundDown(sumOfValues, 2); ;
                result.Add(output);
            }
            return result;
        }

        private static void ProcessActivation(InvoiceEvents invoicings, ChargeEvent[] defaultValues)
        {
            int dayBegin = invoicings.EventBeginDate.Day - 1;
            for (int i = dayBegin; i < defaultValues.Length; i++)
            {
                defaultValues[i].Day = i + 1;
                defaultValues[i].IsActive = true;
                //Ativacao nao altera se tinha uma promocao no periodo
                if (defaultValues[i].Type != MachineEventTypeEnum.PROMOCAO)
                {
                    defaultValues[i].Type = MachineEventTypeEnum.ATIVACAO;
                    defaultValues[i].Value = invoicings.Value;
                }
            }
        }

        private static void ProcessDeactivation(InvoiceEvents invoicings, ChargeEvent[] defaultValues)
        {
            int dayBegin = invoicings.EventBeginDate.Day - 1;
            for (int i = dayBegin; i < defaultValues.Length; i++)
            {
                defaultValues[i].Day = i + 1;
                defaultValues[i].IsActive = false;
                //Nao altera o valor do dia em caso de desativacao
            }
        }

        private static void ProcessChange(InvoiceEvents invoicings, ChargeEvent[] defaultValues)
        {
            int dayBegin = invoicings.EventBeginDate.Day - 1;
            for (int i = dayBegin; i < defaultValues.Length; i++)
            {
                var previousCharge = defaultValues[i];
                if (previousCharge.Type != MachineEventTypeEnum.PROMOCAO)
                {
                    defaultValues[i].Day = i + 1;
                    defaultValues[i].Type = MachineEventTypeEnum.MUDANCA;
                    defaultValues[i].Value = invoicings.Value;
                    //Nao altera o status de ativo em caso de mudanca
                }
            }
        }

        private static void ProcessDiscount(InvoiceEvents invoicings, ChargeEvent[] defaultValues)
        {
            int dayBegin = invoicings.EventBeginDate.Day - 1;
            int dayEnd = invoicings.EventEndDate.Day;
            for (int i = dayBegin; i < dayEnd; i++)
            {
                defaultValues[i].Day = i + 1;
                defaultValues[i].Type = MachineEventTypeEnum.PROMOCAO;
                defaultValues[i].Value = invoicings.Value;
                //Nao altera o status de ativo em caso de promocao
            }
        }
    }
}
