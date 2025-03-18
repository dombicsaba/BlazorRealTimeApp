using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Application.Common.Interfaces
{
    public interface IRealTimeNotifier
    {
        Task NotifyArticlesUpdated();
    }

}
