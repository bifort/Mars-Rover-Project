using MarsRoverProject.Domain.Enums;
using MarsRoverProject.Domain.Models;
using System;
using System.Collections.Generic;

namespace MarsRoverProject.Domain.Services
{
    public class MissionControlService
    {
        private IMap _map = null;
        private IList<IRover> _rovers = null;        

        public MissionControlService()
        {
            _rovers = new List<IRover>();
            _map = new Map(5, 5);
        }

        public dynamic ProcessInstructions(string[] lines)
        {
            // parse the instructions 
            foreach(string l in lines)
            {
                string line = l.Replace(" ", string.Empty);
                string[] instructions = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string position = instructions[0];
                string commands = instructions[1];

                IPosition landingPosition = new Position(
                    Convert.ToInt32(position[0].ToString()),
                    Convert.ToInt32(position[1].ToString()),
                    (Orientation)Enum.Parse(typeof(Orientation), position[2].ToString().ToUpper(), true)
                    );

                IList<Command> movementCommands = new List<Command>();
                foreach(char command in commands)
                {
                    movementCommands.Add((Command)Enum.Parse(typeof(Command), command.ToString().ToUpper(), true));
                }

                ExecuteInstructions(landingPosition, movementCommands);                
            }

            return GetRoversRouteLogs();
        }

        private void ExecuteInstructions(IPosition landingPosition, IList<Command> commands)
        {
            IRover rover = DeployRover(landingPosition);
            rover.Navigate(commands);
        }        

        private IRover DeployRover(IPosition landingPosition)
        {
            IRover r = new Rover(_map, landingPosition);
            _rovers.Add(r);
            return r;
        }

        public dynamic GetRoversRouteLogs()
        {
            var rovers = new List<dynamic>();

            for (int i = 0; i < _rovers.Count; i++)
            {
                var rover = _rovers[i];
                var moves = new List<dynamic>();

                foreach (var position in rover.RouteLog)
                {
                    moves.Add(new
                    {
                        x = position.X,
                        y = position.Y,
                        o = position.Orientation.ToString()
                    });
                }

                rovers.Add(new
                {
                    name = $"Rover number {i + 1}",
                    moves = moves
                });
            }

            dynamic payload = new
            {
                status = "OK",
                rovers = rovers
            };

            return payload;
        }
    }
}