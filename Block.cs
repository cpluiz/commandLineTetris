using System;
using System.Linq;
using System.Collections.Generic;
using Extensions;

namespace Tetris {
    public class Block {
        public enum Shape {
             I = 1,
             O = 2,
             T = 3,
             S = 4,
             Z = 5,
             L = 6,
             J = 7
        };
        //posX e posY equivalem ao canto superior direito da peça
        public int PosX, PosY, rotation;
        public char[] tetromino;

        /// <summary>
        /// Define o formato do bloco (tetromino) a ser utilizado
        /// </summary>
        /// <param name="shape">enum com o formato da peça a ser utilizada</param>
        public Block(int startPosX, int startPosY){
            this.PosX = startPosX;
            this.PosY = startPosY;
            this.rotation = 0;
            Shape shape = EnumExtensions.GetRandomEnumValue<Shape>();
            tetromino = new char[16];
            List<char> tempShape = new List<char>();
            switch(shape) {
                case Shape.I:
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    break;
                case Shape.O:
                    tempShape.AddRange("zzzz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                case Shape.T:
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                case Shape.S:
                    tempShape.AddRange("zxzz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                case Shape.Z:
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zxzz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                case Shape.L:
                    tempShape.AddRange("zxzz".ToCharArray());
                    tempShape.AddRange("zxzz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                case Shape.J:
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zzxz".ToCharArray());
                    tempShape.AddRange("zxxz".ToCharArray());
                    tempShape.AddRange("zzzz".ToCharArray());
                    break;
                default:
                    break;
            }
            tetromino = tempShape.ToArray<char>();
        }

        /// <summary>
        /// Rotaciona a peça de acordo com a rotação (r) sendo que
        /// r = 0 - Sem rotação (0 graus)
        /// r = 1 - Rotação de 90 graus
        /// r = 2 - Rotação de 180 graus
        /// r = 3 - Rotação de 270 graus
        /// </summary>
        /// <param name="px">coordenada X interna da peça a ser rotacionada</param>
        /// <param name="py">coordenada Y interna da peça a ser rotacionada</param>
        /// <param name="r">rotação</param>
        /// <returns>Posição do símbolo correspondente a rotação atual do objeto para a coordenada (x,y) informada</returns>
        public int Rotate(int px, int py) {
            // r % 4 restringe o valor atual da rotação dentro dos valores permitidos
            switch(this.rotation % 4) {
                case 0: return py * 4 + px;
                case 1: return 12 + py - (px * 4);
                case 2: return 15 - (py * 4) - px;
                case 3: return 3 - py + (px * 4);
            }
            return 0;
        }
    }
}