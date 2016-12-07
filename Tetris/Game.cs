using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{


    public class Game
    {

        private int score = 0;
        public int Score
        {
            get { return score; }
        }

        private int W, H;
        public int Width
        {
            get { return W; }
        }

        public int Height
        {
            get { return H; }
        }

        public int[,] Board;
   

        public Game(int w, int h)
        {
            W = w;
            H = h;
            Board = new int[W, H];
            score = 0;
        }

        public void CheckFullLines()
        {
            int success_lines = 0;      // score increment depends on it
            for (int i = 0; i < H; ++i) // for each line from bottom to top
            {
                int cnt = 0;
                for (int j = 0; j < W; ++j)
                    if (Board[j, i] == 1)
                        ++cnt;
                if (cnt == W) // line is full
                {
                    ++success_lines;
                    // move all static blocks down
                    for (int k = i + 1; k < H; ++k)
                        for (int m = 0; m < W; ++m)
                            Board[m, k - 1] = Board[m, k];
                }
            }
            switch (success_lines)
            {
                case 1: score++; break;
                case 2: score += 2; break;
                case 3: score += 4; break;
                case 4: score += 7; break;
            }
        }

        public void Update()
        {
            for (int i = 0; i < H; ++i)
            {
                bool stop_signal = false;
            
            }
        }

      //  const string[] Figures = new string[] { "fef" };
        public void TryToPostFigure()
        {
            int center = W / 2;
            Board[center, H - 1] = 2;
        }
    }
}
