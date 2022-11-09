using MarsRoverProject.Domain.Enums;

namespace MarsRoverProject.Domain.Models
{
    public interface IPosition
    {
        /// <summary>
        /// The x-axis coordinate
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// The y-axis coordinate
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// Orientation (N, E, S, W)
        /// </summary>
        Orientation Orientation { get; set; }
    }
}