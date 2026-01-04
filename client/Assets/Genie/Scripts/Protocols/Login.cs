namespace Genie.Protocols
{
    public class Login
    {
        public static readonly string endPoint = "login";
        public class Request
        {
            public string dummy;
        }
        public class Response
        {
            public UserInfo UserInfo;
        }
    }
}