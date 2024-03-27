using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Sync
{
    public class Credentials
    {
        private string login;
        private string password;
        private string appId;
        private string appName;

        [Obsolete("Up from 2.3.3 login is not a long, but string")]
        public Credentials(long login, string password, string appId = "", string appName = "")
        {
            this.login = login.ToString();
            this.password = password;
            this.appId = appId;
            this.appName = appName;
        }

        public Credentials(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public Credentials(string login, string password, string appId, string appName)
        {
            this.login = login;
            this.password = password;
            this.appId = appId;
            this.appName = appName;
        }

        public string Login
        {
            get
            {
                return login;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

        public string AppId
        {
            get
            {
                return appId;
            }
            set
            {
                appId = value;
            }
        }

        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                appName = value;
            }
        }

        public override string ToString()
        {
            return "Credentials [login=" + Login + ", password=" + Password + "]";
        }
    }
}
