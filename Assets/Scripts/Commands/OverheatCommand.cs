using System.Collections.Generic;
using Game.Heat;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class OverheatCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "overheat";
        public override string Description { get; } = "triggers overheat";
        public override string[] Aliases { get; } = new string[] { "oh" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0)) return;

            HeatManager.Heat = 100f;
            Log("Forced overheat!", "cheat");
        }
    }
}