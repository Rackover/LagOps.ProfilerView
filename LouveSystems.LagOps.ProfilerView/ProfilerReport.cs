using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouveSystems.LagOps.ProfilerView
{

    [Serializable]
    struct ProfilerReport
    {
        public const byte REPORT_VERSION = 1;

        public byte Version;
        public List<ProfilerTickReport> Reports;

        public ProfilerReport(List<ProfilerTickReport> reports)
        {
            Version = REPORT_VERSION;
            Reports = reports;
        }
    }
}
