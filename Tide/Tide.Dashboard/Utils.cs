namespace Tide.Dashboard
{
    public static class Utils
    {
        public static readonly int StartCycle = 2019;
        public static readonly int CyclesCount = 4;

        public static readonly string YELLOW_COLOR = "#f2b516";
        public static readonly string BLUE_COLOR = "#1678f2";
        public static readonly string GREEN_COLOR = "#3cd242";


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

        /// <summary>
        /// Maps a number from one range to another range
        /// </summary> 
        public static decimal MapNumberToRange(decimal num, decimal inputMin, decimal inputMax, decimal outputMin, decimal outputMax)
        {
            try
            {
                // Maps a number from one range to another range
                return Convert.ToDecimal(((num - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin));
            }
            catch
            {
                return outputMin;
            }
        }

    }


}
