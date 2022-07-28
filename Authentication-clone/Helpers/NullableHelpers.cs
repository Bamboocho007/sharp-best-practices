namespace Authentication_clone.Helpers
{
    public static class NullableHelpers
    {
        public static int? TryParseNullableInt(string? val)
        {
            int number = int.TryParse(val, out number) ? number : number;
            return number;
        }
    }
}
