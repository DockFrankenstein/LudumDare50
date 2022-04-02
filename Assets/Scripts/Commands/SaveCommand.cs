using System.Collections.Generic;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class SaveCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "save";
        public override string Description { get; } = "saves game state";

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0)) return;
            Save.SaveManager.Save();
        }
    }
}