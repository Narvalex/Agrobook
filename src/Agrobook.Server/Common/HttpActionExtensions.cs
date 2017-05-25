using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Agrobook.Server
{
    public static class HttpActionExtensions
    {
        /// <summary>
        /// Returns null if not valid.
        /// </summary>
        public static string GetToken(this HttpActionContext context)
        {
            string token = null;
            IEnumerable<string> values;
            if (context.Request.Headers.TryGetValues("Authorization", out values))
            {
                if (values.Count() == 1)
                {
                    AuthenticationHeaderValue tokenDescriptor;
                    if (AuthenticationHeaderValue.TryParse(values.First(), out tokenDescriptor))
                    {
                        return tokenDescriptor.Parameter;
                    }
                }
            }

            return token;
        }
    }
}
