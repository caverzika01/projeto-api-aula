namespace _2TDSPK.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToFIAP(this string str)
        {
            return $"{str}@fiap.com.br";
        }
    }
}
