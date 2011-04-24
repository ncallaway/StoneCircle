using System;

namespace StoneCircle
{
    static public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StoneCircle game = new StoneCircle())
            {
                game.Run();
            }
        }
    }
}

