using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ArrayList;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace _7x7_Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public enum GameScreen
    {
        MainMenuScreen, PlayingScreen, GameOverScreen, GameWelCome, Achievements,HighScores
    }

    public enum Ball
    {
        None, Blue, Green, Red, Violet, Yellow
    }

    public class ScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Viewport viewport;
        
        private SpriteBatch spriteBatch;

        public static GameScreen prevFrameScreen = GameScreen.GameWelCome;

        public static GameScreen currentScreen = GameScreen.MainMenuScreen;

        private Texture2D _menu2D;

        private Texture2D _ball2D;

        private Rectangle[] _menuRec;

        private Button AllowMove;

        private Button Return;

        private SpriteFont _menusSpriteFont;

        private int _padding = 50;

        private float _scale = 0.3f;

        private float _scale_ball;

        private int _btWidth;

        private int _btHeight;

        private int _buttonIndex = 0;

        private int _x_ball;

        private int _y_ball;

        private int _padding_cell = 65;

        private int[,] _array;

        private int isNewGame = 0;

        private Texture2D ball_none;

        private Texture2D ball_blue;

        private Texture2D ball_green;

        private Texture2D ball_red;

        private Texture2D ball_yellow;

        private Texture2D ball_violet;

        /// <summary>
        /// ball pressed
        /// </summary>

        private Texture2D ball_blue1;

        private Texture2D ball_green1;

        private Texture2D ball_red1;

        private Texture2D ball_yellow1;

        private Texture2D ball_violet1;

        private Point _selected;

        private Point _pre;

        private bool _moved = false;

        private bool _canmove = false;

        private bool _can_del = false;

        private bool _pressed = false;

        private bool _draw_path = false;

        private Point _ball_pressed;

        private ListPoint ListPath;

        public bool GameInProgress = false;

        public ScreenManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();

        }


        protected override void LoadContent()
        {
            base.LoadContent();
            _menu2D = Game.Content.Load<Texture2D>(@"Menu");
            _menusSpriteFont = Game.Content.Load<SpriteFont>(@"menuLarge");
            _ball2D = Game.Content.Load<Texture2D>("Ball");
            _menuRec=new Rectangle[4];
            _btWidth = _menu2D.Width/2;
            _btHeight = _menu2D.Height/4;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            //////
            _x_ball = (viewport.Width - _padding_cell*7)/2;

            _y_ball = viewport.Height-_x_ball - _padding_cell*7;

            //--Load Ball--//
            ball_none = Game.Content.Load<Texture2D>(@"Color/None");
            ball_blue = Game.Content.Load<Texture2D>(@"Color/Blue");
            ball_green = Game.Content.Load<Texture2D>(@"Color/Green");
            ball_red = Game.Content.Load<Texture2D>(@"Color/Red");
            ball_yellow = Game.Content.Load<Texture2D>(@"Color/Yellow");
            ball_violet = Game.Content.Load<Texture2D>(@"Color/Violet");

            //--Load Ball--//

            ball_blue1 = Game.Content.Load<Texture2D>(@"Color/Blue1");
            ball_green1 = Game.Content.Load<Texture2D>(@"Color/Green1");
            ball_red1 = Game.Content.Load<Texture2D>(@"Color/Red1");
            ball_yellow1 = Game.Content.Load<Texture2D>(@"Color/Yellow1");
            ball_violet1 = Game.Content.Load<Texture2D>(@"Color/Violet1");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            
            
            TouchCollection touchCollection = TouchPanel.GetState();

            if (currentScreen != prevFrameScreen)
            {
                SetupGestures();

                if (currentScreen == GameScreen.PlayingScreen)
                {
                    GameInProgress = true;
                }

                prevFrameScreen = currentScreen;
            }
            HandleMenuButton();
            UpdateInput(gameTime);
            ButtonCheckState(touchCollection);          
            //Check Current

                    while (TouchPanel.IsGestureAvailable&&GameInProgress)
                    {
                        GestureSample gs = TouchPanel.ReadGesture();

                        switch (gs.GestureType)
                        {
                            case GestureType.Tap:
                                int x = (int)(gs.Position.X - _x_ball) / _padding_cell;
                                int y = (int)(gs.Position.Y - _y_ball) / _padding_cell;
                                Point current = new Point(x, y);
                                if ((current.X < 7) && (current.Y < 7) && (current.X >= 0) && (current.Y >= 0))
                                {

                                    if (_pre!=current)
                                    {
                                        _selected = _pre;
                                        _pre = current;
                                    }


                                    if (_array[current.X, current.Y] != 0)
                                    {
                                        _pressed = true;
                                        _ball_pressed = new Point(current.X, current.Y);
                                    }

                                }
                                if (_array[_selected.X, _selected.Y] != 0&&_array[current.X,current.Y]==0)
                                {
                                    ListPath = new ListPoint();
                                    ListPath= Algorithms.Algorithms.havePath(_array, _selected.X, _selected.Y, current.X, current.Y);

                                        if (ListPath != null)
                                        {
                                            _moved = true;
                                            _draw_path = true;
                                        }
                                        else
                                        {
                                            _moved = false;
                                            _pressed = false;
                                        }

                                }
                                //else if (_pressed && _array[_selected.X, _selected.Y] != 0 && current.X == _selected.X && current.Y == _selected.Y)
                                //{
                                //    _pressed = false;
                                //}


                                if (_moved)
                                {
                                    //random_ball_next(_array);
                                    _array[current.X, current.Y] = _array[_selected.X, _selected.Y];
                                    _array[_selected.X, _selected.Y] = 0;
                                     current = _pre = _selected;
                                    _moved = false;
                                    _pressed = false;
                                    ListPoint list_del = new ListPoint();
                                    list_del = Algorithms.Algorithms.KiemTra(_array);
                                        if (list_del.Count >= 4)
                                        {
                                            Algorithms.Algorithms.del_ball(_array, list_del);
                                        }
                                        else
                                        {
                                            random_ball_next(_array);
                                            list_del = Algorithms.Algorithms.KiemTra(_array);
                                            if (list_del.Count >= 4)
                                            {
                                                Algorithms.Algorithms.del_ball(_array, list_del);
                                            }
                                        }

                                }

                                Debug.WriteLine(current.ToString()+_pre.ToString()+_selected.ToString()+_moved.ToString()+_pressed.ToString());
                                break;
                            default: 
                                break;
                        }
                    }
           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.WhiteSmoke);
            switch (currentScreen)
            {
                    case GameScreen.MainMenuScreen:

                    spriteBatch.Begin();
                    
                        for (int i = 1; i < 5; i++)
                        {                            
                            spriteBatch.Draw(_menu2D, new Vector2(50, (i - 1) * (_btHeight*_scale + _padding)+100),
                                             new Rectangle(0, (i - 1) * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                            _menuRec[i - 1] = new Rectangle(50, (i - 1) * (_btHeight*3/10  + _padding) + 100,viewport.Width,_btHeight*3/10);
                        }
                        
                        spriteBatch.DrawString(_menusSpriteFont, "Resume Game", new Vector2(_btWidth*_scale + 100, (_btHeight * _scale + _padding/2)), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "New Game", new Vector2(_btWidth * _scale + 100, 2* (_btHeight * _scale + _padding / 2) + _padding/2), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "Achievements", new Vector2(_btWidth * _scale + 100, 3 * (_btHeight * _scale + _padding) - _padding/2), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "High Scores", new Vector2(_btWidth * _scale + 100, 4 * (_btHeight * _scale + _padding / 2) + _padding + _padding/2), Color.Black);
                        
                    spriteBatch.End();
 

                    break;

                    case GameScreen.PlayingScreen:

                    spriteBatch.Begin();

                            //if (isNewGame==2)//new game
                            //{

                                for (int i = 0; i < 7; i++)
                                    for (int j = 0; j < 7; j++)
                                    {
                                        if (_array[i, j] == (int)Ball.Blue)
                                        {
                                            spriteBatch.Draw(ball_blue,new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1),Color.White );
                                        }
                                        else if (_array[i, j] == (int)Ball.Green)
                                        {
                                            spriteBatch.Draw(ball_green, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1), Color.White);

                                        }
                                        else if (_array[i, j] == (int)Ball.Red)
                                        {
                                            spriteBatch.Draw(ball_red, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1), Color.White);

                                        }
                                        else if (_array[i, j] == (int)Ball.Violet)
                                        {
                                            spriteBatch.Draw(ball_violet, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1), Color.White);

                                        }
                                        else if (_array[i, j] == (int)Ball.Yellow)
                                        {
                                            spriteBatch.Draw(ball_yellow, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1), Color.White);
                                        }
                                        else if(_array[i, j]==(int)Ball.None)
                                        {
                                            spriteBatch.Draw(ball_none, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell -1, _padding_cell -1), Color.White);
                                        }

                                    }
                                if (_pressed)
                                {
                                    if (_array[_ball_pressed.X, _ball_pressed.Y] == (int)Ball.Blue)
                                    {
                                        spriteBatch.Draw(ball_blue1, new Vector2(_x_ball - 8 + _padding_cell * _ball_pressed.X, _y_ball - 8 + _padding_cell * _ball_pressed.Y), new Rectangle(0, 0, _padding_cell - 1 + 17, _padding_cell - 1 + 17), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                                    }
                                    else if (_array[_ball_pressed.X, _ball_pressed.Y] == (int)Ball.Green)
                                    {
                                        spriteBatch.Draw(ball_green1, new Vector2(_x_ball - 8 + _padding_cell * _ball_pressed.X, _y_ball - 8 + _padding_cell * _ball_pressed.Y), new Rectangle(0, 0, _padding_cell - 1 + 17, _padding_cell - 1 + 17), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                                    }
                                    else if (_array[_ball_pressed.X, _ball_pressed.Y] == (int)Ball.Red)
                                    {
                                        spriteBatch.Draw(ball_red1, new Vector2(_x_ball - 8 + _padding_cell * _ball_pressed.X, _y_ball - 8 + _padding_cell * _ball_pressed.Y), new Rectangle(0, 0, _padding_cell - 1 + 17, _padding_cell - 1 + 17), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                                    }
                                    else if (_array[_ball_pressed.X, _ball_pressed.Y] == (int)Ball.Violet)
                                    {
                                        spriteBatch.Draw(ball_violet1, new Vector2(_x_ball - 8 + _padding_cell * _ball_pressed.X, _y_ball - 8 + _padding_cell * _ball_pressed.Y), new Rectangle(0, 0, _padding_cell - 1 + 17, _padding_cell - 1 + 17), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                                    }
                                    else if (_array[_ball_pressed.X, _ball_pressed.Y] == (int)Ball.Yellow)
                                    {
                                        spriteBatch.Draw(ball_yellow1, new Vector2(_x_ball - 8 + _padding_cell * _ball_pressed.X, _y_ball - 8 + _padding_cell * _ball_pressed.Y), new Rectangle(0, 0, _padding_cell - 1 + 17, _padding_cell - 1 + 17), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                                    }
                                }

                                if (_draw_path)
                                {
                        
                                }

                                #region test
                                //----------//
                                //for (int i = 0; i < 7; i++)
                                //    for (int j = 0; j < 7; j++)
                                //    {
                                //        if (_array[i, j] == (int)Ball.Blue)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "1", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);
                                //        }
                                //        else if (_array[i, j] == (int)Ball.Green)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "2", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);

                                //        }
                                //        else if (_array[i, j] == (int)Ball.Red)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "3", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);

                                //        }
                                //        else if (_array[i, j] == (int)Ball.Violet)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "4", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);

                                //        }
                                //        else if (_array[i, j] == (int)Ball.Yellow)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "5", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);
                                //        }
                                //        else if (_array[i, j] == (int)Ball.None)
                                //        {
                                //            spriteBatch.DrawString(_menusSpriteFont, "0", new Vector2(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j), Color.Black);
                                //        }

                                //    }
                                #endregion
                            //}
                            //else if(isNewGame==1)//resume
                            //{
                            //    ResumeGame();
                            //}

                    spriteBatch.End();
                            
                    break;

                    case GameScreen.Achievements:

                        GraphicsDevice.Clear(Color.SlateGray);
                        break;
                    case GameScreen.HighScores:
                         GraphicsDevice.Clear(Color.BurlyWood);
                         break;
                    case GameScreen.GameOverScreen:
                        break;
            }
            //spriteBatch.DrawString(_menusSpriteFont, currentScreen.ToString() + "\n" + prevFrameScreen.ToString(), new Vector2(_btWidth * _scale + 100, 4 * (_btHeight * _scale + _padding / 2) + _padding + _padding / 2), Color.Black);
        }

        private void UpdateInput(GameTime gameTime)
        {
            if (HandleBackButton())
                return;
        }

        private bool HandleBackButton()
        {
            bool backPressed = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;

            if (backPressed)
            {
                if (currentScreen == GameScreen.MainMenuScreen)
                {
                    Game.Exit();
                }
                else if(currentScreen == GameScreen.PlayingScreen||
                         currentScreen==GameScreen.Achievements||
                         currentScreen==GameScreen.HighScores||
                         currentScreen==GameScreen.GameOverScreen)
                {
                    currentScreen = GameScreen.MainMenuScreen;
                    _buttonIndex = 0;
                }
            }

            return backPressed;
        }

        private void HandleMenuButton()
        {
            if (currentScreen == GameScreen.MainMenuScreen)
            {
                if (_buttonIndex == 1)//resume
                {
                    ResetTouchPanel();
                    isNewGame = 1;
                }
                else if (_buttonIndex == 2)//new game
                {
                    ResetTouchPanel();
                    currentScreen = GameScreen.PlayingScreen;
                    isNewGame = 2;
                    _array = CreateArray(7, 7);
                    TouchCollection touchCollection = TouchPanel.GetState();
                    _pressed = false;
                }
                else if (_buttonIndex == 3)//achievements
                {
                    ResetTouchPanel(); 
                    currentScreen = GameScreen.Achievements;
                }
                else if (_buttonIndex == 4)//high scores
                {
                    ResetTouchPanel(); 
                    currentScreen = GameScreen.HighScores;
                }
                else
                {
                    ResetTouchPanel(); 
                    currentScreen = GameScreen.MainMenuScreen;
                }
            }
        }

        private void ButtonCheckState(TouchCollection touches)
        {
            if (touches.Count > 0 && touches[0].State ==
     TouchLocationState.Pressed)
            {
                // Examine whether the tapped position is in the rectangle
                Point touchPoint = new
                    Point((int)touches[0].Position.X,
                    (int)touches[0].Position.Y);
                if (currentScreen == GameScreen.MainMenuScreen)
                {
                    if (_menuRec[0].Contains(touchPoint))
                    {
                        _buttonIndex = 1;
                    }

                    else if (_menuRec[1].Contains(touchPoint))
                    {
                        _buttonIndex = 2;
                    }

                    else if (_menuRec[2].Contains(touchPoint))
                    {
                        _buttonIndex = 3;
                    }
                    else if (_menuRec[3].Contains(touchPoint))
                    {
                        _buttonIndex = 4;
                    }
                    else
                    {
                        _buttonIndex = 0;
                    }
                }
            }
        }

        #region SetupGestures
        private void SetupGestures()
        {
            switch (currentScreen)
            {
                case GameScreen.MainMenuScreen:
                    TouchPanel.EnabledGestures = GestureType.Tap;
                    break;

                case GameScreen.PlayingScreen:
                    TouchPanel.EnabledGestures = GestureType.Tap;
                    break;

                case GameScreen.Achievements:
                    TouchPanel.EnabledGestures = GestureType.Tap;
                    break;

                case GameScreen.HighScores:
                    TouchPanel.EnabledGestures = GestureType.Tap;
                    break;

                case GameScreen.GameWelCome:
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;

                case GameScreen.GameOverScreen:
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;
                default:
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;
            }
        }
        #endregion

        #region Create Array
        private int[,] CreateArray(int a, int b)
        {
            int[,] Array = new int[a, b];

            for (int i = 0; i < a; i++)
                for (int j = 0; j < b; j++)
                {
                    Array[i, j] = (int)_7x7_Game.Ball.None;
                }

            Random rand = new Random();
            List<int> listRandomNumber1 = new List<int>();
            List<int> listRandomNumber2 = new List<int>();
            int temp = 0;
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();
            for (int i = 0; i < a; i++)
            {
                list1.Add(i);
            }
            //random
            for (int i = 0; i < a; i++)
            {
                temp = rand.Next(list1.Count);
                listRandomNumber1.Add(list1[temp]);
                list1.RemoveAt(temp);
            }
            for (int i = 0; i < b; i++)
            {
                list2.Add(i);
            }
            for (int i = 0; i < b; i++)
            {
                temp = rand.Next(list2.Count);
                listRandomNumber2.Add(list2[temp]);
                list2.RemoveAt(temp);
            }

            Array[listRandomNumber1[0], listRandomNumber2[0]] = rand.Next(1, 6);
            Array[listRandomNumber1[1], listRandomNumber2[1]] = rand.Next(1, 6);
            Array[listRandomNumber1[2], listRandomNumber2[2]] = rand.Next(1, 6);

            return Array;
        }
        #endregion

        #region Create Random Ball Next
        public int random_ball_next(int[,] Array)
        {
            Random rand = new Random();
            ListPoint listpoint = new ListPoint();
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (Array[i, j] == (int)Ball.None)
                    {
                        listpoint.Add(new List { X = i, Y = j });
                    }
                }
            }
            if (listpoint.Count == 0) return 0;
            int temp;
            for (int i = 0; i < 3; i++)
            {
                temp = rand.Next(listpoint.Count);
                Array[listpoint[temp].X, listpoint[temp].Y] = rand.Next(1,6);             
            }
            return 1;
        }
        #endregion

        private void ResumeGame()
        {
            
        }

        private void ResetTouchPanel()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }
    }
}
