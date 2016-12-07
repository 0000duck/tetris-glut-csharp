using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Tao.OpenGl;

using Tao.FreeGlut;

namespace Tetris
{
    public delegate void ControlCallbackDelegate(char key);
    public delegate void CommonCallback();

    static class Visual
    {

        public struct Pt
        {
            public double x, t, r, tp, rp;

            public Pt(double x, double t, double r, double tp, double rp)
            {
                this.x = x; this.t = t; this.r = r;
                this.rp = rp; this.tp = tp;
            }
        }


        private static ControlCallbackDelegate ccd = null;
        private static CommonCallback render = null;

        static void kbdFunc(byte key, int x, int y)
        {
            if (ccd != null)
                ccd((char)key);
            if (key == 27)
                Environment.Exit(0);
        }

        static void Line(double x1, double y1, double x2, double y2)
        {
            Gl.glLineWidth(1.5f);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x2, y2);
            Gl.glEnd();
        }

        static void Rect(double x1, double y1, double x2, double y2, bool fill = false)
        {
            if (!fill)
            {
                Line(x1, y1, x2, y1);
                Line(x1, y1, x1, y2);
                Line(x2, y2, x1, y2);
                Line(x2, y2, x2, y1);
            }
            else
            {
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glVertex2d(x1, y1);
                Gl.glVertex2d(x2, y1);
                Gl.glVertex2d(x2, y2);
                Gl.glVertex2d(x1, y2);
                Gl.glEnd();
            }
        }

        static void Arrow(double x1, double y1, double x2, double y2, double winglen = 3)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double len = Math.Sqrt(dx * dx + dy * dy);
            dx /= len; dy /= len;

            double vp1x = -dy;
            double vp1y = dx;
            double vp2x = dy;
            double vp2y = -dx;

            double v1x = dx + vp1x;
            double v1y = dy + vp1y;
            double v2x = dx + vp2x;
            double v2y = dy + vp2y;
            len = Math.Sqrt(v1x * v1x + v1y * v1y);
            v1x /= len; v1y /= len;
            len = Math.Sqrt(v2x * v2x + v2y * v2y);
            v2x /= len; v2y /= len;


            Gl.glLineWidth(1.5f);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x2, y2);
            Gl.glVertex2d(x2, y2);
            Gl.glVertex2d(x2 - winglen * v1x, y2 - winglen * v1y);
            Gl.glVertex2d(x2, y2);
            Gl.glVertex2d(x2 - winglen * v2x, y2 - winglen * v2y);
            Gl.glEnd();
        }

        const double MenuItSZX = 23;
        const double MenuItSZY = 8;
        public static void MenuIt(double x, double y, string text, bool select)
        {
            if (select)
            {
                Gl.glColor3d(0.3, 0.3, 0.3);
                Rect(x, y, x + MenuItSZX, y + MenuItSZY, true);
                Gl.glColor3d(1, 1, 1);
                Rect(x - 0.1, y - 0.1, 0.1 + x + MenuItSZX, 0.1 + y + MenuItSZY);
            }
            else
            {
                Gl.glColor3d(0, 0, 0);
                Rect(x, y, x + MenuItSZX, y + MenuItSZY);
                Gl.glColor3d(0, 0, 0);
            }

            PrintGL(x + 2, y + MenuItSZY - 6, text, 8);
        }

        public static void PrintGL(double x, double y, string str, double size = 5)
        {
            Gl.glPushMatrix();
            Gl.glTranslated(x, y, 0);
            Gl.glLineWidth((float)(0.2 * size));
            double sz = 0.005f * size;
            Gl.glScaled(sz, sz, sz);

            for (int c = 0; c < str.Length; c++)
                Glut.glutStrokeCharacter(Glut.GLUT_STROKE_ROMAN, (int)str[c]);
            Gl.glPopMatrix();
        }

        static void changeSize(int w, int h)
        {


            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, w, h);
            Glu.gluOrtho2D(0, 100, 150, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        static void renderScene()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glColor3d(0, 0, 0);

            if (render != null)
                render();

            Glut.glutSwapBuffers();

        }

        public static void SetControlCallBack(ControlCallbackDelegate _ccd)
        {
            ccd += _ccd;
        }

        public static void SetRenderCallBack(CommonCallback _render)
        {
            render += _render;
        }

        public static double BoardX = 10, BoardY = 10;
        public static double BlockSz = 5;
        public static void DrawBoard(Game g)
        {
            double Bs04 = 0.4 * BlockSz;
            double cy = BoardY + 0.5 * BlockSz;
            for (int i = g.Height - 1; i >= 0; --i)
            {
                for (int j = 0; j < g.Width; ++j)
                {
                    double cx = BoardX + (j + 0.5) * BlockSz;
                    switch (g.Board[j, i])
                    {
                        case 0:
                            Gl.glColor3d(1, 1, 0);
                            Rect(cx - Bs04, cy - Bs04, cx + Bs04, cy + Bs04);
                            break;
                        case 1:
                            Gl.glColor3d(0, 0, 1);
                            Rect(cx - Bs04, cy - Bs04, cx + Bs04, cy + Bs04, true);
                            break;
                        case 2:
                            Gl.glColor3d(1, 0, 0);
                            Rect(cx - Bs04, cy - Bs04, cx + Bs04, cy + Bs04, true);
                            break;
                    }
                }
                cy += BlockSz;
            }
        }


        public static void Run()
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DEPTH | Glut.GLUT_DOUBLE | Glut.GLUT_RGBA);
            Glut.glutInitWindowPosition(100, 100);
            Glut.glutInitWindowSize(300, 450);
            Glut.glutCreateWindow("Tetris by Tirinox");
            Glut.glutDisplayFunc(renderScene);
            Glut.glutIdleFunc(renderScene);
            Glut.glutKeyboardFunc(kbdFunc);
            Glut.glutReshapeFunc(changeSize);
      

            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glLineWidth(0.5f);
            Gl.glClearColor(1, 1, 1, 1);

            Glut.glutMainLoop();
        }



    }
}
