using Dastan;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dastan
{
    class MoveOption
    {
        //name of the move
        protected string Name;
        protected List<Move> PossibleMoves;

        public MoveOption(string N)
        {
            Name = N;
            PossibleMoves = new List<Move>();
        }

        public void AddToPossibleMoves(Move M)
        {
            PossibleMoves.Add(M);
        }

        public string GetName()
        {
            return Name;
        }

        public bool CheckIfThereIsAMoveToSquare(int StartSquareReference, int FinishSquareReference)
        {
            int StartRow = StartSquareReference / 10;//22/10=2
            int StartColumn = StartSquareReference % 10;//22%10=2
            int FinishRow = FinishSquareReference / 10;//21/10=2
            int FinishColumn = FinishSquareReference % 10;//21%10=1
            foreach (var M in PossibleMoves)
            {
                //2+1==2 && 2+0==1
                //2+(-1)==2 && 2+0==1
                //2+0==2 && 2-1==1

                //2+1==2 && 2+0=1
                if (StartRow + M.GetRowChange() == FinishRow && StartColumn + M.GetColumnChange() == FinishColumn)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
