﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using ClientDependency.Core.Config;
using ClientDependency.Core.FileRegistration.Providers;
using System.IO;
using System.Web;

namespace ClientDependency.Core.FileRegistration.Providers
{
    
    public abstract class BaseRenderer : BaseFileRegistrationProvider
    {
        public virtual void RegisterDependencies(List<IClientDependencyFile> allDependencies,
            HashSet<IClientDependencyPath> paths,
            out string jsOutput,
            out string jsPreloadOutput,
            out string cssOutput,
            out string cssPreloadOutput,
            HttpContextBase http)
        {
            WriteDependencies(allDependencies, paths, out jsOutput, out jsPreloadOutput, out cssOutput, out cssPreloadOutput, http);
        }
    }
}

