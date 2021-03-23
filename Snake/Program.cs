using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using static System.Console;
using System.Data;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            CursorVisible = false;
            MainMenu mm = new MainMenu();
            mm.MenuService();           

            WindowHeight = 40;
            WindowWidth = 40;

            int screenWidth = WindowWidth;
            int screenHeight = WindowHeight;                 
            
        }       
    }
    class SnakesCreator
    {
        public List<int> yChords { get; set; }
        public List<int> xChords { get; set; }

        public List<int> tempYChords { get; set; }
        public List<int> tempXChords { get; set; }

        Game Sc = new Game();
        public int x { get; set; }
        public int y { get; set; }
        public bool isVertical { get; set; }
        public int p;
        public Movement mv { get; set; }

        public void SnakeCreator()
        {
            y = 1;   
            Clear();
            for (int i = 0; i < yChords.Count; i++)
            {
                SetCursorPosition(xChords[i]+=x, yChords[i]+=y);
                Write("*");                                
            }
            Sc.CreateFood(xChords,yChords);
            SetCursorPosition(0,0);
            Sc.points = 0;
            Write("Score: "+Sc.points);           
            SetCursorPosition(0, 1);
            WriteLine("----------------------------------------");
        }
        public void SnakeMove()
        {
            //int p = 0;
            do
            {
                SetCursorPosition(Sc.xF, Sc.yF);
                Write("*");
                int c = yChords.Count - 1;
                int z = 0;
                SetCursorPosition(xChords[c], yChords[c]);
                Write(" ");

                List<int> tempYChords = new List<int>(yChords);
                List<int> tempXChords = new List<int>(xChords);
                for (int i = 0; i < c; i++)
                {
                    z ++;
                    xChords[z] = tempXChords[i];
                    yChords[z] = tempYChords[i];

                    if (i == 0)
                    {
                        xChords[i] += x;
                        yChords[i] += y;
                    }
                    else if(yChords[0]==1)
                    {
                        Sc.Loose(p);
                    }

                }
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                try
                {
                    if (xChords[0] >= 40 || yChords[0] >= 40)
                        Sc.Loose(p);
                    EatYourself();
                    SetCursorPosition(xChords[0], yChords[0]);
                    Write("*");
                }
                catch(ArgumentOutOfRangeException)
                {
                    Game s = new Game();
                    s.result = false;
                    s.Loose(p);
                }
                z =0;

                if (Sc.xF == xChords[0] + x && Sc.yF == yChords[0] + y)
                {
                    Sc.CreateFood(xChords, yChords);
                    Sc.points += 10;
                    p = Sc.points;
                    SetCursorPosition(0, 0);
                    WriteLine("Score: " + Sc.points);
                    xChords.Add(xChords[c] - x);
                    yChords.Add(yChords[c] - y);
                }              
            } while (KeyAvailable == false);
        }  
        public void EatYourself()
        {
            bool colider = false;
            int colideIndex = 0;
            List<int> tempYChords = new List<int>(yChords);
            List<int> tempXChords = new List<int>(xChords);
            for (int i = 1; i<tempXChords.Count; i++)
            {
                if (tempXChords[0] == tempXChords[i] && tempYChords[0] == tempYChords[i])
                {
                    colider = true;
                    colideIndex = i;
                }
                
                if(colider == true)
                {
                    SetCursorPosition(tempXChords[i],tempYChords[i]);
                    Write(" ");                   

                    if (colideIndex<xChords.Count)
                    {
                        xChords.RemoveAt(colideIndex);
                        yChords.RemoveAt(colideIndex);
                        Sc.points -= 10;
                        p = Sc.points;
                        SetCursorPosition(0, 0);                        
                        WriteLine("Score: " + Sc.points+"  ");

                    }                   
                }       
            }
        }
    }
    class Movement
    {
        SnakesCreator snakePix;
        public ConsoleKeyInfo currentKey { get; set; }
        public ConsoleKeyInfo previousKey { get; set; }
        public bool correctDirection { get; set; }
        public void MovementService(int d)
        {
            int xChange = 0;
            int yChange = 0;
            switch(d)
            {
                case 0:
                    break;
                case 1:
                    xChange = 0;
                    yChange = -1;
                    MoveUD(xChange,yChange);
                    break;
                case 2:
                    xChange = 0;
                    yChange = 1;
                    MoveUD(xChange,yChange);                    
                    break;
                case 3:
                    xChange = -1;
                    yChange = 0;
                    Move(xChange, yChange);
                    break;
                case 4:
                    xChange = 1;
                    yChange = 0;
                    Move(xChange, yChange);
                    break;
            }
        }
        public void MovementDirection(ConsoleKeyInfo cki, SnakesCreator snake, Movement m )
        {
            int d = 0;
            snakePix = snake;

            if (cki.Key == ConsoleKey.W)
                d = 1;
            else if (cki.Key == ConsoleKey.S)
                d = 2;
            else if (cki.Key == ConsoleKey.A)
                d = 3;
            else if (cki.Key == ConsoleKey.D)
                d = 4;

            MovementService(d);
        }
        public void Move(int xCh,int yCh)
        {
            snakePix.x = xCh;
            xCh = 0;
            snakePix.y = yCh;
            yCh = 0;  
            snakePix.SnakeMove();
        }
        public void MoveUD(int xCh, int yCh)
        {
            snakePix.x = xCh;
            xCh = 0;
            snakePix.y = yCh;
            yCh = 0;   
            snakePix.SnakeMove();
        }
        public bool CheckDirection(ConsoleKeyInfo ck)
        {
            if (previousKey.Key == ConsoleKey.W && ck.Key != ConsoleKey.S)
                return correctDirection = true;
            else if (previousKey.Key == ConsoleKey.S && ck.Key != ConsoleKey.W)
                return correctDirection = true;
            else if (previousKey.Key == ConsoleKey.A && ck.Key != ConsoleKey.D)
                return correctDirection = true;
            else if (previousKey.Key == ConsoleKey.D && ck.Key != ConsoleKey.A)
                return correctDirection = true;
            else
                return false;
        }
    }
    class Game
    {
        public bool result { get; set; }
        Random r = new Random();
        public int xF { get; set; }
        public int yF { get; set; }
        public int points { get; set; }
        public void Loose(int p)
        {
            points = p;
            Clear();
            string name;
            int tempScore = 0;
            int id = 0;
            WriteLine("Type your name: ");
            name = ReadLine();
            SetCursorPosition(5, 15);
            Write("You loose, your score: "+ points +" use any key to open Main Menu");
            string query = "Select Score,Id from Results order by 1 asc";
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sebastian\Desktop\Programy\Snake\Snake\bin\ScoreDB.mdf;Integrated Security=True;Connect Timeout=30");
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader sdr;
            sdr = command.ExecuteReader();
            while(sdr.Read())
            {
                id = Convert.ToInt32(sdr["Id"]);
                tempScore = Convert.ToInt32(sdr["Score"]);
                if (points > tempScore)
                {
                    string queryDelete = "Delete from Results where Id='"+id+"'";
                    string queryInsert = "Insert into Results values('"+name+"','"+points+"')";
                    sdr.Close();
                    SqlCommand commandDelete = new SqlCommand(queryDelete,connection);
                    commandDelete.ExecuteNonQuery();
                    SqlCommand commandInsert = new SqlCommand(queryInsert, connection);
                    commandInsert.ExecuteNonQuery();
                    connection.Close();
                    break;
                }
                
            }
            connection.Close();
            ReadKey();
            MainMenu mm = new MainMenu();
            mm.MenuService();
        }
        public void CreateFood(List<int> xChords, List<int> yChords)
        {
            bool repeat = false;
            
            do
            {
                repeat = false;

                xF = r.Next(0, 30);
                yF = r.Next(2, 30);

                if (xF < 0 || yF <= 1)
                    repeat = true;

                for (int i=0;i<xChords.Count;i++)
                {
                    if (xChords[i] == xF && yChords[i] == yF)
                        repeat = true;
                }

            } while (repeat);
            repeat = false;
            SetCursorPosition(xF, yF);
            Write("*");           
        }
        public void GameControl()
        {
            List<int> yChords = new List<int>() { 15, 16, 17, 18, 19 };
            List<int> xChords = new List<int>() { 15, 15, 15, 15, 15 };

            SnakesCreator snake = new SnakesCreator();

            snake.yChords = yChords;
            snake.xChords = xChords;
            snake.isVertical = true;

            Movement mv = new Movement();

            snake.mv = mv;

            mv.previousKey = new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false);

            ConsoleKeyInfo cki = new ConsoleKeyInfo(' ', ConsoleKey.DownArrow, false, false, false);

            snake.SnakeCreator();

            Game sc = new Game();
            sc.result = true;

            do
            {

                do
                {
                    cki = ReadKey(true);
                } while ((cki.Key != ConsoleKey.W && cki.Key != ConsoleKey.S && cki.Key != ConsoleKey.A && cki.Key != ConsoleKey.D));

                if (mv.previousKey.Key == ConsoleKey.DownArrow)
                    mv.previousKey = cki;
                if (mv.CheckDirection(cki) == true)
                {
                    mv.MovementDirection(cki, snake, mv);
                    mv.previousKey = cki;
                }
                else
                {
                    mv.MovementDirection(mv.previousKey, snake, mv);
                }
            } while (sc.result);
            //sc.Loose();
        }
    }
    class MainMenu
    {
        bool needRefresh;
        bool isActiveMenu;
        int whichOption;
        int x;
        int y;
        public void MenuService()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo(' ', ConsoleKey.A, false, false, false);
            List<int> pointer = new List<int> { 17, 18, 19, 20 };
            isActiveMenu = true;
            needRefresh = true;
            do
            {
                if (needRefresh == true)
                {
                    Clear();
                    whichOption = 0;                   
                    x = 17;
                    y = 5;                    
                    MenuCreator();                   
                    SetCursorPosition(x, y);
                    Write("--->");
                    needRefresh = false;
                }

                cki = ReadKey();

                if (cki.Key == ConsoleKey.DownArrow || cki.Key == ConsoleKey.UpArrow)
                    MovePointer(pointer, cki);

                ClearBuff();
                if (cki.Key == ConsoleKey.Enter)
                {
                    LaunchOption();
                }
                

            } while (isActiveMenu);

            ReadKey();
        }
        public void MenuCreator()
        {
            int Height = 30;
            int Width = 50;
            SetWindowSize(Width, Height);
            SetBufferSize(Width, Height);
            CreateFrame();
            SetCursorPosition(22, 5);
            WriteLine("New game");
            SetCursorPosition(22, 7);
            WriteLine("Records");
            SetCursorPosition(22, 9);
            WriteLine("Manual");
            SetCursorPosition(22, 11);
            WriteLine("Exit");

        }
        public void CreateFrame()
        {
            for(int i = 0; i<48; i+=2)
            {
                SetCursorPosition(2+i, 3);
                Write("*");
                SetCursorPosition(2+i, 13);
                Write("*");
            }
            for (int i = 4; i < 13; i++)
            {
                SetCursorPosition(2, i);
                WriteLine("*");
                SetCursorPosition(48, i);
                WriteLine("*");
            }
        }
        public void MovePointer(List<int> p, ConsoleKeyInfo cki)
        {
            if (cki.Key == ConsoleKey.UpArrow)
            {
                if (whichOption == 0)
                    whichOption = 3;
                else if (whichOption >= 0)
                    whichOption -= 1;
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (whichOption == 3)
                    whichOption = 0;
                else if (whichOption >= 0)
                    whichOption += 1;
            }

            SetCursorPosition(x, y);
            WriteLine("    ");

            switch (whichOption)
            {
                case 0:
                    x = 17;
                    y = 5;
                    break;
                case 1:
                    x = 17;
                    y = 7;
                    break;
                case 2:
                    x = 17;
                    y = 9;
                    break;
                case 3:
                    x = 17;
                    y = 11;
                    break;
            }
            
            SetCursorPosition(x, y);
            for (int i = 0; i != p.Count; i++)
            {                
                if (i == p.Count-1)
                    Write(">");
                else
                    Write("-");
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }
            
        }
        public void LaunchOption()
        {
            switch(whichOption)
            {
                case 0:
                    NewGame();
                    break;
                case 1:
                    Records();
                    break;
                case 2:
                    Manual();
                    break;
                case 3:
                    Exit();
                    break;
            }
        }
        public void ClearBuff()
        {
            SetCursorPosition(0, 0);
            while (KeyAvailable)
                ReadKey(false);
        }
        public void GoMainMenu()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            do
            {
                ClearBuff();
                cki = ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    needRefresh = true;
            } while (cki.Key != ConsoleKey.Escape);
        }
        public void NewGame()
        {
            Game g = new Game();
            g.GameControl();
            needRefresh = true;
        }
        public void Records()
        {
            Clear();
            int i = 3;
            string Name;
            string Score ="null";
            string query = "Select Top 10 Score,Name from Results order by Score desc";            
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sebastian\Desktop\Programy\Snake\Snake\bin\ScoreDB.mdf;Integrated Security=True;Connect Timeout=30");
            connection.Open();
            SqlCommand command = new SqlCommand(query,connection);
            SqlDataReader sdr;
            command.Parameters.Clear();
            sdr = command.ExecuteReader();
            SetCursorPosition(20,0);
            Write("Top 10 Scores");
            while (sdr.Read())
            {  
                Name = sdr["Name"].ToString();
                Score = sdr["Score"].ToString();
                SetCursorPosition(1, i);
                Write(i-2+"."+Name);
                SetCursorPosition(14,i);
                Write(Score);
                i++;
            }
            i = 0;
            SetCursorPosition(8, 29);
            WriteLine("Press ESC to leave and pass to Menu.");
            GoMainMenu();
        }
        public void Manual()
        {
            Clear();
            CreateFrame();
            SetCursorPosition(22, 4);
            WriteLine("Manual");
            SetCursorPosition(4, 6);
            WriteLine("It is manual for game Snake. How to play?");
            SetCursorPosition(4, 7);
            WriteLine("Standard control in game: W,A,S,D- ");
            SetCursorPosition(4, 8);
            WriteLine("go forward, go left, go backwards, go right.");
            SetCursorPosition(4, 9);
            WriteLine("To control MainMenu use uparrow, downarrow ");
            SetCursorPosition(4, 10);
            WriteLine("and enter to choose option.");
            SetCursorPosition(4, 11);
            WriteLine("If you want to launch a pause just click 'P'");
            SetCursorPosition(4, 12);
            WriteLine("while game is on.");
            SetCursorPosition(8, 29);
            WriteLine("Press ESC to leave and pass to Menu.");
            GoMainMenu();
            
        }
        public void Exit()
        {
            Environment.Exit(0);
        }
    }    
}
