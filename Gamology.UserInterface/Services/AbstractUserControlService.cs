using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Utils;

namespace Gamology.UserInterface.Services
{
    public abstract class AbstractUserControlService<T> : IUserControlService<T> where T : BindableBase
    {
        private T _model;
        protected T Model => _model;

        public AbstractUserControlService(T model)
        {
            _model = model;
        }

        public abstract void NavigateTo(string destinationView);
    }
}
