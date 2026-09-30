using System;

namespace WebApplication7
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Database.Initialize();
        }
    }
}
