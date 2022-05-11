using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using qASIC.Console.Commands;
using Game.Commentary;

namespace Game.Commands
{
    public class ToggleCommentaryCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "toggle_commentary";
        public override string Description { get; } = "toggles developer commentary on extras";
        public override string[] Aliases { get; } = new string[] { "tc" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            bool state = false;
            switch (args.Count)
            {
                //no args
                case 1:
                    state = !CommentaryController.ShowCommentary;
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

            CommentaryController.ShowCommentary = state;
            Log($"Commentary has been {(state ? "enabled" : "disabled")}!", "cheat"); 
        }
    }
}