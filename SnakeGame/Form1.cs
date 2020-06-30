using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> Snake = new List<Point>();
        private List<Point> Food = new List<Point>();
        private int maxXpos;
        private int maxYpos;

        public Form1()
        {
            InitializeComponent();
            new SnakeProperties();
            GameTimer.Interval = 1000 / SnakeGame.SnakeProperties.Speed;
            GameTimer.Tick += updateScreen;
            GameTimer.Start();
            maxXpos = pbCanvas.Size.Width / SnakeGame.SnakeProperties.Width;
            maxYpos = pbCanvas.Size.Height / SnakeGame.SnakeProperties.Height;
            startGame();
        }

        private void startGame()
        {
            label3.Visible = false;
            new SnakeProperties();
            Snake.Clear();
            Point head = new Point { X = 10, Y = 5 };
            Snake.Add(head);
            label2.Text = SnakeGame.SnakeProperties.Score.ToString();
            generateFood();
        }

        private void generateFood()
        {
            Random rnd = new Random();
            var numFoods = rnd.Next(1,4);

            for (int i = 0; i < numFoods; i++)
            {
                Food.Add(new Point(rnd.Next(0, maxXpos), rnd.Next(0, maxYpos)));
            }
        }

        private void updateScreen(object sender, EventArgs e)
        {
            if (SnakeGame.SnakeProperties.GameOver)
            {
                if (Input.LastPressed.Equals(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if (Input.LastPressed.Equals(Keys.Right) && SnakeGame.SnakeProperties.direction != Directions.Left)
                {
                    SnakeGame.SnakeProperties.direction = Directions.Right;
                }
                else if (Input.LastPressed.Equals(Keys.Left) && SnakeGame.SnakeProperties.direction != Directions.Right)
                {
                    SnakeGame.SnakeProperties.direction = Directions.Left;
                }
                else if (Input.LastPressed.Equals(Keys.Up) && SnakeGame.SnakeProperties.direction != Directions.Down)
                {
                    SnakeGame.SnakeProperties.direction = Directions.Up;
                }
                else if (Input.LastPressed.Equals(Keys.Down) && SnakeGame.SnakeProperties.direction != Directions.Up)
                {
                    SnakeGame.SnakeProperties.direction = Directions.Down;
                }
                movePlayer();
                Input.LastPressed = null;
                
            }
            pbCanvas.Invalidate();
        }

        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                Point snakePoint = Snake[i];

                if (i == 0)
                {
                    switch (SnakeGame.SnakeProperties.direction)
                    {
                        case Directions.Right:
                            snakePoint.X++;
                            break;
                        case Directions.Left:
                            snakePoint.X--;
                            break;
                        case Directions.Up:
                            snakePoint.Y--;
                            break;
                        case Directions.Down:
                            snakePoint.Y++;
                            break;
                    }

                }
                else
                {
                    snakePoint.X = Snake[i - 1].X;
                    snakePoint.Y = Snake[i - 1].Y;
                }

                // Detect head colliding with food
                if (Food.Contains(Snake[0]))
                {
                    snakeEat(Snake[0]);
                }

                // Wraparound logic
                if (snakePoint.X >= maxXpos) snakePoint.X %= maxXpos;
                if (snakePoint.Y >= maxXpos) snakePoint.Y %= maxYpos;
                if (snakePoint.X < 0) snakePoint.X = maxXpos;
                if (snakePoint.Y < 0) snakePoint.Y = maxYpos;
                Snake[i] = snakePoint;
            }
            // Detect head colliding with body
            if ((Snake.Where(x => x.Equals(Snake[0])).Count() == 2))
            {
                snakeDie();
            }
        }

        private void snakeDie()
        {
            SnakeGame.SnakeProperties.GameOver = true;
        }

        private void snakeEat(Point p)
        {
            Point body = new Point { X = Snake[Snake.Count - 1].X, Y = Snake[Snake.Count - 1].Y };
            Snake.Add(body);
            SnakeGame.SnakeProperties.Score += SnakeGame.SnakeProperties.Points;
            label2.Text = SnakeGame.SnakeProperties.Score.ToString();
            label5.Text = Snake.Count.ToString();
            Food.Remove(p);
            generateFood();
        }


        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            

            if (!SnakeGame.SnakeProperties.GameOver)
            {
                Brush snakeColor;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Aqua;
                    }
                    else
                    {
                        snakeColor = Brushes.Cyan;
                    }
                    //canvas.FillEllipse(snakeColor,
                    //                    new Rectangle(
                    //                        Snake[i].X * SnakeSettings.Width,
                    //                        Snake[i].Y * SnakeSettings.Height,
                    //                        SnakeSettings.Width, SnakeSettings.Height));
                    canvas.FillRectangle(snakeColor,
                                            new Rectangle(
                                            Snake[i].X * SnakeGame.SnakeProperties.Width,
                                            Snake[i].Y * SnakeGame.SnakeProperties.Height,
                                            SnakeGame.SnakeProperties.Width, SnakeGame.SnakeProperties.Height));

                    foreach (var food in Food)
                    {
                        canvas.FillRectangle(Brushes.HotPink,
                                        new Rectangle(
                                            food.X * SnakeGame.SnakeProperties.Width,
                                            food.Y * SnakeGame.SnakeProperties.Height,
                                            SnakeGame.SnakeProperties.Width, SnakeGame.SnakeProperties.Height
                                            ));
                    }

                    for (int  j = 0; j < maxXpos; j++)
                    {
                        canvas.DrawLine(new Pen(Color.White), new Point(j * SnakeGame.SnakeProperties.Width, 0), new Point(j * SnakeGame.SnakeProperties.Width, pbCanvas.Size.Height));
                    }
                    for (int k = 0; k < maxYpos; k++)
                    {
                        canvas.DrawLine(new Pen(Color.White), new Point(0, k * SnakeGame.SnakeProperties.Height), new Point(pbCanvas.Size.Width, k * SnakeGame.SnakeProperties.Width));
                    }

                }

            }
            else
            {
                string gameOver = "Game Over\nFinal Score is " + SnakeGame.SnakeProperties.Score + "\nPress enter to Restart\n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
