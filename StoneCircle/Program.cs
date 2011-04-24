using System;

namespace StoneCircle
{
    /// <summary>
    /// Static entry point to the StoneCircle application. Contains a *very* thin main method
    /// which creates and runs a StoneCircle game.
    /// <see cref="StoneCircle"/>
    /// </summary>
    static public class Program
    {
        /// <summary>
        /// Entry point for the application. Creates and runs a StoneCircle game.
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

