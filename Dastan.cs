using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dastan
{
    class Dastan
    {
        //list of squares
        protected List<Square> Board;//only declare
        protected int NoOfRows, NoOfColumns, MoveOptionOfferPosition;
        //list of players(2 players)
        protected List<Player> Players = new List<Player>();
        //list of string for move option offer
        protected List<string> MoveOptionOffer = new List<string>();
        //represents current player
        protected Player CurrentPlayer;
        //generate random numbers
        protected Random RGen = new Random();

        //parameterized constructor
        public Dastan(int R, int C, int NoOfPieces)
        {
            CreateCustomPlayers();
            CreateMoveOptions();
            NoOfRows = R;
            NoOfColumns = C;
            MoveOptionOfferPosition = 0;
            CreateMoveOptionOffer();
            CreateBoard();
            CreatePieces(NoOfPieces);
            CurrentPlayer = Players[0];
        }
        private void CalculateSahmMove(int StartSquareReference)
        {

        }
        private bool GetValidInt(string input)
        {
            if (Regex.IsMatch(input, @"^\d+$") && CheckSquareInBounds(Convert.ToInt32(input)))
                return true;
            if (Regex.IsMatch(input, @"^\d+$") && Convert.ToInt32(input) <= MoveOptionOffer.Count)
                return true;
            if (Regex.IsMatch(input, @"^\d+$"))
                return true;
            Console.WriteLine("Invalid Input");
            return false;
        }
        private bool AwardWafr()
        {
            int perc = 25;

            int randomValue = RGen.Next(1, 101);//20
            if (randomValue <= perc)
            {
                return true;
            }
            return false;
        }
        private void CreateCustomPlayers()
        {
            string player1, player2;
            Console.WriteLine("Enter name of player One: ");
            player1 = Console.ReadLine();
            do
            {
                Console.WriteLine("Enter name of player Two: ");
                player2 = Console.ReadLine();
            }
            while (player1 == player2);
            Players.Add(new Player("Player One: " + player1, 1));
            Players.Add(new Player("Player Two: " + player2, -1));
        }
        private void DisplayBoard()
        {
            Console.Write(Environment.NewLine + "   ");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write(Column.ToString() + "  ");
            }
            Console.Write(Environment.NewLine + "  ");
            for (int Count = 1; Count <= NoOfColumns; Count++)
            {
                Console.Write("---");
            }
            Console.WriteLine("-");
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                Console.Write(Row.ToString() + " ");
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    //1*10+1=11
                    int Index = GetIndexOfSquare(Row * 10 + Column);
                    Console.Write("|" + Board[Index].GetSymbol());
                    Piece PieceInSquare = Board[Index].GetPieceInSquare();
                    if (PieceInSquare == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(PieceInSquare.GetSymbol());
                    }
                }
                Console.WriteLine("|");
            }
            Console.Write("  -");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write("---");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void DisplayState()
        {
            DisplayBoard();
            if(CurrentPlayer.GetSahmStatus()==false)
                Console.WriteLine("Move option offer: " + MoveOptionOffer[0]);
            else
                Console.WriteLine("Move option offer: " + MoveOptionOffer[MoveOptionOfferPosition]);
            Console.WriteLine();
            Console.WriteLine(CurrentPlayer.GetPlayerStateAsString());
            Console.WriteLine("Turn: " + CurrentPlayer.GetName());
            Console.WriteLine();
        }


        private int GetIndexOfSquare(int SquareReference)
        {
            //22
            //13
            int Row = SquareReference / 10;//22/10=2  13/10=1  11/10=1
            int Col = SquareReference % 10;//22%10=2  13%10=3  11%10=1
            //BODMAS
            //1*6+1=7
            //0*6+2

            //
            return (Row - 1) * NoOfColumns + (Col - 1);
        }

        private bool CheckSquareInBounds(int SquareReference)
        {
            int Row = SquareReference / 10;//22/10=2
            int Col = SquareReference % 10;//22%10=2
            if (Row < 1 || Row > NoOfRows)
            {
                return false;
            }
            else if (Col < 1 || Col > NoOfColumns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckSquareIsValid(int SquareReference, bool StartSquare)
        {
            if (!CheckSquareInBounds(SquareReference))
            {
                return false;
            }
            Piece PieceInSquare = Board[GetIndexOfSquare(SquareReference)].GetPieceInSquare();
            if (PieceInSquare == null)
            {
                if (StartSquare)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()))
            {
                if (StartSquare)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (StartSquare)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool CheckIfGameOver()
        {
            bool Player1HasMirza = false;
            bool Player2HasMirza = false;
            foreach (var S in Board)
            {
                Piece PieceInSquare = S.GetPieceInSquare();
                if (PieceInSquare != null)
                {
                    if (S.ContainsKotla() && PieceInSquare.GetTypeOfPiece() == "mirza" && !PieceInSquare.GetBelongsTo().SameAs(S.GetBelongsTo()))
                    {
                        return true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[0]))
                    {
                        Player1HasMirza = true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[1]))
                    {
                        Player2HasMirza = true;
                    }
                }
            }
            return !(Player1HasMirza && Player2HasMirza);
        }

        private int GetSquareReference(string Description)
        {
            string SelectedSquare;
            do
            {
                Console.Write("Enter the square " + Description + " (row number followed by column number): ");
                SelectedSquare = Console.ReadLine();
            } while (!GetValidInt(SelectedSquare));

            return Convert.ToInt32(SelectedSquare);
        }

        private void UseMoveOptionOffer()
        {
            if (CurrentPlayer.GetChoiceOptionsLeft() < 1)
            {
                return;
            }
            string ReplaceChoice;
            do
            {
                Console.Write("Choose the move option from your queue to replace (1 to 7): ");
                ReplaceChoice = Console.ReadLine();
            } while (!GetValidInt(ReplaceChoice));
            CurrentPlayer.DecreaseChoiceOptionsLeft();
            if(CurrentPlayer.GetSahmStatus()==false)
                CurrentPlayer.UpdateMoveOptionQueueWithOffer(Convert.ToInt32(ReplaceChoice) - 1, CreateMoveOption(MoveOptionOffer[0], CurrentPlayer.GetDirection()));
            else
                CurrentPlayer.UpdateMoveOptionQueueWithOffer(Convert.ToInt32(ReplaceChoice) - 1, CreateMoveOption(MoveOptionOffer[MoveOptionOfferPosition], CurrentPlayer.GetDirection()));
            CurrentPlayer.ChangeScore(-(10 - (Convert.ToInt32(ReplaceChoice) * 2)));
            MoveOptionOfferPosition = RGen.Next(0, 7);
        }

        private int GetPointsForOccupancyByPlayer(Player CurrentPlayer)
        {
            int ScoreAdjustment = 0;
            foreach (var S in Board)
            {
                ScoreAdjustment += (S.GetPointsForOccupancy(CurrentPlayer));
            }
            return ScoreAdjustment;
        }

        private void UpdatePlayerScore(int PointsForPieceCapture)
        {
            CurrentPlayer.ChangeScore(GetPointsForOccupancyByPlayer(CurrentPlayer) + PointsForPieceCapture);
        }

        private int CalculatePieceCapturePoints(int FinishSquareReference)
        {
            if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
            {
                return Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetPointsIfCaptured();
            }
            return 0;
        }

        public void PlayGame()
        {
            bool GameOver = false;
            int oldScore = 0;
            bool legalMoveflag = false;
            int undoChoice = 0;
            int Choice = 0;
            int StartSquareReference = 0;
            int FinishSquareReference = 0;
            while (!GameOver)
            {
                if (legalMoveflag)
                {
                    DisplayBoard();
                    Console.WriteLine("Do you want to Undo? Press 1 for Yes 2 for No: ");
                    undoChoice = Convert.ToInt32(Console.ReadLine());
                }
                if (undoChoice == 1)
                {
                    if (CurrentPlayer.SameAs(Players[0]))
                    {
                        CurrentPlayer = Players[1];//p2
                    }
                    else
                    {
                        CurrentPlayer = Players[0];//p1
                    }
                    CurrentPlayer.ResetQueueBackAfterUndo(Choice - 1);
                    //104-100=4
                    CurrentPlayer.ChangeScore(-((CurrentPlayer.GetScore() - oldScore) + 5));
                    UpdateBoard(FinishSquareReference, StartSquareReference);
                    DisplayState();
                }
                else
                    DisplayState();
                legalMoveflag = false;
                bool SquareIsValid = false;

                do
                {
                    string userChoice;
                    do
                    {

                        Console.WriteLine("Choice Option Left: " + CurrentPlayer.GetChoiceOptionsLeft());
                        if (CurrentPlayer.GetChoiceOptionsLeft() == 0)
                            Console.Write("Choose move option to use from queue (1 to 3) or 8 to spy over oppenent's queue: ");
                        else
                            Console.Write("Choose move option to use from queue (1 to 3) or 9 to take the offer or 8 to spy over oppenent's queue: ");
                        userChoice = Console.ReadLine();
                    } while (!GetValidInt(userChoice));
                    Choice = Convert.ToInt32(userChoice);
                    if (Choice == 9)
                    {
                        UseMoveOptionOffer();
                        DisplayState();
                    }
                    else if (Choice == 8)
                    {
                        if (CurrentPlayer.SameAs(Players[0]))
                        {
                            CurrentPlayer = Players[1];//p2
                        }
                        else
                        {
                            CurrentPlayer = Players[0];//p1
                        }

                        Console.WriteLine(CurrentPlayer.GetJustQueueAsString());
                        if (CurrentPlayer.SameAs(Players[0]))
                        {
                            CurrentPlayer = Players[1];//p2
                        }
                        else
                        {
                            CurrentPlayer = Players[0];//p1
                        }
                        CurrentPlayer.ChangeScore(-5);
                        DisplayState();
                    }
                }
                while (Choice < 1 || Choice > 3);
                if (CurrentPlayer.ChoiceIsSahm(Choice - 1))
                {
                    while (!SquareIsValid)
                    {
                        StartSquareReference = GetSquareReference("containing the piece to move");
                        SquareIsValid = CheckSquareIsValid(StartSquareReference, true);
                    }
                    CalculateSahmMove(StartSquareReference);
                }
                else
                {
                    while (!SquareIsValid)
                    {
                        StartSquareReference = GetSquareReference("containing the piece to move");
                        SquareIsValid = CheckSquareIsValid(StartSquareReference, true);
                    }

                    SquareIsValid = false;
                    while (!SquareIsValid)
                    {
                        FinishSquareReference = GetSquareReference("to move to");
                        SquareIsValid = CheckSquareIsValid(FinishSquareReference, false);
                    }
                }
                bool MoveLegal = CurrentPlayer.CheckPlayerMove(Choice, StartSquareReference, FinishSquareReference);
                if (MoveLegal)
                {
                    legalMoveflag = true;
                    int PointsForPieceCapture = CalculatePieceCapturePoints(FinishSquareReference);
                    //temporary store
                    oldScore = CurrentPlayer.GetScore();//100
                    CurrentPlayer.ChangeScore(-(Choice + (2 * (Choice - 1))));
                    CurrentPlayer.UpdateQueueAfterMove(Choice);
                    UpdateBoard(StartSquareReference, FinishSquareReference);
                    UpdatePlayerScore(PointsForPieceCapture);
                    Console.WriteLine("New score: " + CurrentPlayer.GetScore() + Environment.NewLine);
                }
                if (CurrentPlayer.SameAs(Players[0]))
                {
                    CurrentPlayer = Players[1];//p2
                }
                else
                {
                    CurrentPlayer = Players[0];//p1
                }
                GameOver = CheckIfGameOver();
            }
            DisplayState();
            DisplayFinalResult();
        }

        private void UpdateBoard(int StartSquareReference, int FinishSquareReference)
        {
            Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(Board[GetIndexOfSquare(StartSquareReference)].RemovePiece());
        }

        private void DisplayFinalResult()
        {
            if (Players[0].GetScore() == Players[1].GetScore())
            {
                Console.WriteLine("Draw!");
            }
            else if (Players[0].GetScore() > Players[1].GetScore())
            {
                Console.WriteLine(Players[0].GetName() + " is the winner!");
            }
            else
            {
                Console.WriteLine(Players[1].GetName() + " is the winner!");
            }
        }

        private void CreateBoard()
        {
            Square S;//only declare
            Board = new List<Square>();
            //1-6
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    //Row=1
                    //col=6/2=3
                    if (Row == 1 && Column == NoOfColumns / 2)
                    {
                        S = new Kotla(Players[0], "K");
                    }
                    //row=6
                    //col=6/2+1=3+1=4
                    else if (Row == NoOfRows && Column == NoOfColumns / 2 + 1)
                    {
                        S = new Kotla(Players[1], "k");
                    }
                    else
                    {
                        //empty square
                        S = new Square();
                    }
                    Board.Add(S);
                }
            }
        }

        private void CreatePieces(int NoOfPieces)
        {
            Piece CurrentPiece;
            //1 to 4
            for (int Count = 1; Count <= NoOfPieces; Count++)
            {
                CurrentPiece = new Piece("piece", Players[0], 1, "!");
                //2*10=20
                //20+1+1=22
                Board[GetIndexOfSquare(2 * 10 + Count + 1)].SetPiece(CurrentPiece);
            }
            CurrentPiece = new Piece("mirza", Players[0], 5, "1");
            //10+3=13
            //13
            Board[GetIndexOfSquare(10 + NoOfColumns / 2)].SetPiece(CurrentPiece);
            for (int Count = 1; Count <= NoOfPieces; Count++)
            {
                CurrentPiece = new Piece("piece", Players[1], 1, "\"");
                Board[GetIndexOfSquare((NoOfRows - 1) * 10 + Count + 1)].SetPiece(CurrentPiece);
            }
            CurrentPiece = new Piece("mirza", Players[1], 5, "2");
            Board[GetIndexOfSquare(NoOfRows * 10 + (NoOfColumns / 2 + 1))].SetPiece(CurrentPiece);
        }

        private void CreateMoveOptionOffer()
        {
            MoveOptionOffer.Add("sahm");
            MoveOptionOffer.Add("jazair");
            MoveOptionOffer.Add("chowkidar");
            MoveOptionOffer.Add("cuirassier");
            MoveOptionOffer.Add("ryott");
            MoveOptionOffer.Add("faujdar");
            MoveOptionOffer.Add("faris");
            MoveOptionOffer.Add("sarukh");
        }

        private MoveOption CreateRyottMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("ryott");
            Move NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateFaujdarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("faujdar");
            Move NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateJazairMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("jazair");
            Move NewMove = new Move(2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateCuirassierMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("cuirassier");
            Move NewMove = new Move(1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateChowkidarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("chowkidar");
            Move NewMove = new Move(1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }
        private MoveOption CreateFarisMoveOption(int Direction)
        {
            //x=[2,1,-1,-2,-2,-1,1, 2]
            //y=[1,2, 2, 1,-1,-2,-2,-1]
            MoveOption NewMoveOption = new MoveOption("faris");
            Move NewMove = new Move(2 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-2 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-2 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }
        private MoveOption CreateSarukhMoveOption(int Direction)
        {
            //y =[0, 1, 0, -1, -2]
            //x =[1, -1, -1, -1, 0]
            MoveOption NewMoveOption = new MoveOption("sarukh");
            Move NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }
        private MoveOption CreateSahmMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("sahm");
            Move NewMove = new Move(0, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        //dastan class fucntion to create 5 move options for players

        private MoveOption CreateMoveOption(string Name, int Direction)
        {
            if (Name == "chowkidar")
            {
                return CreateChowkidarMoveOption(Direction);
            }
            else if (Name == "ryott")
            {
                return CreateRyottMoveOption(Direction);
            }
            else if (Name == "faujdar")
            {
                return CreateFaujdarMoveOption(Direction);
            }
            else if (Name == "jazair")
            {
                return CreateJazairMoveOption(Direction);
            }
            else if (Name == "faris")
            {
                return CreateFarisMoveOption(Direction);
            }
            else if (Name == "sarukh")
            {
                return CreateSarukhMoveOption(Direction);
            }
            else if (Name == "sahm")
            {
                return CreateSahmMoveOption(Direction);
            }
            else
            {
                return CreateCuirassierMoveOption(Direction);
            }
        }

        private void CreateMoveOptions()
        {
            Players[0].AddToMoveOptionQueue(CreateMoveOption("ryott", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("sarukh", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faris", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("chowkidar", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("cuirassier", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faujdar", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("jazair", 1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("ryott", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("sarukh", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faris", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("chowkidar", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("jazair", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faujdar", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("cuirassier", -1));
        }
    }
}
