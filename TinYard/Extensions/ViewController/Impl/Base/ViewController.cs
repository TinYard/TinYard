using System.Collections.Generic;
using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.ViewController.Impl.Base
{
    public class ViewController : IViewController
    {
        #region Singleton Impl

        public static ViewController Instance { get; private set; }

        private static readonly object _creationLock = new object();

        public ViewController(IContext context)
        {
            _context = context;

            if(Instance == null)
            {
                lock (_creationLock)
                {
                    Instance = this;
                }
            }
        }

        #endregion

        private IContext _context;
        private HashSet<IView> _registeredViews = new HashSet<IView>();

        public static bool Register(IView view)
        {
            bool registered = Instance._registeredViews.Add(view);

            if(registered)
            {
                Instance._context.Injector.Inject(view);
            }

            return registered;
        }
    }
}
