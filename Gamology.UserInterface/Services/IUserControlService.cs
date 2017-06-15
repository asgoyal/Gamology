using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.Services
{
    public interface IUserControlService<T> where T : BindableBase
    {
        void NavigateTo(string destinationView);
    }
}
