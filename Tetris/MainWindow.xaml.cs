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

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            // the order of the list below matches the IDs
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls; // one image control for every cell in game grid

        private GameState gameState = new GameState();
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid) 
        {
            // this image control array will have 22 rows and 10 columns
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image 
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                

                    Canvas.SetTop(imageControl, (r - 2) * cellSize); //this is to push the top hidden rows up to not show up on the canvas
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls; // this is a two-dimensional array with one image for every cell in game grid
        }

        private void DrawGrid(GameGrid grid) // a for loop for each position
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c]; // this sets the starting source for id
                    imageControls[r, c].Source = tileImages[id]; // sets source of image at position using id
                }
            }
        }

        private void DrawBlock(Block block) // this loops through tile positions and update the image sources
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id]; 
            }
        }

        private void Draw(GameState gameState) // this calls draw method that draws both grid and current block
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
        }

        private async Task GameLoop() // async is needed, as the game has to wait without blocking UI
        {
            Draw(GameState);

            while (!gameState.GameOver)
            {
                await Task.Delay(500);
                gameState.MoveBlockDown();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // if the game has ended, then pressing any keys should not do anything
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                //these are the controls to move the tetris blocks
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                default:
                    return; // this ensure that the game redraws if the player does something when pressing a key
            }

            Draw(gameState);
        }
        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Draw(gameState);
        }

    }
}
