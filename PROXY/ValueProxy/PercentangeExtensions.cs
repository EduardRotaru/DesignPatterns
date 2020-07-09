namespace PROXY
{
    public static class PercentangeExtensions
    {
        public static Percentange Percent(this int value)
        {
            return new Percentange(value /100.0f); 
        }
        public static Percentange Percent(this float value)
        {
            return new Percentange(value / 100.0f);
        }
    }
}