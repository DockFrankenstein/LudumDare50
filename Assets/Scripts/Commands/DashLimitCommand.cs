using System.Collections.Generic;
using Game.Player;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class DashLimitCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "dash_limit";
        public override string Description { get; } = "changes the limit of dashes";
        public override string[] Aliases { get; } = new string[] { "dashlimit" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            if (PlayerReference.Singleton?.move == null)
            {
                LogError("Singleton not assigned. Please load a level first!");
                return;
            }

            switch (args.Count)
            {
                case 1:
                    Log($"Dash limit: {PlayerReference.Singleton.move.dashLimit}", "info");
                    break;
                case 2:
                    if (!int.TryParse(args[1], out int value))
                    {
                        ParseException(args[1], "int");
                        return;
                    }

                    PlayerReference.Singleton.move.dashLimit = value;
                    Log($"Dash limit has been changed to {value}!", "cheat");
                    break;
            }
        }
    }
}