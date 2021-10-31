using System;
using System.Reflection;
using CustomTowerDefense;

namespace TowerDefenseForWindows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TowerDefenseGame())
            {
                game.Run();
            }
        }
    }
}