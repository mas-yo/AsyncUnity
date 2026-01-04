namespace Genie.Logics
{
    public static class Serializer
    {
        public static T Deserialize<T>(byte[] data)
        {
            return default(T);
        }

        public static byte[] Serialize<T>(T obj)
        {
            return new byte[]{};
        }
        
    }
}