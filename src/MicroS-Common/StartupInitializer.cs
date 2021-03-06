﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroS_Common
{
    public class StartupInitializer : IStartupInitializer
    {
        private readonly ISet<IInitializer> _initializers = new HashSet<IInitializer>();
        public StartupInitializer()
        {

        }
        public void AddInitializer(IInitializer initializer)
            => _initializers.Add(initializer);

        public async Task InitializeAsync()
            => await Task.WhenAll(_initializers.Select(i => i.InitializeAsync()));
    }
}
