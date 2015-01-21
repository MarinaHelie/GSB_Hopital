using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Gsb_Hopital_Version1
{
    class Utility
    {
        
        internal static string GetConnectionString()
        {
            string returnValue = null;
            ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["Gsb_Hopital_Version1.Properties.Settings.GSB_HopitalConnectionString"];
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}