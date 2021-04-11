using System;
using System.Threading;

namespace Tetris {
    public class Game {
        Board board;
        Block currentBlock, nextBlock;
        Thread update; 
        AutoResetEvent waitHandle;
        bool gameOver = false;
        bool sendNextBlock = false;
        public bool GameOver{get{return this.gameOver;}}

        private int points = 0;
        const int basePoints = 10;

        public Game() {
            board = new Board();
            points = 0;
            Start();
        }

        void Start(){
            update = new Thread(new ThreadStart(Update));
            update.Priority = ThreadPriority.Normal;
            currentBlock = new Block((board.Width/2)-2, 0);
            nextBlock = new Block((board.Width/2)-2, 0);
            update.Start();
            waitHandle = new AutoResetEvent(false);
        }

        public void Update() { 
            try{
                while(true){
                    board.ClearBlock(currentBlock);
                    if(currentBlock == null){
                        currentBlock = nextBlock;
                        nextBlock = new Block((board.Width/2)-2, 0);
                        if(!board.ValidBlockLocation(nextBlock, nextBlock.PosX, nextBlock.PosY)){
                            this.gameOver = true;
                            update.Interrupt();
                        }
                    }
                    MoveBlock();
                    Thread.Sleep(400);
                    board.DrawBlock(currentBlock);
                    if(sendNextBlock){
                        Console.SetCursorPosition(0, board.Height+4);
                        Console.Write("Checking full lines");
                        this.points += (board.CheckFullLine(currentBlock.PosY) * basePoints);
                        sendNextBlock = false;
                        currentBlock = null;
                    }
                    board.DrawBoard();
                    board.DrawScore(this.points);
                    board.DrawNextBlock(nextBlock);
                    Thread.Sleep(5);
                }
            }catch(Exception){}
        }

        public void MoveBlock(){
            if(Console.KeyAvailable){
                int nextPosX = currentBlock.PosX;
                int nextRotation = currentBlock.rotation;
                ConsoleKey pressedKey = Console.ReadKey(false).Key;
                if( pressedKey == ConsoleKey.UpArrow || pressedKey == ConsoleKey.Z ){
                    nextRotation = currentBlock.rotation+1;
                    currentBlock.rotation = nextRotation;
                    currentBlock.rotation = board.ValidBlockLocation(currentBlock, currentBlock.PosX, currentBlock.PosY) ? nextRotation : nextRotation-1;
                    return;
                }
                nextPosX += (pressedKey == ConsoleKey.LeftArrow) ? -1 : (pressedKey == ConsoleKey.RightArrow ? 1 : 0);
                if(board.ValidBlockLocation(currentBlock, nextPosX, currentBlock.PosY)){
                    currentBlock.PosX = nextPosX;
                }
                return;
            }
            if(board.ValidBlockLocation(currentBlock, currentBlock.PosX, currentBlock.PosY+1))
                currentBlock.PosY ++;
            else
                sendNextBlock = true;
        }

    }
}