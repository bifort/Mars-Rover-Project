using MarsRoverProject.Domain.Enums;

namespace MarsRoverProject.Domain.Models
{
    /// <summary>
    /// Represents X,Y coordinates with specific Orientation    
    /// </summary>
    public class Position : IPosition
    {
        /// <summary>
        /// The x-axis coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The y-axis coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Orientation (N, E, S, W)
        /// </summary>
        public Orientation Orientation { get; set; }

        public Position(int x, int y, Orientation orientation)
        {
            X = x;
            Y = y;
            Orientation = orientation;
        }
    }
}