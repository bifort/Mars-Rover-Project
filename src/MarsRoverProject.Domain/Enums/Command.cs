namespace MarsRoverProject.Domain.Enums
{
    public enum Command
    {
        L = 1, // spin rover by 90 degrees left

        R = 2, // spin rover by 90 degrees right

        M = 3  // move rover forward one grid point and maintain the same heading
    }
}
