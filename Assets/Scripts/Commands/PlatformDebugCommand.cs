using System.Collections;
using System.Collections.Generic;
using Game.Logic;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class PlatformDebugCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "platform_debug";
        public override string Description { get; } = "toggles platform debug";
        public override string[] Aliases { get; } = new string[] { "pd" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            bool state = false;
            switch (args.Count)
            {
                //no args
                case 1:
                    state = !Platform.Debug;
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

            Platform.Debug = state;
            Log($"Platform debug has been {(state ? "enabled" : "disabled")}!", "cheat");
        }
    }
}