using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Shared.ErrorModoule.Exeptions
{
    public class UnAuthorizedExeption : ApplicationException
    {

        public UnAuthorizedExeption(string message) : base(message)
        {

        }
    }
}
