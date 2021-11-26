using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouveSystems.LagOps.ProfilerView
{
    [Serializable]
    struct ProfilerTickReport
    {
        public List<ActionReport> ActionReports;
        public long TickTime;
        public ulong Tick;
        public bool IsConcerning;

        public ProfilerTickReport(ulong tick, List<ActionReport> actionReports, long tickTime, bool isConcerning)
        {
            this.ActionReports = actionReports;
            this.TickTime = tickTime;
            this.Tick = tick;
            this.IsConcerning = isConcerning;
        }
    }
}
