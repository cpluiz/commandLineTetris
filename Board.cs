using System;

namespace Tetris {
    public class Board {
        int width, height;
        public int Width{get{return this.width;}}
        public int Height{get{return this.height;}}
        char[] playField;
        const char WALL_SYMBOL = '#';
        const char BLOCK_PICE_SYMBOL = 'x';
        const char EMPTY_SPACE_SYMBOL = 'z';
        /// <summary>
        /// Define um novo tabuleiro com o tamanho padrão similar as dimensões da versão do GameBoy
        /// </summary>
        public Board() {
            this.width = 12;
            this.height = 18;
            PopulateBoard();
        }

        /// <summary>
        /// Define um novo tabuleiro com dimensões personalizadas
        /// </summary>
        /// <param name="width">largura do tabuleiro</param>
        /// <param name="height">altura do tabuleiro</param>
        public Board(int width, int height) {
            this.width = width;
            this.height = height;
            PopulateBoard();
        }

        /// <summary>
        /// Prepara os valores referentes ao tabuleiro
        /// </summary>
        void PopulateBoard() {
            playField = new char[width * height];
            //Inicializa o tabuleiro com as bordas e valores em branco
            for(int x = 0;x < width;x++)
                for(int y = 0;y < height;y++)
                    playField[y * width + x] = (x == 0 || x == width - 1 || y == height - 1) ? WALL_SYMBOL : EMPTY_SPACE_SYMBOL;
        }
        public void DrawBoard() {
            Console.CursorVisible = false;
            //Console.Clear();
            Console.SetCursorPosition(0,0);
            for(int y = 0;y < height;y++) {
                for(int x = 0;x < width;x++) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = (playField[y * width + x] == EMPTY_SPACE_SYMBOL) ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write("{0} ",playField[y * width + x]);
                }
                Console.WriteLine();
            }
        }

        public void DrawBlock(Block block){
            if(block == null) return;
            for(int bx=0; bx < 4; bx++){
                for(int by=0; by < 4; by++){
                    //Primeiro, pegamos o valor da forma nas coordenadas atuais da peça,
                    //levando em consideração sua rotação
                    int shapeIndex = block.Rotate(bx, by);
                    //Em seguida pegamos a posição desse bloco de acordo com o tabuleiro
                    //do jogo
                    int boardIndex = (block.PosY + by) * width + (block.PosX + bx);
                    if(block.tetromino[shapeIndex] != EMPTY_SPACE_SYMBOL)
                        playField[boardIndex] = BLOCK_PICE_SYMBOL;
                }
            }
        }

        public void DrawNextBlock(Block block){
            Console.SetCursorPosition(this.width*2, 3);
            Console.Write("Next pice:");
            for(int bx=0; bx < 4; bx++){
                for(int by=0; by < 4; by++){
                    int shapeIndex = block.Rotate(bx, by);
                    Console.SetCursorPosition(this.width*2+bx,by+5);
                    Console.ForegroundColor = (block.tetromino[shapeIndex] == EMPTY_SPACE_SYMBOL) ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write("{0} ",block.tetromino[shapeIndex]);
                }
            }
        }

        public void DrawScore(int score){
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(this.width*2, 0);
            Console.Write("Score: {0}", score.ToString());
        }

        public void ClearBlock(Block block){
            if(block == null) return;
            for(int bx=0; bx < 4; bx++){
                for(int by=0; by < 4; by++){
                    //Primeiro, pegamos o valor da forma nas coordenadas atuais da peça,
                    //levando em consideração sua rotação
                    int shapeIndex = block.Rotate(bx, by);
                    //Em seguida pegamos a posição desse bloco de acordo com o tabuleiro
                    //do jogo
                    int boardIndex = (block.PosY + by) * width + (block.PosX + bx);
                    if(block.tetromino[shapeIndex] != EMPTY_SPACE_SYMBOL)
                        playField[boardIndex] = EMPTY_SPACE_SYMBOL;
                }
            }
        }

        public bool ValidBlockLocation(Block block, int nextX, int nextY){
            for(int bx=0; bx < 4; bx++){
                for(int by=0; by<4; by++){
                    //Primeiro, pegamos o valor da forma nas coordenadas atual da peça,
                    //levando em consideração sua rotação
                    int shapeIndex = block.Rotate(bx, by);
                    //Em seguida pegamos a posição desse bloco de acordo com o tabuleiro
                    //do jogo
                    int boardIndex = (nextY + by) * width + (nextX + bx);
                    if(nextX + bx >=0 && nextX + bx < width){
                        if(nextY + by >= 0 && nextY + by < height){
                            //Se o índice atual da peça for sólido e o índice do tabuleiro não for sólido
                            //ocorreu uma colisão, então não é necessário mais nenhuma verificação
                            if(block.tetromino[shapeIndex] == BLOCK_PICE_SYMBOL && playField[boardIndex] != EMPTY_SPACE_SYMBOL){
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public int CheckFullLine(int boardY, int checkedLines = 0, int clearStreak = 0){
            bool cleanedLine = false;
            for(int y = boardY+3; y >= boardY+checkedLines ;--y) {
                if(y >= this.height-1) continue;
                bool hasEmpty = false;
                //Ignore board walls
                for(int x = 1;x < width-1;x++) {
                    hasEmpty |= playField[y * width + x] == EMPTY_SPACE_SYMBOL || playField[y * width + x] == WALL_SYMBOL;
                    if(hasEmpty) break;
                }
                if(!hasEmpty){
                    Console.SetCursorPosition(0, height+5);
                    clearStreak ++;
                    ClearLine(y, checkedLines);
                    cleanedLine = true;
                    break;
                }
            }
            if(cleanedLine)
                clearStreak = CheckFullLine(boardY, ++checkedLines, clearStreak);
            return clearStreak;
        }
        private void ClearLine(int line, int checkedLines){
            for(int y = line; y >= line-4+checkedLines; y--){
                //Ignore board walls
                for(int x = 1;x < width-1;x++) {
                    playField[y * width + x] = playField[(y-1) * width + x];
                }
            }
        }
    }
}