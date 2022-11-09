using MarsRoverProject.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MarsRoverProject.Domain.Models
{
    public class Rover : IRover
    {
        private IMap _map;

        public IPosition CurrentPosition { get; private set; }

        public IList<IPosition> RouteLog { get; private set; }

        public Rover(IMap map, IPosition landingPosition)
        {
            if (map.IsPositionInsideMap(landingPosition))
            {
                _map = map;
                RouteLog = new List<IPosition>();
                CurrentPosition = landingPosition;
                RouteLog.Add(new Position(CurrentPosition.X, CurrentPosition.Y, CurrentPosition.Orientation));                
            }
            else
            {
                throw new ArgumentException("Rover's landing position is outside the map");
            }
        }

        /// <summary>
        /// Change rover's position by providing a set of Commands
        /// </summary>
        /// <param name="commands"></param>
        public void Navigate(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
            {
                ExecuteCommand(command);
                RouteLog.Add(new Position(CurrentPosition.X, CurrentPosition.Y, CurrentPosition.Orientation));
            }
        }

        /// <summary>
        /// Update rover's current position
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ExecuteCommand(Command command)
        {
            switch (command)
            {
                case Command.L:
                    SpinLeft();
                    break;

                case Command.R:
                    SpinRight();
                    break;

                case Command.M:
                    MoveForward();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, "Unsupported command");
            }
        }

        /// <summary>
        /// Spin left by 90 degrees
        /// </summary>
        private void SpinLeft()
        {
            switch (CurrentPosition.Orientation)
            {
                case Orientation.N:
                    CurrentPosition.Orientation = Orientation.W;
                    break;

                case Orientation.E:
                    CurrentPosition.Orientation = Orientation.N;
                    break;

                case Orientation.S:
                    CurrentPosition.Orientation = Orientation.E;
                    break;

                case Orientation.W:
                    CurrentPosition.Orientation = Orientation.S;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation), CurrentPosition.Orientation, "Unsupported orientation");
            }
        }

        /// <summary>
        /// Spin right by 90 degrees
        /// </summary>
        private void SpinRight()
        {
            switch (CurrentPosition.Orientation)
            {
                case Orientation.N:
                    CurrentPosition.Orientation = Orientation.E;
                    break;

                case Orientation.E:
                    CurrentPosition.Orientation = Orientation.S;
                    break;

                case Orientation.S:
                    CurrentPosition.Orientation = Orientation.W;
                    break;

                case Orientation.W:
                    CurrentPosition.Orientation = Orientation.N;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation), CurrentPosition.Orientation, "Unsupported orientation");
            }
        }

        /// <summary>
        /// Move rover forward by one grid point maintaining the same orientation
        /// </summary>
        private void MoveForward()
        {
            IPosition newPosition = null;
            switch (CurrentPosition.Orientation)
            {
                case Orientation.N:
                    newPosition = new Position(CurrentPosition.X, CurrentPosition.Y + 1, CurrentPosition.Orientation);
                    break;

                case Orientation.E:
                    newPosition = new Position(CurrentPosition.X + 1, CurrentPosition.Y, CurrentPosition.Orientation);
                    break;

                case Orientation.S:
                    newPosition = new Position(CurrentPosition.X, CurrentPosition.Y - 1, CurrentPosition.Orientation);
                    break;

                case Orientation.W:
                    newPosition = new Position(CurrentPosition.X - 1, CurrentPosition.Y, CurrentPosition.Orientation);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation), CurrentPosition.Orientation, "Unsupported orientation");
            }

            if (_map.IsPositionInsideMap(newPosition))
            {
                CurrentPosition = newPosition;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Position), newPosition, "Position is outside the map");
            }
        }
    }
}