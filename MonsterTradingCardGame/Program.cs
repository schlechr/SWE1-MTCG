using System;
using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * create Player
             * show and select player options.
             */

            /*Console.WriteLine("Hello World!");

            Monster Test = new Monster();
            Console.WriteLine(Test.Lost(1));

            NewPlayer user = new NewPlayer();
            int result;
            while (true)
            {
                result = user.ShowMenu();

                if( result == 1 )
                {
                    user.buyPackage();
                    Console.ReadLine();
                }
                else if ( result == 2 )
                {
                    Console.WriteLine("You are done, press Enter to continue!");
                    Console.ReadLine();
                    return;
                }
                Console.Clear();
            }*/


            Console.WriteLine("Started server at port 10001");
            HTTPServer server = new HTTPServer(10001);
            server.Start();
        }
    }
}
