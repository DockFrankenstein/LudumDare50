using Game.Player;
using System.Collections.Generic;
using Game.Tools;
using qASIC.Console.Commands;

namespace Game.Commands
{
    public class TestPointCommand : GameConsoleCommand
    {
        public override string CommandName { get; } = "test_point";
        public override string Description { get; } = "teleports to level's test point";
        public override string[] Aliases { get; } = new string[] { "test" };
        
        public override void Run(List<string> args)
        {
            if (!CheckForArgumentCount(args, 0)) return;

            if (PlayerReference.Singleton?.move == null)
            {
                LogError("Player not assigned!");
                return;
            }

            if (TestPoint.Singleton == null)
            {
                LogError("There is no test point in this level!");
                return;
            }

            PlayerReference.Singleton.move.Teleport(TestPoint.Singleton.transform.position);
            Heat.HeatManager.StopHeat = true;
            Save.SaveManager.Save();
        }
    }
}