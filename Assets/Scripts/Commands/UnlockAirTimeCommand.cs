using System.Collections.Generic;
using Game.Player;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class UnlockAirTimeCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "unlock_air_time";
        public override string Description { get; } = "unlocks/locks air time";
        public override string[] Aliases => new string[] { "unlockairtime", "lockairtime", "lock_air_time" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            bool state = false;
            switch (args.Count)
            {
                //no args
                case 1:
                    state = !PlayerMovement.UnlockAirTime;
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

            PlayerMovement.UnlockAirTime = state;
            Log($"Air time has been {(state ? "unlocked" : "locked")}!", "cheat");
        }
    }
}