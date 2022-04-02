using System.Collections.Generic;
using Game.Player;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class ResetDashCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "reset_dash";
        public override string Description { get; } = "resets dash";
        public override string[] Aliases { get; } = new string[] { "resetdash", "dashreset", "dash_reset", "dr" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0)) return;

            if (PlayerReference.Singleton?.move == null)
            {
                LogError("Singleton not assigned. Please load a level first!");
                return;
            }

            PlayerReference.Singleton.move.ResetDash();
            Log("Dash successfully reset!", "cheat");
        }
    }
}