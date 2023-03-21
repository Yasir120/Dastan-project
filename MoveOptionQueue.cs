using System;
using System.Collections.Generic;
using System.Text;

namespace Dastan
{
    class MoveOptionQueue
    {
        private List<MoveOption> Queue = new List<MoveOption>();

        public string GetQueueAsString()
        {
            string QueueAsString = "";
            int Count = 1;
            foreach (var M in Queue)
            {
                QueueAsString += Count.ToString() + ". " + M.GetName() + "   ";
                Count += 1;
            }
            return QueueAsString;
        }

        public void Add(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);
        }

        public void Replace(int Position, MoveOption NewMoveOption)
        {
            Queue[Position] = NewMoveOption;
        }

        public void MoveItemToBack(int Position)
        {
            MoveOption Temp = Queue[Position];
            Queue.RemoveAt(Position);
            Queue.Add(Temp);
        }

        public MoveOption GetMoveOptionInPosition(int Pos)
        {
            return Queue[Pos];
        }
        public void ResetQueueBack(int Position)
        {
            //10 20 30
            //count=3
            MoveOption temp = Queue[Queue.Count - 1];
            Queue[Queue.Count - 1] = Queue[Position];
            Queue[Position] = temp;
        }
    }
}
