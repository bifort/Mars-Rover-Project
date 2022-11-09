using MarsRoverProject.Domain.Enums;
using System.Collections.Generic;

namespace MarsRoverProject.Domain.Models
{
    public interface IRover
    {
        IPosition CurrentPosition { get; }

        IList<IPosition> RouteLog { get; }

        /// <summary>
        /// Change rover's position by providing a set of Commands
        /// </summary>
        /// <param name="commands"></param>
        void Navigate(IEnumerable<Command> commands);        
    }
}