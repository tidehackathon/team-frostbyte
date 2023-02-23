using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Core
{
    //public class Utils
    //{
    //    #region singleton
    //    private Utils()
    //    {

    //    }
    //    private static Utils? instance = null;
    //    public static Utils Instance
    //    {
    //        get
    //        {
    //            if (instance == null) instance = new Utils();
    //            return instance;
    //        }
    //    }

    //    #endregion

    //    private IServiceResolver? _Services;
    //    public IServiceResolver Services => _Services ?? throw new NotSupportedException("Services not set.");

     
    //    #region configuration
    //    /// <summary>
    //    /// Utils service configuration status.
    //    /// </summary>
    //    private bool Configured = false;

    //    /// <summary>
    //    /// Register services in Utils instance.
    //    /// </summary>
    //    /// <typeparam name="TService"></typeparam>
    //    /// <param name="service"></param>
    //    /// <exception cref="System.InvalidOperationException">Thrown when Utils instance is already configured.</exception>
    //    private void SetService<TService>(TService service) where TService : class
    //    {
    //        if (Configured) throw new System.InvalidOperationException("Utils instance is already configured !");
    //        switch (service)
    //        {
    //            case IServiceResolver serviceResolver: _Services = serviceResolver; break;
    //            case TService _: break;
    //        }
    //    }

    //    /// <summary>
    //    /// Mark configuration as ready.
    //    /// </summary>
    //    private void Configure()
    //    {
    //        Configured = true;
    //    }
    //    #endregion
    //}
}
