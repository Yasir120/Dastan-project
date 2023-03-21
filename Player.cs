using System;
using System.Collections.Generic;
using System.Text;

namespace Dastan
{
    class Player
    {
        //name of the player
        private string Name;
        private int Direction, Score;
        private MoveOptionQueue Queue = new MoveOptionQueue();
        private bool WafrAwarded;
        private int ChoiceOptionsLeft;
        private bool SahmUsed;
        //constructor
        public Player(string N, int D)
        {
            Score = 100;
            Name = N;
            Direction = D;
            ChoiceOptionsLeft = 3;
            SahmUsed = false;
        }
        public bool GetSahmStatus()
        {
            return SahmUsed;
        }
        public void SetSahmUsed(bool SahmUsed)
        {
            this.SahmUsed = SahmUsed;
        }
        public bool ChoiceIsSahm(int Position)
        {
           MoveOption move= Queue.GetMoveOptionInPosition(Position);
            return move.GetName() == "sahm";
        }
        public void ResetQueueBackAfterUndo(int Position)
        {
            Queue.ResetQueueBack(Position);
        }
        public void DecreaseChoiceOptionsLeft()
        {
            --this.ChoiceOptionsLeft;
        }
        public int GetChoiceOptionsLeft()
        {
            return this.ChoiceOptionsLeft;
        }
        public string GetJustQueueAsString()
        {
            return "Your Opponent's Queue: " + Queue.GetQueueAsString();
        }
        public void SetWafrAwarded(bool WafrAwarded)
        {
            this.WafrAwarded = WafrAwarded;
        }
        public bool GetWafrAwarded()
        {
            return this.WafrAwarded;
        }
        public bool SameAs(Player APlayer)
        {
            if (APlayer == null)
            {
                return false;
            }
            else if (APlayer.GetName() == Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPlayerStateAsString()
        {
            return Name + Environment.NewLine + "Score: " + Score.ToString() + Environment.NewLine + "Move option queue: " + Queue.GetQueueAsString() + Environment.NewLine;
        }

        public void AddToMoveOptionQueue(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);
        }

        public void UpdateQueueAfterMove(int Position)
        {
            Queue.MoveItemToBack(Position - 1);
        }

        public void UpdateMoveOptionQueueWithOffer(int Position, MoveOption NewMoveOption)
        {
            Queue.Replace(Position, NewMoveOption);
        }

        public int GetScore()
        {
            return Score;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetDirection()
        {
            return Direction;
        }

        public void ChangeScore(int Amount)
        {
            //100-8=92
            //104+100=204
            Score += Amount;
        }

        public bool CheckPlayerMove(int Pos, int StartSquareReference, int FinishSquareReference)
        {
            MoveOption Temp = Queue.GetMoveOptionInPosition(Pos - 1);
            return Temp.CheckIfThereIsAMoveToSquare(StartSquareReference, FinishSquareReference);
        }
    }
}
