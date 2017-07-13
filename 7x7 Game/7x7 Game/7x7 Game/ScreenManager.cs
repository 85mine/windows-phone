using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
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
        MainMenuScreen, PlayingScreen, GameOverScreen, GameWelCome, Achievements, HighScores,Help
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

        private Texture2D _gameOver2D;

        private Texture2D _comingSoon2D;

        private Texture2D _help12D;

        private Texture2D _help22D;

        private Texture2D _help32D;

        private Rectangle[] _menuRec;

        private Button _move_bt;

        private Button _return_bt;

        private Button _help_bt;

        private int _return_count = 0;

        private int _move_count = 0;

        private bool choice_show = false;

        private SpriteFont _menusSpriteFont;

        private SpriteFont _playSpriteFont;

        private SpriteFont _scoreSpriteFont;

        private SpriteFont _GameOverSpriteFont;

        private SoundEffect destroy_sound;

        private SoundEffect cantmove_sound;

        private int _padding = 50;

        private float _scale = 0.3f;

        private float _scale_ball;

        private int _btWidth;

        private int _btHeight;

        private int _buttonIndex = 0;

        private int _x_ball;

        private int _y_ball;

        private int _padding_cell = 62;

        private int _x_menu;

        private int _y_menu;

        private int _padding_menu = 60;

        private int[,] _array;

        private object[,] _objects_return;

        private int isNewGame = 0;

        private Texture2D _logo_game;

        private int _score = 0;

        private int _combo = 0;

        /// <summary>
        /// ball_normal
        /// </summary>
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

        private bool _draw_pressed = true;

        private bool _draw_path = false;

        private int _draw_ball = 0;

        private Point _ball_pressed;

        private ListPoint ListPath;

        private int[] _next_ball = new int[3];

        public bool GameInProgress = false;

        private int[,] _pre_map;

        private int[,] _selected_map;

        private int high_score = 0;

        private int max_combo = 0;

        /// <summary>
        /// Game Over
        /// </summary>

        private bool _game_over = false;

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
            _gameOver2D = Game.Content.Load<Texture2D>(@"game_over");
            _comingSoon2D = Game.Content.Load<Texture2D>(@"comingsoon");
            //Load Fonts
            _menusSpriteFont = Game.Content.Load<SpriteFont>(@"menuLarge");
            _playSpriteFont = Game.Content.Load<SpriteFont>(@"PlayFont");
            _scoreSpriteFont = Game.Content.Load<SpriteFont>(@"ScoreFont");
            _GameOverSpriteFont = Game.Content.Load<SpriteFont>(@"Gameover");
            _ball2D = Game.Content.Load<Texture2D>("Ball");
            _menuRec = new Rectangle[4];
            _btWidth = _menu2D.Width / 2;
            _btHeight = _menu2D.Height / 4;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            //////
            _x_ball = (viewport.Width - _padding_cell * 7) / 2;

            _y_ball = viewport.Height - _x_ball - _padding_cell * 7;

            _x_menu = 20;

            _y_menu = 100;

            //--Load Logo Game --//

            _logo_game = Game.Content.Load<Texture2D>(@"LogoGame");

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

            //
            ListPath = new ListPoint();
            // Create Button

            _return_bt = new Button(Game.Content, new Vector2(viewport.Width - 30 - 56, 10), 0.5f, "Return");
            _move_bt = new Button(Game.Content, new Vector2(viewport.Width - 30 - 56 - 56 - 10, 10), 0.5f, "Move");
            _help_bt=new Button(Game.Content,new Vector2(viewport.Width-70,viewport.Height-70),0.5f,"Help");

            _pre_map = new int[7, 7];
            _selected_map = new int[7, 7];

            //Help

            _help12D = Game.Content.Load<Texture2D>(@"Help1");
            _help22D = Game.Content.Load<Texture2D>(@"Move");
            _help32D = Game.Content.Load<Texture2D>(@"Return");
            //Load sound
            cantmove_sound = Game.Content.Load<SoundEffect>(@"canmovesound");
            destroy_sound = Game.Content.Load<SoundEffect>(@"destroysound");
            //check game over
             _game_over = LoadGameOver();
            //high score
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
                else
                {
                    GameInProgress = false;
                }
                prevFrameScreen = currentScreen;
            }
            if (currentScreen == GameScreen.PlayingScreen)
            {
                GameInProgress = true;
                _return_bt.Update(touchCollection, gameTime);
                _move_bt.Update(touchCollection, gameTime);
            }
            if (currentScreen == GameScreen.MainMenuScreen)
            {
                _help_bt.Update(touchCollection, gameTime);
                HandleMenuButton();
            }

            UpdateInput(gameTime);
            ButtonCheckState(touchCollection);
            //Check Current

            while (TouchPanel.IsGestureAvailable && GameInProgress)
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

                            if (_array[current.X, current.Y] != 0)
                            {
                                if (_pre.X == current.X && _pre.Y == current.Y)
                                {
                                    if (_pressed)
                                    {
                                        _pressed = false;
                                    }
                                    else
                                    {
                                        _pressed = true;
                                    }
                                }
                                else
                                {
                                    _pressed = true;
                                }

                                _ball_pressed = new Point(current.X, current.Y);
                            }


                            if (_pre != current)
                            {
                                _selected = _pre;
                                _pre = current;
                            }

                     
                        }
                        else
                        {
                            current = _selected = _pre;
                        }


                        if (_move_bt.isSelected == false)
                        {
                            if (_array[_selected.X, _selected.Y] != 0 && _array[current.X, current.Y] == 0)
                            {
                                ListPath = Algorithms.Algorithms.havePath(_array, _selected.X, _selected.Y,
                                                                          current.X, current.Y);

                                if (ListPath != null&&_pressed)
                                {
                                    _moved = true;
                                    _draw_path = true;
                                    _draw_ball = _array[_selected.X, _selected.Y];
                                }
                                else
                                {
                                    if (ListPath == null)
                                    {
                                        cantmove_sound.Play();
                                    }
                                    _moved = false;
                                    _pressed = false;
                                }

                            }

                            if (_moved)
                            {
                                _objects_return = _backup_map(_pre_map, _score, _combo, _move_bt.isSelected, _return_bt.isSelected, _move_count, _return_count);
                                _pre_map = _array;
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
                                    _score += list_del.Count * 5 * (1 + _combo);
                                    _combo++;
                                    if (max_combo < _combo)
                                    {
                                        max_combo = _combo;
                                    }
                                    //destroy_sound.Play();
                                }
                                else
                                {
                                    _combo = 0;
                                    random_ball_next(_array, _next_ball);
                                    list_del = Algorithms.Algorithms.KiemTra(_array);
                                    if (list_del.Count >= 4)
                                    {
                                        Algorithms.Algorithms.del_ball(_array, list_del);
                                        _score += list_del.Count * 5 * (1 + _combo);
                                        _combo++;

                                        if (max_combo < _combo)
                                        {
                                            max_combo = _combo;
                                        }
                                      //  destroy_sound.Play();
                                    }
                                    else
                                    {
                                        _combo = 0;
                                    }

                                    _next_ball = ballnext();
                                }
                            }
                        }
                        else
                        {
                            _objects_return = _backup_map(_pre_map, _score, _combo, _move_bt.isSelected,
                                                              _return_bt.isSelected, _move_count, _return_count+1);
                            _pre_map = _array;

                            if (_move_count <= 0)
                            {
                                _move_count = 0;
                                _move_bt.isSelected = false;
                            }
                            if (_array[_selected.X, _selected.Y] != 0 && _array[current.X, current.Y] == 0 && _move_bt.isSelected)
                            {
                                _array[current.X, current.Y] = _array[_selected.X, _selected.Y];
                                _array[_selected.X, _selected.Y] = 0;
                                current = _pre = _selected;
                                _move_bt.isSelected = false;
                                _move_count--;
                            }

                            ListPoint list_del = new ListPoint();
                            list_del = Algorithms.Algorithms.KiemTra(_array);
                            if (list_del.Count >= 4)
                            {
                                Algorithms.Algorithms.del_ball(_array, list_del);
                                _score += list_del.Count * 5 * (1 + _combo);
                                _combo++;

                                if (max_combo < _combo)
                                {
                                    max_combo = _combo;
                                }
                               // destroy_sound.Play();
                            }

                        }

                        if (Empty(_array))
                        {
                            random_ball_next(_array, ballnext());
                        }
                        if (_return_bt.isSelected && _return_count > 0)
                        {

                            _return_map(_objects_return);
                            _return_count--;

                            if (_return_count <= 0)
                            {
                                _return_count = 0;
                            }
                            _return_bt.isSelected = false;
                            _move_bt.isSelected = false;
                            _pressed = false;
                        }
                        else
                        {
                            _return_bt.isSelected = false;
                        }

                        save_game(_array, _score, _combo, _move_bt.isSelected, _return_bt.isSelected, _move_count, _return_count);

                        save_highscore(_score, max_combo, high_score);

                        if (the_end(_array) <= 0)
                        {
                            ResetTouchPanel();
                            currentScreen = GameScreen.GameOverScreen;
                            _game_over = true;
                            high_score = load_high_score()[0, 0];
                            max_combo = load_high_score()[0, 1];
                        }

                        SaveGameOver(_array);

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
            spriteBatch.Begin();

            switch (currentScreen)
            {
                case GameScreen.MainMenuScreen:

                    _help_bt.Draw(spriteBatch);

                    if (!_game_over)
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            spriteBatch.Draw(_menu2D, new Vector2(50, (i - 1) * (_btHeight * _scale + _padding) + 100),
                                             new Rectangle(0, (i - 1) * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                            _menuRec[i - 1] = new Rectangle(50, (i - 1) * (_btHeight * 3 / 10 + _padding) + 100, viewport.Width, _btHeight * 3 / 10);

                        }


                        if (_buttonIndex == 1)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 0 * (_btHeight * _scale + _padding) + 100),
                                             new Rectangle(_btWidth, 0, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }
                        else if (_buttonIndex == 2)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 1 * (_btHeight * _scale + _padding) + 100),
                                             new Rectangle(_btWidth, _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }
                        else if (_buttonIndex == 3)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 2 * (_btHeight * _scale + _padding) + 100),
                                             new Rectangle(_btWidth, 2 * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }
                        else if (_buttonIndex == 4)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 3 * (_btHeight * _scale + _padding) + 100),
                                             new Rectangle(_btWidth, 3 * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }

                        spriteBatch.DrawString(_menusSpriteFont, "Resume Game", new Vector2(_btWidth * _scale + 100, (_btHeight * _scale + _padding / 2)), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "New Game", new Vector2(_btWidth * _scale + 100, 2 * (_btHeight * _scale + _padding / 2) + _padding / 2), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "Achievements", new Vector2(_btWidth * _scale + 100, 3 * (_btHeight * _scale + _padding) - _padding / 2), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "High Scores", new Vector2(_btWidth * _scale + 100, 4 * (_btHeight * _scale + _padding / 2) + _padding + _padding / 2), Color.Black);

                    }
                    else
                    {
                        _menuRec[0] = new Rectangle(0,0,0,0);

                        for (int i = 2; i < 5; i++)
                        {
                            spriteBatch.Draw(_menu2D, new Vector2(50, (i - 1) * (_btHeight * _scale + _padding)),
                                             new Rectangle(0, (i - 1) * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                            _menuRec[i - 1] = new Rectangle(50, (i - 1) * (_btHeight * 3 / 10 + _padding), viewport.Width, _btHeight * 3 / 10);

                        }

                        if (_buttonIndex == 2)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 1 * (_btHeight * _scale + _padding)),
                                             new Rectangle(_btWidth, _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }
                        else if (_buttonIndex == 3)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 2 * (_btHeight * _scale + _padding)),
                                             new Rectangle(_btWidth, 2 * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }
                        else if (_buttonIndex == 4)
                        {

                            spriteBatch.Draw(_menu2D, new Vector2(50, 3 * (_btHeight * _scale + _padding)),
                                             new Rectangle(_btWidth, 3 * _btWidth, _btWidth, _btHeight), Color.White,
                                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                        }

                        spriteBatch.DrawString(_menusSpriteFont, "New Game", new Vector2(_btWidth * _scale + 100, 2 * (_btHeight * _scale + _padding / 2) + _padding / 2 -100), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "Achievements", new Vector2(_btWidth * _scale + 100, 3 * (_btHeight * _scale + _padding) - _padding / 2 - 100), Color.Black);
                        spriteBatch.DrawString(_menusSpriteFont, "High Scores", new Vector2(_btWidth * _scale + 100, 4 * (_btHeight * _scale + _padding / 2) + _padding + _padding / 2 - 100), Color.Black);

                    }

                    
                    break;

                case GameScreen.PlayingScreen:

                    _return_bt.Draw(spriteBatch);
                    _move_bt.Draw(spriteBatch);

                    spriteBatch.Draw(_logo_game, new Vector2(20, 10), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 0.125f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_playSpriteFont, "SCORE", new Vector2(_x_menu, _y_menu), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, _score.ToString(), new Vector2(_x_menu, _y_menu + 20), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, "UP NEXT", new Vector2(_x_menu + 230, _y_menu), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, "COMBO", new Vector2(_x_menu, _y_menu + 100), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, _combo.ToString() + "X", new Vector2(_x_menu, _y_menu + 120), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, _move_count.ToString(), new Vector2(viewport.Width - 30 - 56 - 20, 10), Color.WhiteSmoke);

                    spriteBatch.DrawString(_playSpriteFont, _return_count.ToString(), new Vector2(viewport.Width - 30 - 10, 10), Color.WhiteSmoke);


                    //Next Ball

                    for (int i = 0; i < 3; i++)
                    {
                        if (_next_ball[i] == (int)Ball.Blue)
                        {
                            spriteBatch.Draw(ball_blue, new Rectangle(_x_menu + 230 + _padding_menu * i, _y_menu + 35, _padding_menu - 1, _padding_menu - 1), Color.White);
                        }
                        else if (_next_ball[i] == (int)Ball.Green)
                        {
                            spriteBatch.Draw(ball_green, new Rectangle(_x_menu + 230 + _padding_menu * i, _y_menu + 35, _padding_menu - 1, _padding_menu - 1), Color.White);
                        }
                        else if (_next_ball[i] == (int)Ball.Red)
                        {
                            spriteBatch.Draw(ball_red, new Rectangle(_x_menu + 230 + _padding_menu * i, _y_menu + 35, _padding_menu - 1, _padding_menu - 1), Color.White);
                        }
                        else if (_next_ball[i] == (int)Ball.Violet)
                        {
                            spriteBatch.Draw(ball_violet, new Rectangle(_x_menu + 230 + _padding_menu * i, _y_menu + 35, _padding_menu - 1, _padding_menu - 1), Color.White);
                        }
                        else if (_next_ball[i] == (int)Ball.Yellow)
                        {
                            spriteBatch.Draw(ball_yellow, new Rectangle(_x_menu + 230 + _padding_menu * i, _y_menu + 35, _padding_menu - 1, _padding_menu - 1), Color.White);
                        }
                    }
                    //if (isNewGame==2)//new game
                    //{

                    for (int i = 0; i < 7; i++)
                        for (int j = 0; j < 7; j++)
                        {
                            if (_array[i, j] == (int)Ball.Blue)
                            {
                                spriteBatch.Draw(ball_blue, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);
                            }
                            else if (_array[i, j] == (int)Ball.Green)
                            {
                                spriteBatch.Draw(ball_green, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);

                            }
                            else if (_array[i, j] == (int)Ball.Red)
                            {
                                spriteBatch.Draw(ball_red, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);

                            }
                            else if (_array[i, j] == (int)Ball.Violet)
                            {
                                spriteBatch.Draw(ball_violet, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);

                            }
                            else if (_array[i, j] == (int)Ball.Yellow)
                            {
                                spriteBatch.Draw(ball_yellow, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);
                            }
                            else if (_array[i, j] == (int)Ball.None)
                            {
                                spriteBatch.Draw(ball_none, new Rectangle(_x_ball + _padding_cell * i, _y_ball + _padding_cell * j, _padding_cell - 1, _padding_cell - 1), Color.White);
                            }

                        }

                    if (_draw_path)
                    {
                        for (int i = 0; i < ListPath.Count; i++)
                        {
                            if (_draw_ball == (int)Ball.Blue)
                            {
                                spriteBatch.Draw(ball_blue,
                                                 new Rectangle(_x_ball + 9 + _padding_cell * ListPath[i].X,
                                                               _y_ball + 9 + _padding_cell * ListPath[i].Y,
                                                               _padding_cell - 1 - 17, _padding_cell - 1 - 17),
                                                 Color.White);
                            }
                            else if (_draw_ball == (int)Ball.Green)
                            {
                                spriteBatch.Draw(ball_green,
                                                 new Rectangle(_x_ball + 9 + _padding_cell * ListPath[i].X,
                                                               _y_ball + 9 + _padding_cell * ListPath[i].Y,
                                                               _padding_cell - 1 - 17, _padding_cell - 1 - 17),
                                                 Color.White);

                            }
                            else if (_draw_ball == (int)Ball.Red)
                            {
                                spriteBatch.Draw(ball_red,
                                                 new Rectangle(_x_ball + 9 + _padding_cell * ListPath[i].X,
                                                               _y_ball + 9 + _padding_cell * ListPath[i].Y,
                                                               _padding_cell - 1 - 17, _padding_cell - 1 - 17),
                                                 Color.White);

                            }
                            else if (_draw_ball == (int)Ball.Violet)
                            {
                                spriteBatch.Draw(ball_violet,
                                                 new Rectangle(_x_ball + 9 + _padding_cell * ListPath[i].X,
                                                               _y_ball + 9 + _padding_cell * ListPath[i].Y,
                                                               _padding_cell - 1 - 17, _padding_cell - 1 - 17),
                                                 Color.White);

                            }
                            else if (_draw_ball == (int)Ball.Yellow)
                            {
                                spriteBatch.Draw(ball_yellow,
                                                 new Rectangle(_x_ball + 9 + _padding_cell * ListPath[i].X,
                                                               _y_ball + 9 + _padding_cell * ListPath[i].Y,
                                                               _padding_cell - 1 - 17, _padding_cell - 1 - 17),
                                                 Color.White);
                            }
                        }

                        _draw_path = false;
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

                    break;

                case GameScreen.Achievements:

                    spriteBatch.Draw(_logo_game, new Vector2(20, 20), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);

                    spriteBatch.Draw(_comingSoon2D, new Vector2((viewport.Width - _comingSoon2D.Width) / 2, (viewport.Height - _comingSoon2D.Height) / 2), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    
                    break;
                case GameScreen.HighScores:

                    int _pddh = 150;

                    int _ppxh = 20;

                    high_score = load_high_score()[0, 0];

                    max_combo = load_high_score()[0, 1];

                    spriteBatch.Draw(_logo_game, new Vector2(_ppxh, _ppxh), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_playSpriteFont, "HIGH SCORE", new Vector2(_ppxh, _pddh), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, high_score.ToString(), new Vector2(_ppxh, _pddh + 20), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, "MAX COMBO", new Vector2(_ppxh, _pddh + 100), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, max_combo.ToString()+"X", new Vector2(_ppxh, _pddh + 100 + 20), Color.Black);

                    break;
                
                case GameScreen.GameOverScreen:

                    int _pdd = 150;

                    int _ppx = 20;

                    high_score = load_high_score()[0, 0];

                    max_combo = load_high_score()[0, 1];

                    spriteBatch.Draw(_logo_game, new Vector2(_ppx, _ppx), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);

                    spriteBatch.Draw(_gameOver2D, new Vector2((viewport.Width - _gameOver2D.Width) / 2, 350), new Rectangle(0, 0, _gameOver2D.Width, _gameOver2D.Width), Color.White, -0.5f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_playSpriteFont, "HIGH SCORE", new Vector2(_ppx, _pdd), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, high_score.ToString(), new Vector2(_ppx, _pdd + 20), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, "SCORE", new Vector2(_ppx, _pdd + 100), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, _score.ToString(), new Vector2(_ppx, _pdd + 100 + 20), Color.Black);

                    spriteBatch.DrawString(_playSpriteFont, "MAX COMBO", new Vector2(_ppx, _pdd + 200), Color.Black);

                    spriteBatch.DrawString(_scoreSpriteFont, max_combo.ToString() + "X", new Vector2(_ppx, _pdd + 200 + 20), Color.Black);

                    break;
                case GameScreen.Help:
                    
                    spriteBatch.Draw(_logo_game, new Vector2(20, 10), new Rectangle(0, 0, _logo_game.Width, _logo_game.Width), Color.White, 0f, Vector2.Zero, 0.125f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_playSpriteFont, "HOW TO PLAY", new Vector2(_x_menu + 100, 30), Color.Black);

                    spriteBatch.DrawString(_GameOverSpriteFont, "The goal is to get the highest score\nby clearing rows of blocks and \nutilizing combos.", new Vector2(_x_menu - 5, _y_menu), Color.Black);

                    spriteBatch.Draw(_help12D, new Vector2(_x_menu - 30, _y_menu+100), new Rectangle(0, 0, _help12D.Width, _help12D.Width), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_GameOverSpriteFont, "To clear blocks, simply drag them to \nmatch 4 or more of the same color \nblocks in a row. You can match \nvertically, horizontally or diagonally. \nNew pieces will be generated on the \nboard after every move.", new Vector2(_x_menu - 5, _y_menu + 250), Color.Black);

                    spriteBatch.DrawString(_GameOverSpriteFont, "MOVE ANYWHERE", new Vector2(_x_menu - 5, _y_menu + 430), Color.Brown);

                    spriteBatch.Draw(_help22D, new Vector2(_x_menu + 120, _y_menu + 470), new Rectangle(0, 0, _help22D.Width, _help22D.Height), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(_GameOverSpriteFont, "UNDO", new Vector2(_x_menu - 5, _y_menu + 560), Color.Green);

                    spriteBatch.Draw(_help32D, new Vector2(_x_menu + 120, _y_menu + 600), new Rectangle(0, 0, _help32D.Width, _help32D.Height), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 1f);

                    break;;
            }

          //  spriteBatch.DrawString(_menusSpriteFont,_buttonIndex.ToString(), new Vector2(0, 0), Color.Black);

            spriteBatch.End();

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
                else if (currentScreen == GameScreen.PlayingScreen ||
                         currentScreen == GameScreen.Achievements ||
                         currentScreen == GameScreen.HighScores ||
                         currentScreen == GameScreen.GameOverScreen||
                    currentScreen==GameScreen.Help)
                {
                    choice_show = false;
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
                GameInProgress = false;
               if (_buttonIndex == 1) //resume
                    {
                        //         load_game(_array, _score, _combo, _move_bt.isSelected, _return_bt.isSelected, _move_count,
                        //_return_count);

                        ResetTouchPanel();
                        currentScreen = GameScreen.PlayingScreen;
                        isNewGame = 1;
                        _array = _array = new int[7, 7];
                        _pressed = false;
                        _combo = (int)load_game(_array)[0, 1];
                        _score = (int)load_game(_array)[0, 0];
                        high_score = load_high_score()[0, 0];
                        max_combo = load_high_score()[0, 1];
                        _next_ball = ballnext();
                        _return_bt.isSelected = (bool)load_game(_array)[0, 3];
                        _move_bt.isSelected = (bool)load_game(_array)[0, 2];
                        _game_over = false;
                        _move_count = (int)load_game(_array)[0, 4];
                        _return_count = (int)load_game(_array)[0, 5];
                        _pre_map = _selected_map = _array;
                        _objects_return = _backup_map(_pre_map, _score, _combo, _move_bt.isSelected,
                                                      _return_bt.isSelected, _move_count, _return_count + 1);
                        save_game(_array, _score, _combo, _move_bt.isSelected, _return_bt.isSelected, _move_count,
                                  _return_count);
                        SaveGameOver(_array);
                    }
                else if (_buttonIndex == 2) //new game
                {
                    if (!choice_show)
                    {
                        choice_show = true;
                        Guide.BeginShowMessageBox("Are you sure ?",
                                                  "Starting a new game will erase your previous game .",
                                                  new string[] {"OK", "Cancel"}, 0,
                                                  MessageBoxIcon.None, new AsyncCallback(OnMessageBoxAction), null);
                    }
                }
                else if (_buttonIndex == 3) //achievements
                {
                    ResetTouchPanel();
                    currentScreen = GameScreen.Achievements;
                }
                else if (_buttonIndex == 4) //high scores
                {
                    ResetTouchPanel();
                    currentScreen = GameScreen.HighScores;
                }
                else if (_help_bt.isSelected)
                {
                    _help_bt.isSelected = false;
                    ResetTouchPanel();
                    currentScreen = GameScreen.Help;
                }
                else
                {
                    ResetTouchPanel();
                    currentScreen = GameScreen.MainMenuScreen;
                }

            }
        }
        void OnMessageBoxAction(IAsyncResult ar)
        {
            int? selectedButton = Guide.EndShowMessageBox(ar);
            switch (selectedButton)
            {
                case 0:
                            ResetTouchPanel();
                            currentScreen = GameScreen.PlayingScreen;
                            isNewGame = 2;
                            _array = CreateArray(7, 7);
                            _pressed = false;
                            _combo = 0;
                            _score = 0;
                            _next_ball = ballnext();
                            _return_bt.isSelected = false;
                            _move_bt.isSelected = false;
                            high_score = load_high_score()[0, 0];
                            max_combo = load_high_score()[0, 1];
                            _game_over = false;
                            _move_count = 6;
                            _return_count = 9;
                            _pre_map = _selected_map = _array;
                            _objects_return = _backup_map(_pre_map, _score, _combo, _move_bt.isSelected,
                                                          _return_bt.isSelected, _move_count, _return_count + 1);
                            save_game(_array, _score, _combo, _move_bt.isSelected, _return_bt.isSelected, _move_count,
                                      _return_count);
                            SaveGameOver(_array);

                    break;

                case 1:

                default:
                            _buttonIndex = 0;
                            choice_show = false;
                    break;
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
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;

                case GameScreen.GameWelCome:
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;
                case GameScreen.GameOverScreen:
                    TouchPanel.EnabledGestures = GestureType.None;
                    break;
                case GameScreen.Help:
                    TouchPanel.EnabledGestures=GestureType.None;
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
        public int random_ball_next(int[,] Array, int[] _nextball)
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
                Array[listpoint[temp].X, listpoint[temp].Y] = _nextball[i];//rand.Next(1,6);             
            }
            return 1;
        }

        private int[] ballnext()
        {
            int[] ballnext = new int[3];

            Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                ballnext[i] = rand.Next(1, 6);
            }
            return ballnext;
        }
        #endregion

        #region Save game
        private void save_game(int[,] _arr, int _score_b, int _comboX, bool _mv, bool _re, int _move_c, int _re_c)
        {
            StreamWriter writeFile;

            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            string _content;

            string _fileName;

            try
            {
                _fileName = "saveandload";
                if (!isolatedFile.DirectoryExists("Documents"))
                {
                    isolatedFile.CreateDirectory("Documents");
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("Documents\\" + _fileName + ".txt", FileMode.CreateNew, isolatedFile));
                }
                else
                {
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("Documents\\" + _fileName + ".txt", FileMode.Open, isolatedFile));
                }

                //score
                _content = _score_b.ToString();
                writeFile.WriteLine(_content);
                //combo X
                _content = _comboX.ToString();
                writeFile.WriteLine(_content);
                //move button state
                _content = _mv.ToString();
                writeFile.WriteLine(_content);
                //return button state
                _content = _re.ToString();
                writeFile.WriteLine(_content);
                //move button count
                _content = _move_c.ToString();
                writeFile.WriteLine(_content);
                //return button count
                _content = _re_c.ToString();
                writeFile.WriteLine(_content);

                //Array
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        _content = _arr[i, j].ToString();
                        writeFile.WriteLine(_content);
                        _content = "";
                    }

                }
             
                writeFile.Close();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region Load Game

        private object[,] load_game(int[,] _arr)
        {

            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            string _fileName;

            object[,] loadinfo=new object[1,6];

            //score
            loadinfo[0, 0] = 0;
            //combo
            loadinfo[0, 1] = 0;
            //move
            loadinfo[0, 2] = false;
            //return
            loadinfo[0, 3] = false;
            //move count
            loadinfo[0, 4] = 6;
            //return count
            loadinfo[0, 5] = 9;

                try
                {
                    _fileName = "saveandload";
                    if (isolatedFile.DirectoryExists("Documents"))
                    {
                        IsolatedStorageFileStream fileStream = isolatedFile.OpenFile("Documents\\" + _fileName + ".txt",
                                                                                     FileMode.Open, FileAccess.Read);
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            //score
                            loadinfo[0, 0] = int.Parse(reader.ReadLine());
                            //combo
                            loadinfo[0, 1] = int.Parse(reader.ReadLine());
                            //move
                            loadinfo[0, 2] = bool.Parse(reader.ReadLine());
                            //return
                            loadinfo[0, 3] = bool.Parse(reader.ReadLine());
                            //move count
                            loadinfo[0, 4] = int.Parse(reader.ReadLine());
                            //return count
                            loadinfo[0, 5] = int.Parse(reader.ReadLine());

                            for (int i = 0; i < 7; i++)
                            {
                                for (int j = 0; j < 7; j++)
                                {
                                    _arr[i, j] = int.Parse(reader.ReadLine());
                                }
                            }

                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            return loadinfo;
        }

        #endregion

        #region Backup_Map
        private object[,] _backup_map(int[,] _arr, int _scr, int _comb, bool mv, bool retu, int mv_c, int retu_c)
        {
            object[,] Array_back = new object[8, 14];

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Array_back[i, j] = _arr[i, j];
                }
            }
            Array_back[7, 8] = _scr;
            Array_back[7, 9] = _comb;
            Array_back[7, 10] = mv;
            Array_back[7, 11] = retu;
            Array_back[7, 12] = mv_c;
            Array_back[7, 13] = retu_c;

            return Array_back;
        }
        #endregion

        #region Return Map

        private void _return_map(object[,] _arr)
        {

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    _array[i, j] = (int)_arr[i, j];
                }
            }

            _score = (int)_arr[7, 8];
            _combo = (int)_arr[7, 9];
            _move_bt.isSelected = (bool)_arr[7, 10];
            _return_bt.isSelected = (bool)_arr[7, 11];
            _move_count = (int)_arr[7, 12];
            _return_count = (int)_arr[7, 13];
        }

        #endregion

        #region Compare Two Array

        public bool ArraysEqual(int[,] a1, int[,] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (a1[i, j] != a2[i, j])
                        return false;
                }
            return true;
        }

        #endregion

        #region Game The End
        public int the_end(int[,] arr)
        {
            int len = 0;
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    if (arr[i, j] == 0)
                        len++;
            return len;
        }
        #endregion

        #region High Score

        private void save_highscore(int _score_h,int _combox_h, int _high_score_h)
        {
            StreamWriter writeFile;

            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            string _content;

            string _fileName="highscore";
            
            try
            {
               
                if (!isolatedFile.DirectoryExists("HighScore"))
                {
                    isolatedFile.CreateDirectory("HighScore");
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("HighScore\\" + _fileName + ".txt", FileMode.CreateNew, isolatedFile));
                }
                else
                {
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("HighScore\\" + _fileName + ".txt", FileMode.Open, isolatedFile));
                }

                //score
                if (_high_score_h < _score_h)
                {
                    _content = _score_h.ToString();
                    writeFile.WriteLine(_content);
                }
                else
                {
                    writeFile.WriteLine(_high_score_h.ToString());
                }
                //combo X
                _content = _combox_h.ToString();
                writeFile.WriteLine(_content);  

                writeFile.Close();
                writeFile.Dispose();
            }
            catch (Exception ex)
            {

            }

        }

        private int[,] load_high_score()
        {
            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            int[,] _highscore_load = new int[1, 2];

            int[,] _ex_highscore = new int[1, 2];

            _ex_highscore[0, 0] = 0;

            _ex_highscore[0, 1] = 0;

            string _fileName = "highscore";

            try
            {
                if (isolatedFile.DirectoryExists("HighScore"))
                {

                    IsolatedStorageFileStream fileStream = isolatedFile.OpenFile("HighScore\\" + _fileName + ".txt",
                                                                                 FileMode.Open, FileAccess.Read);

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        //score
                        _highscore_load[0, 0] = int.Parse(reader.ReadLine());
                        //combo
                        _highscore_load[0, 1] = int.Parse(reader.ReadLine());

                        reader.Close();
                        reader.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                return _ex_highscore;
            }
            return _highscore_load;
        }

        #endregion

        #region Game Over

        private void SaveGameOver(int[,] _arr)
        {
            StreamWriter writeFile;

            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            string _content;

            string _fileName = "GameOver";

            if (the_end(_arr) <= 0)
            {
                _content = "true";
            }
            else
            {
                _content = "false";
            }

            try
            {

                if (!isolatedFile.DirectoryExists("GameOver"))
                {
                    isolatedFile.CreateDirectory("GameOver");
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("GameOver\\" + _fileName + ".txt", FileMode.CreateNew, isolatedFile));
                }
                else
                {
                    writeFile = new StreamWriter(new IsolatedStorageFileStream("GameOver\\" + _fileName + ".txt", FileMode.Open, isolatedFile));
                }

                writeFile.WriteLine(_content);
                writeFile.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private bool LoadGameOver()
        {

            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            string readoneline;

            string _fileName;

            try
            {
                _fileName = "GameOver";
                if (isolatedFile.DirectoryExists(_fileName))
                {
                    IsolatedStorageFileStream fileStream = isolatedFile.OpenFile("GameOver\\" + _fileName + ".txt",
                                                                                 FileMode.Open, FileAccess.Read);
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        readoneline = reader.ReadLine();
                        reader.Close();
                        return bool.Parse(readoneline);
                    }
                }
            }
            catch (Exception ex)
            {
                return true;
            }

            return true;
        }

        #endregion

        #region CheckEmpty

        private bool Empty(int[,] _arr)
        {

            for(int i=0;i<7;i++)
                for (int j = 0; j < 7; j++)
                {
                    if (_arr[i, j] != 0)
                    {
                        return false;
                    }
                }

            return true;
        }

        #endregion

        private void ResetTouchPanel()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

    }
}
