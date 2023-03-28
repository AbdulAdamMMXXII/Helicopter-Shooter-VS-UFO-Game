using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelicopterShooterGame
{
    public partial class Form1 : Form
    {
        // flags for helicopter movements, shooting and game over 
        bool goUp, goDown, shot, gameOver;

        int score = 0; // player score
        int speed = 8; // speed of pillars moving
        int UFOspeed = 10; // speed of UFO

        Random rand = new Random(); // random number generator for UFO placement

        int playerSpeed = 7; // speed of player movement
        int index = 0; // index to cycle through UFO images



        public Form1()
        {
            InitializeComponent();
        }
        // main timer event for the game
        private void MainTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            // helicopter movements
            if (goUp == true && player.Top > 0)
            {
                player.Top -= playerSpeed;
            }
            if(goDown == true && player.Height < this.ClientSize.Height)
            {
                player.Top += playerSpeed;
            }

            // move the UFO and change it when it goes off screen
            ufo.Left -= UFOspeed;
            if(ufo.Left + ufo.Width <0)
            {
                ChangeUFO();
            }

            // repeat through controls to handle pillars and bullets
            foreach (Control x in this.Controls)
            {
                // handle pillars
                if (x is PictureBox && (string)x.Tag == "pillar")
                {
                     x.Left -= speed;

                    // wrap pillars when they go off screen
                    if (x.Left < -200)
                    {
                        x.Left = 1000;
                    }

                    // if helicopter hits the pillar, end the game
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        GameOver();
                    }
                }

                // handle bullets
                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Left += 25;
                    if(x.Left > 900)
                    {
                        RemoveBullet(((PictureBox)x));
                    }

                    // if bullet hits UFO, remove the bullet and add score
                    if (ufo.Bounds.IntersectsWith(x.Bounds))
                    {
                        RemoveBullet(((PictureBox)x));
                        score += 1;
                        ChangeUFO();
                    }
                }
            }

            // if helicopter hits UFO, end the game
            if (player.Bounds.IntersectsWith(ufo.Bounds))
            {
                GameOver();
            }

            // make the game more difficult as the score increases
            //Level 2 speed
            if (score > 3)
            {
                speed = 12;
                UFOspeed = 18;
            }

            //Level 3 speed
            if (score > 6)
            {
                speed = 18;
                UFOspeed = 25;
            }

            //Level 4
            if (score > 12)
            {
                speed = 25;
                UFOspeed = 30;
            }
            if (score > 16)
            {
                GameTimer.Stop();
                Console.BackgroundColor = ConsoleColor.Red;
                MessageBox.Show(txtScore.Text = " You won! " + "Your score: " + score + " " + "\n Press entre to play again.");
                gameOver = true;
            }
        }

        // handle key down events for helicopter movements and shooting
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }

            if(e.KeyCode == Keys.Space && shot == false)
            {
                MakeBullet();
                shot = true;
            }
        }

        // handle key up events for helicopter movements and shooting
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if(e.KeyCode == Keys.Down)
            {
                goDown = false;
            } 
            if(shot == true)
            {
                shot = false;
            }

            // if game is over, restart it with the "Enter" key
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {

            goUp = false;
            goDown = false;
            shot = false;
            gameOver = false;
            score = 0;
            speed = 8;
            UFOspeed = 10;
            txtScore.Text = "Score: " + score;
            ChangeUFO();
            //Relocate halicapter to position x
            player.Top = 119;
            //Relocate colon1 to position x
            pillar1.Left = 579;
            //Relocate colon to position x
            pillar2.Left = 253;

            GameTimer.Start();
        }

        private void GameOver()
        {
            GameTimer.Stop();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            MessageBox.Show("Game over! Score: " + score + "\nPress Enter to try again.");
            gameOver = true;
        }

        private void RemoveBullet(PictureBox bullet)
        {
            this.Controls.Remove(bullet);
            bullet.Dispose();
        }

        private void MakeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.BackColor = Color.Maroon;
            bullet.Height = 5;
            bullet.Width = 10;

            bullet.Left = player.Left + player.Width;
            bullet.Top = player.Top + player.Height / 2;

            bullet.Tag = "bullet";
            this.Controls.Add(bullet);
        }

        private void ChangeUFO()
        {
            if(score > 3)
            {
                index = 1;
            }
            else
            {
                index += 1;
            }

            switch (index)
            {
                case 1:
                    ufo.Image = Properties.Resources.alien1;
                    break;
                    case 2:
                    ufo.Image = Properties.Resources.alien2;
                    break;
                    case 3:
                    ufo.Image = Properties.Resources.alien3;
                    break;
            }
            ufo.Left = 1000;
            ufo.Top = rand.Next(20, this.ClientSize.Height - ufo.Height);
        }
    }
}
