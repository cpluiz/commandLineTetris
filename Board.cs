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

        /// <summary>
        /// Renderiza o tabuleiro na tela, incluindo as paredes e peças que já tenham alcançado
        /// suas posições finais
        /// </summary>
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

        /// <summary>
        /// Renderiza o bloco atual na tela, levando em consideração sua posição e rotação atuais
        /// </summary>
        /// <param name="block">Tetromino/peça atual a ser renderizado</param>
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

        /// <summary>
        /// Renderiza ao lado do tabuleiro a forma do próximo bloco a ser utilizado pelo jogador
        /// </summary>
        /// <param name="width">Próximo tetromino/peça a ser utilizado</param>
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

        /// <summary>
        /// Renderiza o placar atual na tela
        /// </summary>
        /// <param name="score">Pontuação atual do jogador</param>
        public void DrawScore(int score){
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(this.width*2, 0);
            Console.Write("Score: {0}", score.ToString());
        }

        /// <summary>
        /// Remove a forma do bloco atual do tabuleiro antes de avaliar se o próximo movimento
        /// da peça é permitido
        /// </summary>
        /// <param name="block">Tetromino/bloco atual sendo controlado pelo jogador</param>
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

        /// <summary>
        /// Verifica se a próxima movimentação e/ou rotação da peça é permitida
        /// </summary>
        /// <param name="block">Tetromino/bloco atual sendo controlado pelo jogador</param>
        /// <param name="nextX">Próxima coordenada X da peça dentro do tabuleiro do jogo</param>
        /// <param name="nextY">Próxima coordenada Y da peça dentro do tabuleiro do jogo</param>
        /// <returns>Retorna false em caso de colisão, e true caso a ação possa ser executada</returns>
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

        /// <summary>
        /// Verifica se existe uma ou mais linhas completadas após a peça atual não poder
        /// se movimentar além da posição atual;
        /// </summary>
        /// <param name="boardY">Coordenada Y do tabuleiro, de acordo com o canto superior esquerdo da última peça em uso</param>
        /// <param name="checkedLines">Contagem de quantas linhas foram verificadas até o momento</param>
        /// <param name="clearStreak">Contagem de quantas linhas estavam completas, para calcular bônus de pontuação</param>
        /// <returns>Quantidade de linhas completadas para efetuar o calculo de pontuação com bônus por linhas extras concluídas</returns>
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
        /// <summary>
        /// Remove uma linha do tabuleiro, movendo para baixo as linhas imediatamente superiores a ela
        /// </summary>
        /// <param name="line">Coordenada Y da linha a ser removida do tabuleiro</param>
        /// <param name="checkedLines">Contagem de quantas linhas foram verificadas até o momento</param>
        // TODO - movimentar todas as linhas para baixo, ao invés das linhas próximas ao último bloco/tetromino posicionado
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