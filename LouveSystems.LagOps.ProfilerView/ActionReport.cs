using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouveSystems.LagOps.ProfilerView
{

    [Serializable]
    struct ActionReport
    {
        public int Depth;
        public string Action;
        public long TotalMiliseconds;

        public ActionReport(string action, int depth, long time)
        {
            Action = action;
            TotalMiliseconds = time;
            Depth = depth;
        }
    }
}
