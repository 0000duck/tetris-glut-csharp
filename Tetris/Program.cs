using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Program
    {
        static Game g = new Game(10, 25);

        static void Render()
        {
            Visual.DrawBoard(g);
        }

        static void Keys(char k)
        {
            if (k == 32)
                g.Update();
            if (k == '1')
                g.TryToPostFigure();
        }

        static void Main(string[] args)
        {
            

            Visual.SetRenderCallBack(Render);
            Visual.SetControlCallBack(Keys);
            Visual.Run();
        }
    }
}
