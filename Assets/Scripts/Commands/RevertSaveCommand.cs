using System.Collections;
using System.Collections.Generic;
using Game.Save;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class RevertSaveCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "revert_save";
        public override string Description { get; } = "reverts the game's state to a previous version";
        public override string[] Aliases { get; } = new string[] { "revert" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            switch (args.Count)
            {
                case 1:
                    SaveManager.Revert();
                    break;
                case 2:
                    if (!int.TryParse(args[1], out int version))
                    {
                        ParseException(args[1], "int");
                        return;
                    }

                    SaveManager.Revert(version);
                    break;
            }
        }
    }
}