namespace MarsRoverProject.Domain.Models
{
    public interface IMap
    {
        /// <summary>
        /// The width of the map
        /// </summary>
        uint Width { get; }

        /// <summary>
        /// The height of the map
        /// </summary>
        uint Height { get; }

        /// <summary>
        /// Check if the position is inside the map
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool IsPositionInsideMap(IPosition position);
    }
}