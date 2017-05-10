using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;

namespace Service
{
    public interface ICommand
    {
        IList<IEvent> CommandCompletedEvents { get; }
    }
}
