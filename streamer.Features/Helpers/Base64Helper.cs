using System;
using System.Text;

namespace streamer.Features.Helpers
{
    public static class Base64Helper
    {
        public static string Encode(string toEncode)
        {
            var bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            var toReturn = Convert.ToBase64String(bytes);
            return toReturn;
        }

        public static string Decode(string toDecode)
        {
            var toReturn = Encoding.GetEncoding(28591).GetString(Convert.FromBase64String(toDecode));
            return toReturn;
        }
    }
}
