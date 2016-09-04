using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoadCrosser
{
    public partial class Form1 : Form
    {
        /* Note that a lot of these settings are being overwritten later on in the code. */
        private bool gameActive = false;
        private bool gameOver = false;
        private bool gamePaused = false;

        private int gameSpeed = 8; // ~ 120 fps (1000 / 120)
        private double vehicleSpeed = 1;
        private int level = 1;

        private int width = 1024;
        private int height = 768;

        private double vehicleDelay = 2000; // MS - spawn speed
        private double maxVehiclesOnScreen = 2;
        private DateTime lastSpawnedVehicle = DateTime.Now;

        private double roadCount = 3;
        private int heightOfRoads = 0;

        private int playerGridSize = 64;

        private float playerWidth = 128;
        private int vehicleWidth = 128;

        private bool isPlayerFrozen = false;


        private int topScore = RoadCrosser.Properties.Settings.Default.maxLevel;

        List<Road> roads = new List<Road>();
        List<Car> carsList = new List<Car>();

        Player player;
        Image playerImage = RoadCrosser.Properties.Resources.player_char_transparent;
        Image roadImage = RoadCrosser.Properties.Resources.Asphalt;
        Image sidewalkImage = RoadCrosser.Properties.Resources.Sidewalk;

        Timer mainGame = new Timer();
        Timer moveCars = new Timer();

        Image[] vehicleImages = new Image[7];

        Font f = null;
        Font fMid = new Font("Arial", 14);
        Font fBig = new Font("Arial", 16, FontStyle.Bold);

        Pen p = new Pen(Brushes.Yellow, 2); // used for the lines in between roads

        TextureBrush texture = null;
        TextureBrush sidewalkTexture = null;


        public Form1()
        {
            InitializeComponent();

            vehicleImages[0] = RoadCrosser.Properties.Resources.bike;
            vehicleImages[1] = RoadCrosser.Properties.Resources.bus;
            vehicleImages[2] = RoadCrosser.Properties.Resources.car1_spr;
            vehicleImages[3] = RoadCrosser.Properties.Resources.green_car;
            vehicleImages[4] = RoadCrosser.Properties.Resources.Porsche_911;
            vehicleImages[5] = RoadCrosser.Properties.Resources.red_car;
            vehicleImages[6] = RoadCrosser.Properties.Resources.trashmaster;

            texture = new TextureBrush(roadImage);
            sidewalkTexture = new TextureBrush(sidewalkImage);
            f = this.Font;

            mainGame.Tick += new EventHandler(UpdateGameScreen);
            moveCars.Tick += new EventHandler(moveVehicle);

            mainGame.Interval = gameSpeed;
            mainGame.Start();

            moveCars.Interval = Convert.ToInt32(Math.Round(vehicleSpeed));
            moveCars.Start();



        }

        private void moveVehicle(object sender, EventArgs e)
        {
            foreach (var car in carsList)
            {
                // moves car to the left - amount depends on the vehicle speed
                car.X -= Convert.ToInt32(Math.Round(vehicleSpeed));
            }
        }

        private void HandleLevelDifficulty()
        {
            // I tried to make it a bit dynamic. Might not be really optimized (for higher levels). That's why I kept the 'oldlevelcode' here as well.
            if (IsPrime(Convert.ToInt32(level)))
            {
                // Amount of roads increases every 2 levels  - no cap
                roadCount++;
            }

            if (vehicleSpeed < 20)
            {
                // vehicles move half a pixel faster after each level
                // caps at 20
                vehicleSpeed += 0.5;
            }

            // the time between vehicle spawn times increases by a certain number in ms every level, until it reaches 1 spawn every ...ms (see if)
            // caps at the end
            if (vehicleDelay > 800)
            {
                vehicleDelay -= 125;
            }
            else if (vehicleDelay > 300)
            {
                vehicleDelay -= 50;
            }
            else if (vehicleDelay > 200)
            {
                vehicleDelay -= 10;
            }
            else if (vehicleDelay > 150)
            {
                vehicleDelay -= 5;
            }

            // the amount of vehicles that can be on screen at the same time. Increases with 0.9 vehicles (rounded number) every level
            // no cap
            maxVehiclesOnScreen = level * 0.9;

            #region oldlevelcode
            /* switch (level)
             {
                 case 1:
                     roadCount = 3;
                     vehicleSpeed = 1;
                     vehicleDelay = 2000;
                     maxVehiclesOnScreen = 2;
                     break;
                 case 2:
                     roadCount = 4;
                     vehicleSpeed = 2;
                     vehicleDelay = 1500;
                     maxVehiclesOnScreen = 4;
                     break;
                 case 3:
                     roadCount = 5;
                     vehicleSpeed = 3;
                     vehicleDelay = 1100;
                     maxVehiclesOnScreen = 6;
                     break;
                 case 4:
                     roadCount = 6;
                     vehicleSpeed = 4;
                     vehicleDelay = 700;
                     maxVehiclesOnScreen = 7;
                     break;
                 case 5:
                     roadCount = 6;
                     vehicleSpeed = 5;
                     vehicleDelay = 450;
                     maxVehiclesOnScreen = 8;
                     break;
                 case 6:
                     roadCount = 7;
                     vehicleSpeed = 5;
                     vehicleDelay = 420;
                     maxVehiclesOnScreen = 9;
                     break;
                 case 7:
                     roadCount = 8;
                     vehicleSpeed = 6;
                     vehicleDelay = 380;
                     maxVehiclesOnScreen = 10;
                     break;
                 case 8:
                     roadCount = 8;
                     vehicleSpeed = 6;
                     vehicleDelay = 350;
                     maxVehiclesOnScreen = 15;
                     break;
                 case 9:
                     roadCount = 8;
                     vehicleSpeed = 7;
                     vehicleDelay = 300;
                     maxVehiclesOnScreen = 20;
                     break;
                 case 10:
                     roadCount = 8;
                     vehicleSpeed = 8;
                     vehicleDelay = 250;
                     maxVehiclesOnScreen = 20;
                     break;
                 case 11:
                     roadCount = 9;
                     vehicleSpeed = 9;
                     vehicleDelay = 220;
                     maxVehiclesOnScreen = 22;
                     break;
                 case 12:
                     roadCount = 9;
                     vehicleSpeed = 10;
                     vehicleDelay = 200;
                     maxVehiclesOnScreen = 25;
                     break;
                 case 13:
                     roadCount = 10;
                     vehicleSpeed = 11;
                     vehicleDelay = 200;
                     maxVehiclesOnScreen = 30;
                     break;
                 case 14:
                     roadCount = 10;
                     vehicleSpeed = 12;
                     vehicleDelay = 200;
                     maxVehiclesOnScreen = 30;
                     break;
                 case 15:
                     roadCount = 11;
                     vehicleSpeed = 13;
                     vehicleDelay = 170;
                     maxVehiclesOnScreen = 35;
                     break;
                 default:
                     break;
             }*/
            #endregion
        }

        private static bool IsPrime(int number)
        {
            // used to decide the level difficulty
            int boundary = (int)Math.Floor(Math.Sqrt(number));

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        private void UpdateGameScreen(object sender, EventArgs e)
        {
            // height and width is always forced
            this.Height = height;
            this.Width = width;

            // code that could be used for a 'PAUSE' screen, not in use right now
           /* if (!this.Focused && level > 0 || gamePaused)
            {
                gameActive = false;
                gamePaused = true;
            }*/

            // as long as the game is active
            if (gameActive)
            {
                //HandleLevelDifficulty();
                GenerateRoads();

                if (carsList.Count > 0)
                {
                    CheckVehicleState(false);
                }
                else
                {
                    // If no cars are visible (list is empty), this probably means a new level just started.
                    // If the boolean value is set to true, this will force cars to be spawned randomly over the road
                    CheckVehicleState(true);
                }

            }

            // Needed to force a refresh of the screen
            pctGame.Invalidate();
        }

        private void GenerateRoads()
        {

            while (roads.Count <= roadCount)
            {
                if (roads.Count == 0)
                {
                    // This road is being spawned at the top (outside screen region). It is a safe road, and no traffic spawns there.
                    heightOfRoads = this.pctGame.Size.Height / Convert.ToInt32(Math.Round(roadCount));
                    Road road = new Road(roads.Count, 0, this.pctGame.Location.Y - heightOfRoads, this.pctGame.Size.Height / Convert.ToInt32(Math.Round(roadCount)), this.pctGame.Width, true);
                    roads.Add(road);
                }
                else
                {
                    // Every road spawns below the previous road (using the heightOfRoads variable). Vehicles can spawn on these roads.
                    Road road = new Road(roads.Count, 0, roads[roads.Count - 1].Y + heightOfRoads, this.pctGame.Size.Height / Convert.ToInt32(Math.Round(roadCount)), this.pctGame.Width, false);
                    roads.Add(road);
                }

            }

            // The bottom/last road should also be safe from traffic ('sidewalk'). This is the starting position for the player.
            Road bottomRoad = roads.LastOrDefault();
            bottomRoad.IsSafe = true;

        }

        private void Draw(Graphics canvas)
        {



            if (!gameOver && !gamePaused)
            {
                // start screen
                canvas.DrawString("ROADCROSSER", fBig, Brushes.Black, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 - 30));
                canvas.DrawString("Press ENTER to start a new game.", f, Brushes.Black, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2));
            }
            if (gameActive)
            {
                // These things are redrawn every refresh

                texture.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                sidewalkTexture.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipX;

                foreach (var road in roads)
                {
                    // draws roads, and fills it with road texture.
                    Rectangle rect = new Rectangle(road.X, road.Y, road.Width, road.Height);
                    canvas.FillRectangle(texture, rect);
                    if (road.IsSafe)
                    {
                        // The texture is overridden if it is a safe road
                        canvas.FillRectangle(sidewalkTexture, rect);
                    }
                    
                    canvas.DrawRectangle(p, rect); // Yellow lines in between roads

                    //canvas.DrawString(road.Id.ToString(), fMid, Brushes.White, new PointF(road.X + 10, road.Y + 10));
                }

                // delete car from list after it drives off screen, so new cars can spawn
                for (int i = 0; i < carsList.Count; i++)
                {
                    canvas.DrawImage(carsList[i].Image, carsList[i].X, carsList[i].Road.Y, vehicleWidth, carsList[i].Road.Height);
                    if (carsList[i].X < 0 - carsList[i].Image.Width)
                    {
                        carsList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                DetectCollision();
                GeneratePlayer();

                if (player != null && !isPlayerFrozen)
                {
                    // Only draw the player if it is initialized, AND if it is not frozen
                    canvas.DrawImage(playerImage, player.X, player.Y, playerWidth, heightOfRoads);
                }

                canvas.DrawString("Level: " + level, fMid, Brushes.White, new PointF(5, 15));
            }
            if (gameOver)
            {
                bool isRecordBeaten = false;
                if (topScore < level)
                {
                    isRecordBeaten = true;
                }

                canvas.DrawString("YOU DIED!", fBig, Brushes.White, new PointF(pctGame.Width / 2, pctGame.Height / 4));
                canvas.DrawString("You were hit by a vehicle!", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2));
                if (isRecordBeaten)
                {

                    canvas.DrawString("You reached level " + level + ", which means you", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 30));
                    canvas.DrawString("have beaten your topscore of " + topScore + "!", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 50));
                    canvas.DrawString("Congratulations! ", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 70));
                    RoadCrosser.Properties.Settings.Default.maxLevel = level;
                    RoadCrosser.Properties.Settings.Default.Save();
                }
                else
                {
                    canvas.DrawString("You reached level " + level + ".", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 30));
                    canvas.DrawString("Your highest score is " + topScore + ".", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 52));
                }

                canvas.DrawString("Press ENTER to restart. ", fMid, Brushes.White, new PointF(pctGame.Width / 2 - 2, pctGame.Height / 2 + 95));

            }

        }

        private void DetectCollision()
        {
            if (player != null && carsList != null) // these things need to be defined first
            {
                foreach (var car in carsList)
                {
                    // playerGridSize is being used as an offset
                    int minPosition = car.X - vehicleWidth + playerGridSize / 2; 
                    int maxPosition = car.X + vehicleWidth - playerGridSize / 2;

                    if (player != null && player.RoadId == car.Road.Id && player.X >= minPosition && player.X <= maxPosition) // collision
                    {
                        gameOver = true;
                        vehicleSpeed = 0;
                    }
                }
            }
        }

        private void GeneratePlayer()
        {
            if (player == null && roads.Count > 0)
            {
                // Player spawns at the bottom road, in the center, final parameter is road ID
                player = new Player(roads[roads.Count - 1].Width / 2, roads[roads.Count - 1].Y, roads.Count - 1);
            }
        }

        private void DetectKeyPress(object sender, KeyEventArgs ke)
        {
            if (gameActive && !gameOver && !isPlayerFrozen)
            {
                if (ke.KeyData == Keys.Left)
                {
                    if (player.X > 0)
                    {
                        player.X -= playerGridSize;
                    }
                }
                else if (ke.KeyData == Keys.Right)
                {
                    if ((player.X + playerWidth) < this.Size.Width)
                    {

                        player.X += playerGridSize;
                    }
                }
                else if (ke.KeyData == Keys.Up)
                {
                    foreach (var road in roads)
                    {
                        if (road.Id == player.RoadId && player.RoadId > 0)
                        {
                            player.Y -= heightOfRoads;
                            if (player.RoadId - 1 > -1)
                            {
                                player.RoadId--;
                                if (player.RoadId <= 0)
                                {
                                    Increaselevel();
                                }
                                break;
                            }
                        }
                    }
                }
                else if (ke.KeyData == Keys.Down)
                {
                    foreach (var road in roads)
                    {
                        if (road.Id == player.RoadId && player.RoadId < roads.Count - 1)
                        {
                            player.Y += heightOfRoads;

                            if (player.RoadId < roads.Count - 1)
                            {
                                player.RoadId++;
                                break;
                            }
                        }
                    }
                }
            }


            if (ke.KeyData == Keys.Enter)
            {
                gameActive = true;
                if (gameOver)
                {

                    gameOver = false;
                    gamePaused = false;
                    player = null;
                    carsList.Clear();
                    roads.Clear();
                    level = 1;
                    roadCount = 3;
                    vehicleSpeed = 1;
                    vehicleDelay = 2000;
                    maxVehiclesOnScreen = 2;
                    topScore = RoadCrosser.Properties.Settings.Default.maxLevel;
                    //HandleLevelDifficulty();
                }
            }
        }

        private void CheckVehicleState(bool firstSpawn)
        {

            if (carsList.Count <= maxVehiclesOnScreen && DateTime.Now > lastSpawnedVehicle) // spawn a new vehicle
            {
                lastSpawnedVehicle = DateTime.Now.AddMilliseconds(vehicleDelay);
                int roadCount = roads.Count;
                Random r = new Random();
                int road = r.Next(1, roadCount - 1); // starts at 1, because road 0 is a safe road & - 1 because the bottom road should be safe from traffic
                int vehicle = r.Next(0, vehicleImages.Length);
                int initialSpawnLocation = r.Next(25, this.Size.Width);

                if (firstSpawn)
                {
                    // these vehicles are being spawned randomly (they don't drive in from the side)
                    // Only half of the needed vehicles are spawned this way, otherwise there will be a clear gap at the start of each level
                    while (carsList.Count <= (maxVehiclesOnScreen / 2))
                    {
                        road = r.Next(1, roadCount - 1);
                        vehicle = r.Next(0, vehicleImages.Length);
                        initialSpawnLocation = r.Next(0, this.Size.Width);
                        Car tempCar = new Car(vehicleImages[vehicle], roads[road], initialSpawnLocation, 10);
                        carsList.Add(tempCar);
                    }
                    isPlayerFrozen = false;
                }
                else
                {
                    if (!roads[road].IsSafe)
                    {
                        Car car = new Car(vehicleImages[vehicle], roads[road], this.Size.Width, 10);

                        carsList.Add(car);
                    }
                }
            }
        }

        private void Increaselevel()
        {

            level++;
            roads.Clear();
            carsList.Clear();
            player = null;
            // Freeze the player until all vehicles are spawned. Otherwise the player might walk into a car without seeing it
            // Freezing is also why the player disappears at the start of levels
            isPlayerFrozen = true;
            HandleLevelDifficulty();
            GenerateRoads();
            CheckVehicleState(true);
            GeneratePlayer();
        }

        private void pctGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Draw(canvas);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            DetectKeyPress(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
