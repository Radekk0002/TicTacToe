using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    public class Player
    {
        private char character;

        public Player(char character)
        {
            this.character = character;
        }

        public char Character { get { return character; } }
    }

    internal interface IAI
    {
        int Move(ArrayList fields);
    }

    public class EasyAI : Player, IAI
    {
        private char[] availableFields;

        public EasyAI(char character) : base(character)
        {
        }

        public int Move(ArrayList fields)
        {
            Random r = new Random();
            int rnd = r.Next(fields.Count);
            int field = (int)fields[rnd];
            fields.RemoveAt(rnd);

            return field;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player p;
        private EasyAI ai;
        private int moveCount;
        private bool ifplayerMove;
        private bool endGame;
        private char[] fields;
        private ArrayList availableFields;

        public MainWindow()
        {
            InitializeComponent();
            LoadGame();
        }

        private void LoadGame()
        {
            Random r = new Random();
            p = new Player('X');
            ai = new EasyAI('O');
            moveCount = 0;
            ifplayerMove = false;
            endGame = false;
            fields = new char[9];
            availableFields = new ArrayList();
            for (int i = 0; i < 9; i++) availableFields.Add(i);
            if (r.Next(2) == 1) ifplayerMove = true;
            else AI_Move();
        }

        private void CheckWinner()
        {
            //Vertically
            for (int i = 0; i < 7; i += 3)
            {
                char tmp = fields[i];
                if (tmp != default && tmp == fields[i + 1] && tmp == fields[i + 2])
                    EndGame(new int[] { i,i+1,i+2});
            }

            //Horizontally
            for (int i = 0; i < 3; i++)
            {
                char tmp = fields[i];
                if (tmp != default && tmp == fields[i + 3] && tmp == fields[i + 6])
                    EndGame(new int[] { i, i + 3, i + 6 });
            }

            //Diagonally
            for (int i = 2; i < 5; i += 2)
            {
                char tmp = fields[4];
                if (tmp != default && tmp == fields[4 - i] && tmp == fields[4 + i])
                    EndGame(new int[] { 4, 4-i, 4+i });
            }
        }

        private void EndGame(int[] t)
        {
            foreach (var field in t)
            {
                Button btn = (Button)this.FindName("b" + field);
                btn.Background = Brushes.Green;
            }
            endGame = true;
        }

        private void AI_Move()
        {
            int choosenField = ai.Move(availableFields);

            Button btn = (Button)this.FindName("b" + choosenField);

            btn.Content = fields[choosenField] = ai.Character;
            ifplayerMove = !ifplayerMove;
            moveCount++;
            if (moveCount > 4) CheckWinner();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bttn = sender as Button;
            int field = (int)((string)bttn.Name)[1] - '0';
            if (!ifplayerMove || fields[field] != default || moveCount > 9 || endGame)
                return;
            ifplayerMove = !ifplayerMove;
            availableFields.Remove(field);
            moveCount++;
            bttn.Content = fields[field] = p.Character;
            if (moveCount > 4) CheckWinner();
            if (!endGame && moveCount<9) AI_Move();
        }

        private void NewGame()
        {
            for (int i = 0; i < 9; i++)
            {
                Button btn = (Button)this.FindName("b" + i);
                btn.Content = String.Empty;
                btn.Background = Brushes.White;
            }
            LoadGame();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString() == "R") NewGame();
        }
    }
}