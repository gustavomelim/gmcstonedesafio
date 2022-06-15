using System;
using System.Globalization;
using DesafioFaturamento.Domain;

namespace DesafioFaturamento.Manager
{
    public static class FaturamentoUtils
    {
        public static CultureInfo GLOBAL_CULTURE = CultureInfo.InvariantCulture;
        public static DateTimeStyles GLOBAL_DATE_STYLE = DateTimeStyles.None;


        public static string ToStringWithDecimalPlaces(decimal param, int precision)
        {
            //Custom funcion to format decimal as string, and avoid problems with system configurations like decimal place separator
            return param.ToString("N" + precision, GLOBAL_CULTURE).Replace(",", "");
        }

        public static decimal RoundDown(decimal i, int decimalPlaces)
        {
            //Custom funcion to round down a decimal number since natives .Net functions are not reliable
            var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
            return Math.Floor(i * power) / power;
        }

        public static decimal Round(decimal i, int decimalPlaces)
        {
            return Math.Round(i, decimalPlaces,MidpointRounding.AwayFromZero);
        }

        public static MachineEventTypeEnum ParseFaturamentoType(string command)
        {
            switch (command.ToUpper())
            {
                case "ATIVAÇÃO": return MachineEventTypeEnum.ATIVACAO;
                case "DESATIVAÇÃO": return MachineEventTypeEnum.DESATIVACAO;
                case "MUDANÇA DE PREÇO": return MachineEventTypeEnum.MUDANCA;
                case "PERÍODO PROMOCIONAL": return MachineEventTypeEnum.PROMOCAO;
                //If the option is invalid an exception is throw
                default: throw new Exception();
            }
        }
    }
}
