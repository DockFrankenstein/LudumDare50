using System.Collections.Generic;
using Game.Heat;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class HeatCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "heat";
        public override string Description { get; } = "changes current heat";

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            switch (args.Count)
            {
                case 1:
                    Log($"Current heat: {HeatManager.Heat}", "info");
                    break;
                case 2:
                    if (!int.TryParse(args[1], out int value))
                    {
                        ParseException(args[1], "int");
                        return;
                    }

                    HeatManager.Heat = value;
                    Log($"Heat level has been changed to {value}!", "cheat");
                    break;
            }
        }
    }
}