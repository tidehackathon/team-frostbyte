namespace Tide.Dashboard
{
    public static class Utils
    {
        public static readonly int StartCycle = 2019;
        public static readonly int CyclesCount = 4;

        public static decimal RoundUp(decimal dec)
        {
            int number = Convert.ToInt32(Math.Floor(dec));
            int remainder = number % 10;
            if (remainder == 0)
            {
                return number;
            }
            else
            {
                return number + (10 - remainder);
            }
        }
    }

    
}
