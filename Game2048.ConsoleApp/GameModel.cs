using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048.ConsoleApp
{
    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class GameModel
    {
        Random rnd = new Random();

        private int boardSize { get; set; }
        private int winScore { get; set; }
        private int playerScore { get; set; }
        private int[,] boardArray { get; set; }
        //public bool continuePlayAfterWin { get; set; }
        public bool isPlay { get; set; }

        public string currentShift { get; set; }
        public bool hasPosibleMovingOnCurrentShift { get; set; }
        public GameModel(int boardSize, int winScore = 2048 /*, bool continuePlayAfterWin = false*/)
        {
            this.boardSize = boardSize;
            this.winScore = winScore;
            //this.continuePlayAfterWin = continuePlayAfterWin;

            if (boardSize < 2)
            {
                throw new Exception("Invalid size");
            }
            isPlay = true;
            Play();
        }

        public void Play()
        {
            InitNewGame();
            while (isPlay)
            {
                Show();
                ConsoleKeyInfo key = Console.ReadKey();
                string basicKey = "WASD";

                // copy array value to temp
                // simple way like "temp = boardArray" doesn't work in some case
                int[,] temp = new int[boardSize, boardSize];
                Buffer.BlockCopy(boardArray, 0, temp, 0, temp.Length * sizeof(int));

                string keyString = key.KeyChar.ToString().ToUpper();
                switch (keyString)
                {
                    case "A":
                        boardArray = ShiftLeft();
                        break;

                    case "D":
                        boardArray = ShiftRight();
                        break;

                    case "W":
                        boardArray = ShiftUp();
                        break;

                    case "S":
                        boardArray = ShiftDown();
                        break;

                    default:
                        break;
                }
                hasPosibleMovingOnCurrentShift = temp.Cast<int>().SequenceEqual(boardArray.Cast<int>()) ? false : true;

                if (basicKey.Contains(keyString))
                {
                    // add new random number
                    Point newP = GetRandomPosition();
                    if(newP.x == -1 && newP.y == -1) 
                    {
                        // if still has change keep it run
                        // else it must be game over
                        if (!IsMovable())
                        {
                            isPlay = false;
                            Console.Clear();
                            Show();
                            Console.WriteLine("Game Over");
                        }
                    }
                    else if(hasPosibleMovingOnCurrentShift)
                    {
                        boardArray[newP.x, newP.y] = GetNewRandomNumber();
                    }
                }                

                if (keyString.Contains("X"))
                {
                    isPlay = false;
                    Console.WriteLine("Good bye\nPress any key to continue");
                }

                if (isPlay)
                {
                    Console.Clear();
                }

            }
        }

        public void Show()
        {
            Console.WriteLine("== Board Game : 2048 ==");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Direction : use w, a, s, d to shift blocks");
            Console.WriteLine(string.Format("Size : {0} x {0} \t\t Score : {1}", boardSize, playerScore));
            Console.WriteLine("Target : " + winScore.ToString());
            int maxno = boardArray.Cast<int>().Max();
            if (maxno >= winScore)
            {
                Console.WriteLine("You Win");
            }
            Console.WriteLine("-----------------------");
            Console.WriteLine("\n");
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                Console.WriteLine(string.Join("\t", boardArray.GetRow(i)));
                Console.WriteLine();
            }
        }

        public GameModel InitNewGame()
        {
            boardArray = new int[boardSize, boardSize];
            // random 2 number in random to coordinat
            GetNewRandomPositionFirstRun();
            return this;
        }


        public int GetNewRandomNumber() 
        {
            // get more 2 on random
            int[] newNumber = new int[] { 2, 2, 2, 2, 4 };
            int randomIndex = rnd.Next(newNumber.Length);
            return newNumber[randomIndex];
        }

        public Point GetRandomPosition()
        {
            int xpos = rnd.Next(boardSize);
            int ypos = rnd.Next(boardSize);

            var listOfZeroPos = GetZeroPosition();
            if (boardArray[xpos, ypos] != 0 && listOfZeroPos.Count() > 0)
            {
                Point rndPoint = listOfZeroPos[rnd.Next(listOfZeroPos.Count())];
                xpos = rndPoint.x;
                ypos = rndPoint.y;   
            }
            else if(listOfZeroPos.Count() == 0)
            {
                xpos = -1;
                ypos = -1;
            }
            return new Point(xpos, ypos);
        }

        public List<Point> GetZeroPosition()
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (boardArray[i, j] == 0)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            return points;
        }

        /// get random position on first run 
        public void GetNewRandomPositionFirstRun()
        {
            Point pos1 = GetRandomPosition();
            Point pos2 = GetRandomPosition();

            while (pos1.x == pos2.x && pos1.y == pos2.y)
            {
                pos2 = GetRandomPosition();
            }

            boardArray[pos1.x, pos1.y] = GetNewRandomNumber();
            boardArray[pos2.x, pos2.y] = GetNewRandomNumber();
        }

        public int[] ShiftArray(int[] arr)
        {
            // zero array to store the calculated value
            int[] newArray = new int[boardSize];
            // removing zero from array
            arr = arr.Where(m => m > 0).ToArray();
            // adding the same number into one cell
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] == arr[i + 1])
                {
                    arr[i] += arr[i + 1];
                    arr[i + 1] = 0;
                    playerScore += arr[i];
                }
            }
            // removing zero again
            arr = arr.Where(m => m > 0).ToArray();
            // copy the result to new array
            for (int i = 0; i < arr.Length; i++)
            {
                newArray[i] = arr[i];
            }
            return newArray;
        }

        // shift array per row
        public int[,] ShiftArray2D(int[,] arr2D)
        {
            int[,] temp = new int[arr2D.GetLength(0), arr2D.GetLength(1)];

            for (int i = 0; i < arr2D.GetLength(0); i++)
            {
                int[] rowArr = arr2D.GetRow(i);
                var shiftedRow = ShiftArray(rowArr);
                for (int j = 0; j < arr2D.GetLength(1); j++)
                {
                    arr2D[i, j] = shiftedRow[j];
                }
            }
            return arr2D;
        }

        public int[,] ReverseX(int[,] arr2D)
        {
            int[,] temp = new int[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int newX = i;
                    int newY = boardSize - j - 1;
                    temp[i, j] = arr2D[newX, newY];
                }
            }
            return temp;
        }

        public int[,] RotateNegative90Deg(int[,] arr2D)
        {
            int[,] temp = new int[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int newX = boardSize - j - 1;
                    int newY = i;
                    temp[i, j] = arr2D[newX, newY];
                }
            }
            return temp;
        }

        public int[,] RotatePositive90Deg(int[,] arr2D)
        {
            int[,] temp = new int[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int newX = j;
                    int newY = boardSize - i - 1;
                    temp[i, j] = arr2D[newX, newY];
                }
            }
            return temp;
        }

        // default shift direction: left, 
        // shift right => reverse x -> default shift -> reverse x again
        // shift up => rotate 90deg -> default shift -> rotate -90deg
        // shift down => rotate -90deg -> default shift -> rotate 90deg
        public int[,] ShiftLeft()
        {
            currentShift = "left";
            return ShiftArray2D(this.boardArray);
        }

        public int[,] ShiftRight()
        {
            currentShift = "right";
            var temp = boardArray;
            temp = ReverseX(temp);
            temp = ShiftArray2D(temp);
            temp = ReverseX(temp);
            return temp;
        }

        public int[,] ShiftUp()
        {
            currentShift = "up";
            var temp = boardArray;
            temp = RotatePositive90Deg(temp);
            temp = ShiftArray2D(temp);
            temp = RotateNegative90Deg(temp);
            return temp;
        }

        public int[,] ShiftDown()
        {
            currentShift = "down";
            var temp = boardArray;
            temp = RotateNegative90Deg(temp);
            temp = ShiftArray2D(temp);
            temp = RotatePositive90Deg(temp);
            return temp;
        }

        public bool IsMovable()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize - 1; j++)
                {
                    if (boardArray[i, j] == boardArray[i, j + 1])
                    {
                        return true;
                    }
                }
            }

            for (int i = 0; i < boardSize - 1; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (boardArray[i, j] == boardArray[i + 1, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public bool IsValidatedArray(int[] arr)
        {
            int arrSize = arr.Length;
            if (arrSize != this.boardSize)
            {
                throw new Exception("Different array size, please check it again");
            }

            if (arr.Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}
