using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFaturamento.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base()
        {
        }

        public InvalidInputException(string message) : base(message)
        {
        }
    }
}
