using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak1.Models
{
    public class SwitchEntity : PowerEntity
    {
        private string status;
        public string Status { get { return status; } set { status = value; } }
    }
}
