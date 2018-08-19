using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckOnClick.Models
{
    public class Appointment
    {
        public DateTime SelectDate { get; set; }

        public int SelectSpecs { get; set; }

        public int SelectDoctor_Id { get; set; }

        public string SelectSlot { get; set; }

        public int Appoint_Id { get; set; }

        public int Doctor_Id { get; set; }

    }
}