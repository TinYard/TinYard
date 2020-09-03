using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.ViewController.Impl.Base
{
    public class ViewRegister : IViewRegister
    {
        #region Singleton Impl

        public static ViewRegister Instance { get; private set; }

        private static readonly object _creationLock = new object();

        public ViewRegister(IContext context)
        {
            _context = context;
            
            if(Instance == null)
            {
                lock (_creationLock)
                {
                    Instance = this;
                }

                OnViewRegister += InjectView;
            }
        }

        #endregion
        
        public static event Action<IView> OnViewRegister;
        public event Action<IView> OnViewRegistered
        {
            add { OnViewRegister += value; }
            remove { OnViewRegister -= value; }
        }

        private IContext _context;
        private HashSet<IView> _registeredViews = new HashSet<IView>();
        public IReadOnlyList<IView> RegisteredViews { get { return _registeredViews.ToList().AsReadOnly(); } }

        public static bool Register(IView view)
        {
            bool registered = false;
            registered = Instance?._registeredViews.Add(view) ?? false;

            if(registered)
            {
                OnViewRegister?.Invoke(view);
            }

            return registered;
        }

        private void InjectView(IView view)
        {
            // DISCUSS: Do we want to inject into Views?
            _context.Injector.Inject(view);
        }
    }
}
