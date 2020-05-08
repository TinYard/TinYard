using System;
using System.Collections.Generic;
using System.Text;
using TinYard.API.Interfaces;

namespace TinYard.Extensions.Logging
{
    public class LoggingExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;
        }
    }
}
