using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.DatabaseContexts;
using Gamology.UserInterface.Services;
using Gamology.UserInterface.UserControls.Canvas;
using Gamology.UserInterface.UserControls.Tool;
using Gamology.UserInterface.Utils;
using Gamology.UserInterface.Utils.Helpers;
using Microsoft.Practices.Unity;

namespace Gamology.UserInterface
{
    class GameEditorMainWindowViewModel : BindableBase
    {
        private ToolViewModel _toolViewModel;
        private CanvasViewModel _canvasViewModel;

        private BindableBase _toolRegion;
        public BindableBase ToolRegion
        {
            get { return _toolRegion; }
            set { SetProperty(ref _toolRegion, value); }
        }

        private BindableBase _workspaceRegion;
        public BindableBase WorkspaceRegion
        {
            get { return _workspaceRegion; }
            set { SetProperty(ref _workspaceRegion, value); }
        }

        public GameEditorMainWindowViewModel(ICommandHandler commandHandler) 
            : base(commandHandler)
        {
            // NOTE this is very important for database to work
            Database.SetInitializer<SQLServerDbContext>(new DropCreateDatabaseIfModelChanges<SQLServerDbContext>());

            _toolViewModel = ContainerHelper.Container.Resolve<ToolViewModel>();
            ToolRegion = _toolViewModel;

            _canvasViewModel = ContainerHelper.Container.Resolve<CanvasViewModel>();
            WorkspaceRegion = _canvasViewModel;
        }
    }
}
