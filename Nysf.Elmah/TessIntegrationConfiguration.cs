#region Bryan's custom NYSF code

//[assembly: Nysf.Elmah.Scc("$Id: TessIntegrationConfiguration.cs 566 2009-05-11 10:37:10Z azizatif $")]

namespace Nysf.Elmah
{
    #region Imports

    using System;
    using System.Collections;

    #endregion

    [ Serializable ]
    internal sealed class TessIntegrationConfiguration
    {
        public static readonly TessIntegrationConfiguration Default;

        private readonly string _tessSessionKeyKey;

        static TessIntegrationConfiguration()
        {
            Default = new TessIntegrationConfiguration((IDictionary) Configuration.GetSubsection("tessIntegration"));
        }
        
        public TessIntegrationConfiguration(IDictionary options)
        {
			_tessSessionKeyKey = options["sessionKeyKey"].ToString();
        }
        
        public string SessionKeyKey
        {
            get { return _tessSessionKeyKey; }
        }
    }
}

#endregion