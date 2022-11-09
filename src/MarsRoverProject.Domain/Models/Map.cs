namespace MarsRoverProject.Domain.Models
{
    public class Map : IMap
    {
        /// <summary>
        /// The width of the map
        /// </summary>
        public uint Width { get; private set; }

        /// <summary>
        /// The height of the map
        /// </summary>
        public uint Height { get; private set; }

        public Map(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        ///  Check if the position is inside the map
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsPositionInsideMap(IPosition position)
        {
            return position.X >= 0 && position.X <= Width &&
                   position.Y >= 0 && position.Y <= Height;
        }
    }
}