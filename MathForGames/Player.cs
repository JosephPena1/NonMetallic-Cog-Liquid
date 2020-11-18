﻿using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    /// <summary>
    /// An actor that moves based on input given by the user
    /// </summary>
    class Player : Actor
    {
        enum Controls
        {
            MOUSE,
            WASD
        }

        private float _speed = 1;
        private float rotation;
        private Controls _controls = Controls.WASD;
        private float _scaleX = 1;
        private float _scaleY = 1;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private Sprite _sprite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Player(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {
            _sprite = new Sprite("Images/player.png");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("Images/player.png");
        }

        public override void Start()
        {
            GameManager.onWin += DrawWinText;

            base.Start();
        }

        public override void Update(float deltaTime)
        {
            //changes controls based on number. 1 = mouse, 2 = WASD.
            //int controls = 2;

            float xDirection = 0;
            float yDirection = 0;

            switch (_controls)
            {
                case Controls.MOUSE:

                    if (Raylib.GetMousePosition().X >= 512 && Raylib.GetMousePosition().Y >= 380)
                    {
                        xDirection = Raylib.GetMousePosition().X;
                        yDirection = Raylib.GetMousePosition().Y;
                    }
                    else if (Raylib.GetMousePosition().X < 512 && Raylib.GetMousePosition().Y >= 380)
                    {
                        xDirection = -Raylib.GetMousePosition().Y;
                        yDirection = Raylib.GetMousePosition().X;
                    }
                    else if (Raylib.GetMousePosition().X >= 512 && Raylib.GetMousePosition().Y < 380)
                    {
                        xDirection = Raylib.GetMousePosition().X;
                        yDirection = -Raylib.GetMousePosition().Y;
                    }
                    else
                    {
                        xDirection = -Raylib.GetMousePosition().Y;
                        yDirection = -Raylib.GetMousePosition().X;
                    }
                    break;

                case Controls.WASD:

                    xDirection = -Convert.ToInt32(Engine.GetKeyDown((int)KeyboardKey.KEY_A))
                        + Convert.ToInt32(Engine.GetKeyDown((int)KeyboardKey.KEY_D));
                    yDirection = -Convert.ToInt32(Engine.GetKeyDown((int)KeyboardKey.KEY_W))
                        + Convert.ToInt32(Engine.GetKeyDown((int)KeyboardKey.KEY_S));
                    break;
                
            }

            if (Engine.GetKeyPressed((int)KeyboardKey.KEY_UP))
            {
                _scaleX += 1;
                _scaleY += 1;
            }
            else if (Engine.GetKeyPressed((int)KeyboardKey.KEY_DOWN))
            {
                _scaleX -= 1;
                _scaleY -= 1;
            }
            SetScale(_scaleX, _scaleY);

            //Set the actors current velocity to be the vector with the direction found scaled by the speed
            Velocity = new Vector2(xDirection, yDirection);
            Velocity = Velocity.Normalized * Speed;

            SetRotation(_rotateCounter);
            _rotateCounter += 0.05f;

            CheckCollision(_collisionTarget);

            base.Update(deltaTime);
        }

        public override void OnCollision(Actor actor)
        {
            if (actor is Enemy && _seconds > 5)
                Engine.SetGameOver(true);

            base.OnCollision(actor);
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        public void DrawWinText()
        {
            Raylib.DrawText("You win. \n Press esc to quit", 0, 0, 5, Color.GREEN);
        }
    }
}