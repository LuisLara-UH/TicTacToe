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

        public void InitializeTables()
        {
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

        public void InitializeOptimalMoves()
        { 
            
        }

        private bool FindIAOptimalMove(Table table)
        {
            foreach (var move in table.PossibleMoves(TableState.Zero))
            {
                if (IsOptimalMove(move))
                { 
                    
                }
            }
            return false;
        }

        private bool IsOptimalMove(Table table)
        {
            foreach (var move in table.PossibleMoves(TableState.Cross))
            { 
            
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
