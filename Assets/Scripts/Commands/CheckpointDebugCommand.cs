using System.Collections.Generic;
using Game.Save;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class CheckpointDebugCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "checkpoint_debug";
        public override string Description { get; } = "toggles checkpoint debug";
        public override string[] Aliases { get; } = new string[] { "cd" };
        
        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            bool state = false;
            switch (args.Count)
            {
                //no args
                case 1:
                    state = !Checkpoint.DebugMode;
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

            Checkpoint.DebugMode = state;
            Log($"Checkpoint debug has been {(state ? "enabled" : "disabled")}!", "cheat");
        }
    }
}