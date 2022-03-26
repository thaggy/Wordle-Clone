using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
//using System.Windows.Forms;


namespace wordle
{
    public partial class MainWindow : Window
    {
        int RowNum = 6;
        int ColumnNum = 5;
        int currentColumn;
        int currentRow;
        Rectangle[,] rectangleList;
        TextBlock[,] textList;

        String[] answerList; // 0 to 2314
        String[] guessList; //0 to 10656
        String answer;
        Random answerNum;
        HashSet<String> guessHash;
        public MainWindow()
        {
            InitializeComponent();
            //Adding KeyDown Event
            this.KeyDown += inputPress;
            //Initializing Values
            rectangleList = new Rectangle[RowNum, ColumnNum];
            textList = new TextBlock[RowNum, ColumnNum];
            answerList = File.ReadAllLines("answer.txt");
            guessList = File.ReadAllLines("guess.txt");
            answerNum = new Random();

            //For loop to create RectangleList
            for (int i = 0; i < RowNum; i++)
            {
                for (int j = 0; j < ColumnNum; j++)
                { 
                    Rectangle test = new Rectangle();
                    test.Stroke = new SolidColorBrush(Color.FromRgb(56, 56, 56));
                    test.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
                    test.Height = 77;
                    test.Width = 77;
                    test.StrokeThickness = 3;
                    test.VerticalAlignment = VerticalAlignment.Center;
                    test.HorizontalAlignment = HorizontalAlignment.Center;
                    test.Margin = new Thickness(10, 10, 10, 10);
                    mainGrid.Children.Add(test);
                    Grid.SetColumn(test, j);
                    Grid.SetRow(test, i);
                    rectangleList[i, j] = test;
                }
            }
            //For loop to create TextList
            for (int i = 0; i < RowNum; i++)
            {
                for (int j = 0; j < ColumnNum; j++)
                {
                    TextBlock temp = new TextBlock();
                    temp.HorizontalAlignment = HorizontalAlignment.Center;
                    temp.VerticalAlignment = VerticalAlignment.Center;
                    temp.Text = "";
                    temp.FontSize = 25;
                    temp.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
                    mainGrid.Children.Add(temp);
                    Grid.SetRow(temp, i);
                    Grid.SetColumn(temp,j);
                    textList[i, j] = temp;
                }
            }

            //Setting Up Game
            currentColumn = 0;
            currentRow = 0;
            answer = answerList[answerNum.Next(answerList.Length)];
            guessHash = guessList.ToHashSet<String>();
           // MessageBox.Show(answer);
        }

        //A Key Is Pressed
        public void inputPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            //Letter is Pressed, Move Forward Row
            if (e.Key <= Key.Z && e.Key >= Key.A)
            {
                LetterPress(e.Key.ToString());
            }
            //Enter, Move Row
            if (e.Key == Key.Enter)
            {
                onEnter();
            }
            //BackSpace, move back Column
            if (e.Key == Key.Back)
            {
                backSpace();
            }
            if (e.Key == Key.D1)
            {
                resetGame();
            }
            if (e.Key == Key.D2)
            {
                MessageBox.Show(answer);
            }
        }

        public void LetterPress(string letter)
        {
            textList[currentRow, currentColumn].Text = letter;
            if (currentColumn != 4)
            {
                currentColumn++;
            }
            else if (currentColumn > 4)
            {
                currentColumn = 4;
            }
        }
        public void backSpace()
        {
            if (currentColumn == 4)
            {
                if (textList[currentRow, currentColumn].Text == "")
                {
                    textList[currentRow, currentColumn-1].Text = "";
                    currentColumn--;
                }
                else
                {
                    textList[currentRow, currentColumn].Text = "";
                }
            }
            else if (currentColumn != 0)
            {
                textList[currentRow, currentColumn].Text = "";
                textList[currentRow, currentColumn-1].Text = "";
                currentColumn--;
            }
            else if (currentColumn == 0)
            {
                textList[currentRow, currentColumn].Text = "";
            }
        }

        public void onEnter()
        {
            if (currentRow != RowNum)
            {
                string guess = textList[currentRow, 0].Text.ToString() + textList[currentRow, 1].Text.ToString() + textList[currentRow, 2].Text.ToString() + textList[currentRow, 3].Text.ToString() + textList[currentRow, 4].Text.ToString();
                if (isLegal(guess))
                {
                    currentColumn = 0;
                    CheckWord(guess);
                    
                    if (currentRow+1 != RowNum)
                    {
                        currentRow++;
                    }
                }
                
            }
        }
        public bool isLegal(string guess)
        {
            if (guessHash.Contains(guess.ToLower()))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Non Legal Guess");
                return false;
            }
        }
        public void CheckWord(string guess)
        {
            char[] guessArray = guess.ToLower().ToCharArray();
            char[] answerArray = answer.ToCharArray();
            //Check if won
            if (guess.ToLower() == answer)
            {
                for (int i = 0; i < guessArray.Length; i++)
                {
                    rectangleList[currentRow,i].Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                }
                MessageBox.Show("You Won!");
            }
            else
            {
                for (int i = 0; i < guessArray.Length; i++)
                {
                    if (guessArray[i] == answerArray[i])
                    {
                        rectangleList[currentRow, i].Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                        answerArray[i] = '!';
                    }
                }
                for (int i = 0; i < guessArray.Length; i++)
                {
                    for (int j = 0; j < answerArray.Length; j++)
                    {
                        //Found common character
                        if (answerArray[j] == guessArray[i])
                        {
                            answerArray[j] = '!';
                            rectangleList[currentRow, i].Fill = new SolidColorBrush(Color.FromRgb(255, 200, 0));
                        }
                    }
                }
                
            }
        }
        public void resetGame()
        {
            currentColumn = 0;
            currentRow = 0;
            answer = answerList[answerNum.Next(answerList.Length)];

            for (int i = 0; i < RowNum; i++)
            {
                for (int j = 0; j < ColumnNum; j++)
                {
                    rectangleList[i, j].Fill = new SolidColorBrush(Color.FromArgb(100,0,0,0));
                    textList[i, j].Text = "";
                }
            }
        }
    }
}
