namespace Footage.Application.Presentation
{
    public enum TimeSpanHoursDisplayMode
    {
        Never, // 1h 15min -> 75:00
        WhenNonZero, // 1h 15min -> 01:15:00
        Always // 0h 45min -> 00:45:00
    }
}