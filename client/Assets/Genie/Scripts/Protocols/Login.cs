using MessagePack;

namespace Genie.Protocols
{
    public class Login
    {
        public static readonly string endPoint = "login";

        [MessagePackObject(false)]
        public class Request
        {
            [Key(0)]
            public string dummy;
        }
        [MessagePackObject(false)]
        public class Response
        {
            [Key(0)]
            public UserInfo UserInfo;
        }
    }
}