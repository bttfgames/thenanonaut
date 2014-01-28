using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Nano
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Level
    {
        ContentManager content;

        Player player;
        Background background;
        Heart heart;

        Texture2D scoreHud;

        //random
        Random rnd;

        //Debug
        SpriteFont debugFont;
        SpriteFont scoreFont;
        SpriteFont scoreNumber;
        string debug1;
        string debug2;

        //Listas de objetos
        List<Shoot> Shoot_List = new List<Shoot>();
        List<Virus> Virus_List = new List<Virus>();
        List<Cholesterol> Chol_List = new List<Cholesterol>();
        List<Explosion> Explosion_List = new List<Explosion>();
        Boss boss;

        //sons
        SoundEffect shot;
        SoundEffect hitVirus;
        SoundEffect hitChol;
        SoundEffect deadChol;
        Song theme;
        Song bossTheme;

        public bool gameOver = false; 

        public Level()
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization logic here
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager _content)
        {
            content = _content;
            rnd = new Random();
            boss = new Boss();
            player = new Player();
            background = new Background();
            heart = new Heart();
            player.Initialize(content, new Vector2(150, 240), GameVars.screenWidth, GameVars.screenHeigth);
            background.Initialize(content, Vector2.Zero);
            heart.Initialize(content);
            debugFont = content.Load<SpriteFont>("gamefont");
            scoreFont = content.Load<SpriteFont>("scorefont");
            scoreNumber = content.Load<SpriteFont>("scorenumber");
            scoreHud = content.Load<Texture2D>("hud");
            shot = content.Load<SoundEffect>("sound\\shot");
            hitVirus = content.Load<SoundEffect>("sound\\hit_virus");
            hitChol = content.Load<SoundEffect>("sound\\chol_hit");
            deadChol = content.Load<SoundEffect>("sound\\chol_dead");
            theme = content.Load<Song>("sound\\theme");
            bossTheme = content.Load<Song>("sound\\boss");

            boss.Initialize(content, new Vector2(GameVars.screenWidth, GameVars.screenHeigth /2));

            if (!GameVars.musicMute)
            {
                MediaPlayer.Volume = (float)GameVars.musicVolume / 10f;
                MediaPlayer.Play(theme);
                MediaPlayer.IsRepeating = true;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Joystick joystick)
        {
            debug1 = joystick.GetDirJoy().ToString();

            //Ver se atira
            if (joystick.fire)
            {
               CreateShoot();
               shot.Play();
            }

            //Cria virus
            if (Virus_List.Count < GameVars.quantVirus)
            {
                CreateVirus();
            }

            if (Chol_List.Count < GameVars.quantCho)
                CreateCholesterol();

            background.Update(gameTime, new Vector2(-1, 0));
            heart.Update(gameTime);

            if (-background.position.X >= background.lengthScene - GameVars.screenWidth*2)
            {
                if (!GameVars.isBoss)
                    MediaPlayer.Play(bossTheme);
                GameVars.isBoss = true;
                boss.Update(gameTime,new Vector2(background.lengthScene + background.position.X - boss.collisionRect.Width/2 + 30, GameVars.screenHeigth - boss.collisionRect.Height/2));
                if (boss.life <= 0)
                    GameVars.bossIsDead = true;
            }
                
            
            if (heart.isDead)
                gameOver = true;

            //Atualiza listas
            for (int i = 0; i < Shoot_List.Count; i++) Shoot_List[i].Update(gameTime);
            foreach (Virus virus in Virus_List) virus.Update(gameTime, background.GetMinTop(virus.collisionRect), background.GetMaxBottom(virus.collisionRect));
            foreach (Cholesterol chol in Chol_List) chol.Update(gameTime, background.GetMinTop(chol.collisionRect), background.GetMaxBottom(chol.collisionRect));
            foreach (Explosion explosion in Explosion_List) explosion.Update(gameTime);

            player.Update(gameTime, background.GetMinTop(player.collisionRect), background.GetMaxBottom(player.collisionRect), joystick.GetDirJoy());

            //Limpa as listas
            CleanList();

            //Colisoes
            Collisions();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {

            //monster.Draw(spriteBatch);
            //hero.Draw(spriteBatch);
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            heart.Draw(spriteBatch);

            //Desenha listas
            foreach (Shoot shoot in Shoot_List) shoot.Draw(spriteBatch);
            foreach(Virus virus in Virus_List) virus.Draw(spriteBatch);
            foreach (Cholesterol chol in Chol_List) chol.Draw(spriteBatch);
            foreach (Explosion explosion in Explosion_List) explosion.Draw(spriteBatch);

            if (background.position.X <= -13000)
            {
                boss.Draw(spriteBatch);
            }

            //Para Debug
            debug1 = Chol_List.Count.ToString();
            if (Chol_List.Count>0) debug2 = Chol_List[0].position.ToString();
            //spriteBatch.DrawString(debugFont, debug2 + " " + debug1, new Vector2(10, 400), Color.White);

            //spriteBatch.DrawString(debugFont, "pos: "+background.position.X, new Vector2(10, 400), Color.White);
            
            // Score ate ir para classe HUD
            //spriteBatch.DrawString(scoreFont, GameVars.energy.ToString(), new Vector2(10, 10), Color.Yellow);
            //hud score
            spriteBatch.Draw(scoreHud, new Vector2(GameVars.screenWidth - scoreHud.Width, 20), Color.White);
            spriteBatch.DrawString(scoreFont, "SCORE", 
                                   new Vector2(GameVars.screenWidth - 250, 30), Color.Yellow);

            spriteBatch.DrawString(scoreNumber, GameVars.score.ToString(),
                       new Vector2(GameVars.screenWidth - 90, 20), Color.Yellow);

        }

#region Colisoes

        public void Collisions()
        {

            //tiros com terreno
            for (int i = 0; i < Shoot_List.Count; i++)
            {
                if ((Shoot_List[i].collisionRect.Top <= background.GetMinTop(Shoot_List[i].collisionRect)) ||
                    (Shoot_List[i].collisionRect.Bottom >= background.GetMaxBottom(Shoot_List[i].collisionRect)))
                {
                    GameVars.energy -= 5;
                    Shoot_List.RemoveAt(i);
                    VibrateController.Default.Start(new TimeSpan(0, 0, 0, 0, 500));
                    background.isShaking = true;
                    deadChol.Play((float)GameVars.soundVolume / 10f, 0f, 0f); // Trocar o som
                    break;
                }
            }

            //tiros com virus
            for (int i = 0; i < Shoot_List.Count; i++)
            {
                for (int v = 0; v < Virus_List.Count; v++)
                {
                    if (Shoot_List[i].collisionRect.Intersects(Virus_List[v].collisionRect))
                    {
                        Shoot_List.RemoveAt(i);
                        hitVirus.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                        CreateExplosion(Virus_List[v].position, 1);
                        Virus_List.RemoveAt(v);
                        if(( GameVars.energy < 100)&&(!GameVars.isBoss))
                            GameVars.energy += 5;
                        GameVars.score += GameVars.scoreVirus;
                        break;
                    }
                }

            }

            //Colisao com Colesterol
            for (int i = 0; i < Shoot_List.Count; i++)
            {
                for (int v = 0; v < Chol_List.Count; v++)
                {
                    if (Shoot_List[i].collisionRect.Intersects(Chol_List[v].collisionRect))
                    {
                        Chol_List[v].life--;
                        if (Chol_List[v].life == 0)
                        {
                            deadChol.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                            CreateExplosion(Chol_List[v].position, 2);
                            Chol_List.RemoveAt(v);
                            if ((GameVars.energy < 100) && (!GameVars.isBoss))
                                GameVars.energy += 5;
                            break;
                        }
                        else {
                            hitChol.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                            Chol_List[v].blink();
                            //CreateExplosion(Chol_List[v].position, 2);
                        }
                        Shoot_List.RemoveAt(i);
                        

                        GameVars.score += GameVars.scoreChol;
                        break;
                    }
                }
            }

            //Player com virus
            for (int v = 0; v < Virus_List.Count; v++)
            {
                if (player.collisionRect.Intersects(Virus_List[v].collisionRect))
                {
                    player.blink();
                    hitVirus.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                    GameVars.energy -= Virus_List[v].damage;
                    CreateExplosion(Virus_List[v].position, 1);
                    Virus_List.RemoveAt(v);
                    VibrateController.Default.Start(new TimeSpan(0, 0, 0, 0, 500));
                    break;
                }
            }

            //Player com Chol
            for (int v = 0; v < Chol_List.Count; v++)
            {
                if (player.collisionRect.Intersects(Chol_List[v].collisionRect))
                {
                    player.blink();
                    hitChol.Play((float)GameVars.soundVolume / 10f,0f,0f);
                    GameVars.energy -= Chol_List[v].damage;
                    CreateExplosion(Virus_List[v].position, 2);
                    Chol_List.RemoveAt(v);
                    VibrateController.Default.Start(new TimeSpan(0, 0, 0, 0, 500));
                    break;
                }
            }

            //tiros com boss
            for (int i = 0; i < Shoot_List.Count; i++)
            {
                if (Shoot_List[i].collisionRect.Intersects(boss.collisionRect))
                {
                    Shoot_List.RemoveAt(i);
                    deadChol.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                    boss.life -= 1;
                    break;
                }
            }


        }

#endregion

#region Cria Objetos

        public void CreateShoot()
        {
            Shoot shoot = new Shoot();
            shoot.Initialize(content, player.position + new Vector2(50,28));
            Shoot_List.Add(shoot);
            //soundShoot.Play((float)GameState.soundVolume / 10f, 0, 0);
        }

        public void CreateVirus()
        {
            Virus virus = new Virus();
            virus.Initialize(content, new Vector2((float)(GameVars.screenWidth + rnd.NextDouble()*GameVars.screenWidth), GameVars.screenHeigth / 2));
            Virus_List.Add(virus);
            //soundShoot.Play((float)GameState.soundVolume / 10f, 0, 0);
        }


        public void CreateCholesterol()
        {
            Cholesterol chol = new Cholesterol();
            chol.Initialize(content, new Vector2((float)(GameVars.screenWidth + rnd.NextDouble() * GameVars.screenWidth), GameVars.screenHeigth / 2));
            Chol_List.Add(chol);
        }

        public void CreateExplosion(Vector2 _position, int tipo)
        {
            Explosion explosion = new Explosion();
            explosion.Initialize(content, _position, tipo);
            Explosion_List.Add(explosion);
        }

#endregion

        void CleanList()
        {
            //Limpa os tiros
            for (int i = 0; i < Shoot_List.Count; i++)
            {
                if (Shoot_List[i].position.X > Shoot_List[i].collisionRect.Width + GameVars.screenWidth)
                {
                    Shoot_List.RemoveAt(i);
                    break;
                }
            }

            //Limpa os virus
            for (int i = 0; i < Virus_List.Count; i++)
            {
                if (Virus_List[i].position.X < -Virus_List[i].collisionRect.Width)
                {
                    Virus_List.RemoveAt(i);
                    break;
                }
            }

            //limpa o colestesterol
            for (int i = 0; i < Chol_List.Count; i++)
            {
                if (Chol_List[i].position.X < - Chol_List[i].collisionRect.Width)
                {
                    Chol_List.RemoveAt(i);
                    break;
                }
            }

            //Limpa as explosoes inativas
            for (int i = 0; i < Explosion_List.Count; i++)
            {
                if (!Explosion_List[i].active)
                {
                    Explosion_List.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
