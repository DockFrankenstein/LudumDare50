using System.Collections.Generic;
using Game.Heat;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class DifficultyCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "difficulty";
        public override string Description { get; } = "changes difficulty";
        public override string[] Aliases { get; } = new string[] { "df" };

        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0, 1)) return;

            switch (args.Count)
            {
                case 1:
                    Log($"Current difficulty: {HeatController.Difficulty}", "info");
                    break;
                case 2:
                    if (!int.TryParse(args[1], out int value))
                    {
                        ParseException(args[1], "int");
                        return;
                    }

                    HeatController.Difficulty = value;
                    Log($"Difficulty level has been changed to {HeatController.Difficulty}!", "cheat");
                    break;
            }
        }
    }
}