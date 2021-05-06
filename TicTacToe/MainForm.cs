using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class MainForm : Form
    {
        private IA _bot;
        private Table _game;
        public MainForm()
        {
            InitializeComponent();

            _bot = new IA();
            _game = new Table();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var mouseEA = (MouseEventArgs)e;
            var point = mouseEA.Location;

            var square = GetSquare(point);

            if (_game.GameFinished() || _game.States[square.SquareNumber] != TableState.Neutral)
                return;

            _game.Mark(square, TableState.Cross);

            if (_game.GameFinished())
            {
                table.Refresh();
                return;
            }

            var nextTable = _bot.OptimalMoves[_game];
            var actionSquare = _game.Difference(nextTable);
            _game.Mark(actionSquare, TableState.Zero);

            table.Refresh();
        }

        private TableSquare GetSquare(Point p)
        {
            int row = (int)(p.X * 3 / table.Width);
            int column = (int)(p.Y * 3 / table.Height);

            return new TableSquare(row, column);
        }

        private void table_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);

            g.DrawLine(pen, new Point(0, table.Height / 3), new Point(table.Width, table.Height / 3));
            g.DrawLine(pen, new Point(0, 2 * table.Height / 3), new Point(table.Width, 2 * table.Height / 3));
            g.DrawLine(pen, new Point(table.Width / 3, 0), new Point(table.Width / 3, table.Height));
            g.DrawLine(pen, new Point(2 * table.Width / 3, 0), new Point(2 * table.Width / 3, table.Height));

            g.DrawLine(pen, new Point(0, 0), new Point(table.Width, 0));
            g.DrawLine(pen, new Point(0, 0), new Point(0, table.Height));
            g.DrawLine(pen, new Point(table.Width - 1, 0), new Point(table.Width - 1, table.Height - 1));
            g.DrawLine(pen, new Point(0, table.Height - 1), new Point(table.Width - 1, table.Height - 1));

            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                {
                    if (_game.States[(new TableSquare(i, j)).SquareNumber] == TableState.Cross)
                    {
                        g.DrawLine(pen, new Point(i * table.Width / 3, j * table.Height / 3), new Point((i + 1) * table.Width / 3, (j + 1) * table.Height / 3));
                        g.DrawLine(pen, new Point(i * table.Width / 3, (j + 1) * table.Height / 3), new Point((i + 1) * table.Width / 3, j * table.Height / 3));
                    }
                    if (_game.States[(new TableSquare(i, j)).SquareNumber] == TableState.Zero)
                        g.DrawEllipse(pen, new Rectangle(new Point(i * table.Width / 3, j * table.Height / 3), new Size(table.Width / 3, table.Height / 3)));
                }
        }

        private void table_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
