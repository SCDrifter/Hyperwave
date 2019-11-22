using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public interface ISourceInfo
    {
        ViewAccount Account { get; }
        ViewLabel Label { get; }

        IAccountSource AccountSource { get; }
        ILabelSource LabelSource { get; }
    }
}
