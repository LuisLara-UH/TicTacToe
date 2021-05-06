using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class IA
    {
        public IA()
        {
            States = new List<TableState>() 
            { 
                TableState.Cross, TableState.Neutral, TableState.Zero
            };
            InitializeTables();
            InitializeOptimalMoves();
        }

        public List<Table> Tables { get; set; }
        public List<TableState> States { get; set; }
        public Dictionary<Table, Table> OptimalMoves { get; set; }

        public void InitializeTables()
        {
            Tables = new List<Table>();
            Table initTable = new Table();
            CalculateTables(0, initTable);
        }

        public void CalculateTables(int pos, Table table)
        {
            if (pos == 9)
            {
                Tables.Add(new Table(table));
                return;
            }

            foreach (var state in States)
            {
                table.Mark(pos, state);
                CalculateTables(pos + 1, table);
            }
        }

        public bool InitializeOptimalMoves()
        {
            OptimalMoves = new Dictionary<Table, Table>();
            return IsOptimalMove(new Table());
        }

        private bool FindIAOptimalMove(Table table)
        {
            if (table.GameFinished())
            {
                return !table.IsWinner(TableState.Cross);
            }

            foreach (var move in table.PossibleMoves(TableState.Zero))
            {
                if (IsOptimalMove(move))
                {
                    OptimalMoves[table] = move;
                    return true;
                }
            }
            return false;
        }

        private bool IsOptimalMove(Table table)
        {
            if (table.GameFinished())
            {
                return !table.IsWinner(TableState.Cross);
            }

            foreach (var move in table.PossibleMoves(TableState.Cross))
            {
                if (!FindIAOptimalMove(move))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Table
    {
        public Table()
        {
            States = new TableState[CantSquares];
            for (int i = 0; i < CantSquares; ++i)
                States[i] = TableState.Neutral;
        }

        public Table(Table table)
        {
            States = new TableState[CantSquares];
            Array.Copy(table.States, States, CantSquares);
        }

        public void Mark(TableSquare square, TableState state)
        {
            States[square.SquareNumber] = state;
        }

        public void Mark(int pos, TableState state)
        {
            States[pos] = state;
        }

        public TableState GetSquare(int row, int column)
        {
            return States[(row * 3) + column];
        }

        public List<Table> PossibleMoves(TableState state)
        {
            List<Table> possibleMoves = new List<Table>();
            for (int i = 0; i < CantSquares; ++i)
                if (States[i] == TableState.Neutral)
                {
                    States[i] = state;
                    possibleMoves.Add(new Table(this));
                    States[i] = TableState.Neutral;
                }
            return possibleMoves;
        }

        public bool GameFinished()
        {
            if (IsWinner(TableState.Cross) || IsWinner(TableState.Zero) 
                || !FindState(TableState.Neutral))
                return true;
            return false;
        }

        public bool IsWinner(TableState state)
        {
            for (int i = 0; i < 3; ++i)
            {
                if ((GetSquare(i, 0) == state && GetSquare(i, 1) == state && GetSquare(i, 2) == state) ||
                    (GetSquare(0, i) == state && GetSquare(1, i) == state && GetSquare(2, i) == state))
                    return true;
            }

            return (GetSquare(1, 1) == state &&
                ((GetSquare(0, 0) == state && GetSquare(2, 2) == state) ||
                (GetSquare(2, 0) == state && GetSquare(0, 2) == state)));
        }

        public bool FindState(TableState state)
        {
            for (int i = 0; i < CantSquares; ++i)
                if (States[i] == state)
                    return true;
            return false;
        }

        public TableSquare Difference(Table table)
        {
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    if (States[(i * 3) + j] != table.States[(i * 3) + j])
                        return new TableSquare(i, j);

            return new TableSquare(0, 0);
        }

        public override bool Equals(object obj)
        {
            if (obj is Table)
            {
                Table table = (obj as Table);

                for (int i = 0; i < CantSquares; ++i)
                    if (States[i] != table.States[i])
                        return false;
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hashCode = (int)States[0];
            for (int i = 1; i < CantSquares; ++i)
                hashCode = (hashCode * 10) + (int)States[i];

            return hashCode;
        }

        public TableState[] States { get; private set; }
        public int CantSquares => 9;

    }

    public class TableSquare
    {
        private int _row;
        private int _column;
        public TableSquare(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public int SquareNumber => (_row * 3) + _column;
    }

    public enum TableState
    { 
        Cross, Zero, Neutral
    }
}
