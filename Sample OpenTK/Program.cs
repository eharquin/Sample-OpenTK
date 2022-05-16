using System;

namespace Sample_OpenTK
{
    public class Program
    {
        public static void Main(String[] args)
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
