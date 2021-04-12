using System;

namespace Tetris
{
    class Program
    {
        static Game game;
        static void Main(string[] args)
        {
            StartNewGame();
        }

        static void StartNewGame(){
            game = new Game();
            Console.Clear();
            while(!game.GameOver){
                //Loop vazio enquanto o jogo está rodando
            }
            //Situação de game over - permite reiniciar o jogo ou sair da aplicação
            while(!Console.KeyAvailable){
                Console.SetCursorPosition(0, 20);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Game Over");
                Console.WriteLine();
                Console.Write("Press ENTER to play again or any key to exit");
            }
            if(Console.ReadKey(false).Key == ConsoleKey.Enter){
                StartNewGame();
            }
        }
        
    }
}
