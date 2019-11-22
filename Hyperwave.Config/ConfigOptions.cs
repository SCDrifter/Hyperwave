using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Config
{
    public enum SkinStyle
    {
        Gallente,
        Amarr,
        Minmatar,
        Caldari
    }

    public enum EmailReadAction
    {
        Manually,
        AfterMessageLeave,
        BeforeMessageLoad
    }
}
