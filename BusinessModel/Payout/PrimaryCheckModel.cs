using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Payout
{
    public class PrimaryCheckModel
    {
        public string statuscode { get; set; } = string.Empty;
        public string statusdesc { get; set; } = string.Empty;
        public string PreferenceAction { get; set; } = string.Empty;
    }

}
