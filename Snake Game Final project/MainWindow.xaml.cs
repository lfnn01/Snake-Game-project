﻿using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake_Game_Final_project
{
    
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValueToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake, Images.Body},
            {GridValue.Food, Images.Food }
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0 },
            {Direction.Right, 90 },
            {Direction.Down, 180 },
            {Direction.Left, 270 }
        };

        private readonly int Rows = 15, Columns = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(Rows, Columns);
        }
        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(Rows, Columns);

        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {

                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                    case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break; 
                    case Key.Up: 
                    gameState.ChangeDirection(Direction.Up);
                    break; 
                    case Key.Down:
                    gameState.ChangeDirection(Direction.Down); 
                    break;
               
            }
        }

        private async Task GameLoop()
        {
            while (!gameState.GameOver) {
                await Task.Delay(100);
                gameState.Move();
                Draw();
        }
        }
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[Rows, Columns];
            GameGrid.Rows = Rows;
            GameGrid.Columns = Columns;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {

                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5),
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images; 
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text=$"SCORE: {gameState.Score}";
        }
        private void DrawGrid()
        {
            for (int r=0; r < Rows; r++)
            {
                for (int c=0; c < Columns; c++)
                {
                    GridValue gridValue = gameState.Grid[r,c];
                    gridImages[r, c].Source = gridValueToImage[gridValue];
                    

                }
            }
            
        }

        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Column];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<Position> position = new List<Position>(gameState.SnakePositions());

            for (int i = 0; i < position.Count; i++)
            {
                Position pos = position[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Column].Source = source;
                await Task.Delay(50);
            }
        }

           private async Task ShowCountDown()
                { 
            for (int i=3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);


            }
                

           }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(500);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "TRY AGAIN :( ";
        }

    }
}