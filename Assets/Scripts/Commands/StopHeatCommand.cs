using System.Collections.Generic;
using Game.Heat;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class StopHeatCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "stop_heat";
        public override string Description { get; } = "toggles heat";
        public override string[] Aliases { get; } = new string[] { "stopheat", "sh" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            bool state = false;
            switch (args.Count)
            {
                //no args
                case 1:
                    state = !HeatManager.StopHeat;
                    break;
                //1 args
                case 2:
                    if (!bool.TryParse(args[1], out state))
                    {
                        ParseException(args[1], "bool");
                        return;
                    }

                    break;
            }

            HeatManager.StopHeat = state;
            Log($"Heat has been {(state ? "disabled" : "enabled")}!", "cheat");
        }
    }
}