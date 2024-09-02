using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Violet
{
    // make it so when previous turn raised, everyone must match to continue playing
    // easy for ai, need a way to get player to play again if an ai raised (also get the ais that played before the raise to play again.)
    // then go to next round
    // Maybe use arrays? similar to hash map, look at
    public class Poker : Game
    {
        private enum GameState
        {
            mainMenu,
            PlayOptions,
            Game,
            AiTurn,
            Betting,
            endGame,
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Deck deck;
        private List<Card> dealerHand;
        private List<int> TakenNames;
        private Player player;
        private List<AIPlayer> Ais;
        private PokerRules rules;
        private SpriteFont font;
        private Random rnd = new Random();
        private Button play;
        private Button quit;
        private GameState State;
        private int Bet, BetAmount, index, numAi, currentAi, amountRaised;
        private int[] PossibleBids = { 1, 5, 25, 100 };
        private string[] Names;
        private string path = "C:\\Users\\isaac\\OneDrive\\Desktop\\Build\\Violet\\Violet\\Names.txt";
        
        public Poker()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = Globals.windowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.windowSize.Y;
            _graphics.ApplyChanges();
            Globals.Content = Content;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.spriteBatch = _spriteBatch;

            // TODO: use this.Content to load your game content here
            play = new Button(Content.Load<Texture2D>("Buttons/Play"), new Vector2(500, 100));
            quit = new Button(Content.Load<Texture2D>("Buttons/Quit"), new Vector2(500, 300));
            font = Content.Load<SpriteFont>("Results");
            deck = new Deck(new Vector2(500, 300));
            deck.Shuffle();
            dealerHand = new List<Card>();
            Ais = new List<AIPlayer>();
            rules = new PokerRules(); 
            TakenNames = new List<int>();
            amountRaised = 0;

            // Build the full path to the file using Path.Combine
            //string fullPath = Path.Combine(Environment.CurrentDirectory, path);

            if (File.Exists(path))
            {
                Names = new string[59];
                Names = File.ReadAllLines(path); 
            }

            else
            {
                // Handle the case where the file doesn't exist
                Debug.WriteLine("File not found: " + path);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch(State)
            {
                case GameState.mainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.PlayOptions:
                    UpdatePlayOptions(gameTime);
                    break;
                case GameState.Game:
                    UpdateGame(gameTime); 
                    break;
                case GameState.AiTurn:
                    UpdateAiTurn(gameTime);
                    break;
                case GameState.Betting:
                    UpdateBetting(gameTime);
                    break;
                case GameState.endGame:
                    UpdateGameEnd(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            switch (State)
            {
                case GameState.mainMenu:
                    DrawMainMenu(gameTime); 
                    break;
                case GameState.PlayOptions:
                    DrawPlayOptions(gameTime);
                    break;
                case GameState.Game:
                    DrawGame(gameTime);
                    break;
                case GameState.AiTurn:
                    break;
                case GameState.Betting:
                    DrawGame(gameTime);
                    DrawBetting(gameTime);
                    break;
                case GameState.endGame:
                    DrawGame(gameTime);
                    DrawGameEnd(gameTime);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            deck = new Deck(new Vector2(500, 300));
            deck.Shuffle();
            dealerHand = new List<Card>();
            player.hand = new Card[2];
            rules.DealerHand = new List<Card>();
            rules.round = 0;
            if (numAi == 0)
            {
                player.hand[0] = deck.DrawCard();
                player.hand[1] = deck.DrawCard();
                player.SetHighCard();

                if (player.hand[0] != null)
                    player.hand[0].Position = new Vector2(100, 450);

                if (player.hand[1] != null)
                    player.hand[1].Position = new Vector2(300, 450);
            }

            else
            {
                for(int i = 0; i < numAi; i++)
                {
                    Ais[i].Hand = new Card[2];
                }

                for(int i = 0; i < 2; i++)
                {
                    player.hand[i] = deck.DrawCard();
                    for (int j = 0; j < numAi; j++)
                    {
                        Ais[j].Hand[i] = deck.DrawCard();
                    }
                }

                if (player.hand[0] != null)
                    player.hand[0].Position = new Vector2(100, 450);

                if (player.hand[1] != null)
                    player.hand[1].Position = new Vector2(300, 450);
            }
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            InputManager.Update();
            play.Update();
            quit.Update();
            if(play.rectangle.Contains(InputManager.MousePosition) && InputManager.LeftClick)
                    State = GameState.PlayOptions;

            if (quit.rectangle.Contains(InputManager.MousePosition) && InputManager.LeftClick)
                Exit();
        }
        private void UpdatePlayOptions(GameTime gameTime) 
        {
            InputManager.Update();
            if(InputManager.RightPressed)
            {
                if(numAi < 3)
                    numAi++;
            }

            if(InputManager.LeftPressed)
            {
                if(numAi > 0)
                    numAi--;
            }

            if(InputManager.SpacePressed)
            {
                LoadPlayers(gameTime);
                State = GameState.Game;
            }
        }

        private void UpdateGameEnd(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ResetGame();
                State = GameState.Game;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                ResetGame();
                State = GameState.mainMenu;
            }
        }

        private void UpdateGame(GameTime gameTime)
        { 
            InputManager.Update();

            if (player.Folded == false)
            {
                if (InputManager.CPressed)
                {
                    if (numAi > 0)
                    {
                        currentAi = 0;
                        State = GameState.AiTurn;
                    }

                    else
                    {
                        rules.UpdateDealer(dealerHand, deck);
                    }
                }

                if (InputManager.RPressed)
                {
                    if (numAi > 0)
                    {
                        Bet = PossibleBids[0];
                        BetAmount = 0;
                        index = 0;
                        State = GameState.Betting;
                    }
                }

                if (InputManager.FPressed)
                {
                    player.Folded = true;
                    State = GameState.AiTurn;
                }

                if (rules.round == 3)
                {
                    if (numAi > 0)
                    {
                        rules.GetDealerHand(dealerHand);
                        rules.setScore(player);
                        foreach (AIPlayer Ai in Ais)
                        {
                            rules.setScore(Ai);
                        }
                        State = GameState.endGame;
                    }

                    else
                    {
                        rules.GetDealerHand(dealerHand);
                        rules.setScore(player);
                        State = GameState.endGame;
                    }
                }
            }

            else
            {
                if (rules.round == 3)
                {
                    if (numAi > 0)
                    {
                        rules.GetDealerHand(dealerHand);
                        rules.setScore(player);
                        foreach (AIPlayer Ai in Ais)
                        {
                            rules.setScore(Ai);
                        }
                        State = GameState.endGame;
                    }

                    else
                    {
                        rules.GetDealerHand(dealerHand);
                        rules.setScore(player);
                        State = GameState.endGame;
                    }
                }

                else
                {
                    State = GameState.AiTurn;
                }
            }
        }
        //here
        private void UpdateAiTurn(GameTime gameTime)
        {
            if (amountRaised != 0)
            {
                rules.GetDealerHand(dealerHand);
                rules.setScore(Ais[currentAi]);
                if (Ais[currentAi].Score > 4 && Ais[currentAi].BiddingAmount >= amountRaised)
                {
                    rules.BidtoPot(20 + amountRaised, Ais[currentAi]);
                    amountRaised += 20;

                    player.Raised = false;
                    foreach (AIPlayer Ai in Ais)
                    {
                        Ai.Raised = false;
                    }
                    player.Raised = false;
                    Ais[currentAi].Raised = true;
                    currentAi++;

                    if (currentAi == Ais.Count)
                    {
                        State = GameState.Game;
                    }
                }

                else if (Ais[currentAi].Score > 1 && Ais[currentAi].BiddingAmount >= amountRaised)
                {
                    rules.BidtoPot(amountRaised, Ais[currentAi]);
                    Ais[currentAi].Raised = true;
                    currentAi++;

                    if (currentAi == Ais.Count && player.Raised == true)
                    {
                        rules.UpdateDealer(dealerHand, deck);
                        amountRaised = 0;
                        State = GameState.Game;
                    }
                }

                else
                {
                    Ais[currentAi].Folded = true;
                    currentAi++;

                    if (currentAi == Ais.Count)
                    {
                        rules.UpdateDealer(dealerHand, deck);
                        State = GameState.Game;
                    }
                }

            }

            else
            {
                if (rules.round == 0)
                {
                    currentAi++;
                    if (currentAi == Ais.Count)
                    {
                        rules.UpdateDealer(dealerHand, deck);
                        State = GameState.Game;
                    }
                }

                else if (rules.round == 1 || rules.round == 2)
                {
                    rules.GetDealerHand(dealerHand);
                    rules.setScore(Ais[currentAi]);
                    if (Ais[currentAi].Score > 1 && Ais[currentAi].BiddingAmount > 0)
                    {
                        BetAmount = rnd.Next(1, Ais[currentAi].BiddingAmount + 1);
                        rules.BidtoPot(BetAmount, Ais[currentAi]);
                    }
                    currentAi++;
                    if (currentAi == Ais.Count)
                    {
                        rules.UpdateDealer(dealerHand, deck);
                        State = GameState.Game;
                    }
                }
            }
        }

        private void UpdateBetting(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.UpPressed && (BetAmount + Bet) <= player.BiddingAmount)
            {
                BetAmount += Bet;
            }

            if (InputManager.DownPressed && (BetAmount - Bet) >= 0)
            {
                BetAmount -= Bet;
            }

            if (InputManager.RightPressed)
            {
                index = (index + 1) % PossibleBids.Length;
                Bet = PossibleBids[index];
            }

            if (InputManager.LeftPressed)
            {
                index--;
                if(index < 0)
                {
                    index = PossibleBids.Length - 1;
                }
                Bet = PossibleBids[index];
            }

            if (InputManager.SpacePressed)
            {
                rules.BidtoPot(BetAmount, player);
                rules.UpdateDealer(dealerHand, deck);
                amountRaised = BetAmount;
                currentAi = 0;
                State = GameState.AiTurn;
            }
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            play.DrawScaled();
            quit.DrawScaled();
        }

        private void DrawPlayOptions(GameTime gameTime)
        {
            _spriteBatch.DrawString(font, "How many bots at the table?", new Vector2(250, 50), Color.Black);
            _spriteBatch.DrawString(font, "Bots: " + numAi, new Vector2(250, 100), Color.Black);
        }

        private void DrawGame(GameTime gameTime)
        {
            player.hand[0].Draw();
            player.hand[1].Draw();

            if (Ais != null)
            {
                foreach (AIPlayer Ai in Ais)
                {
                    _spriteBatch.DrawString(font, "Name: " + Ai.Name, new Vector2(500, 250), Color.Black);
                }
            }
            deck.Draw();
            _spriteBatch.DrawString(font, "Remaining Funds: " + player.BiddingAmount, new Vector2(500, 500), Color.Black);
            //_spriteBatch.DrawString(font, "Current Pot: " + rules.pot, new Vector2(500, 250), Color.Black);

            foreach (Card card in dealerHand)
            {
                card.Draw();
            }
        }

        private void DrawBetting(GameTime gameTime)
        {
            _spriteBatch.DrawString(font, "Bet: " + Bet, new Vector2(800, 500), Color.Black);
            _spriteBatch.DrawString(font, "Amount to bet: " + BetAmount, new Vector2(800, 550), Color.Black);
        }

        private void DrawGameEnd(GameTime gameTime)
        {
            if (rules != null)
                if (player.Score == 1)
                    _spriteBatch.DrawString(font, "You got a " + player.HighCard.Value + " high", new Vector2(50, 50), Color.Black);
                else
                    _spriteBatch.DrawString(font, "You got Score: " + player.Score, new Vector2(50, 50), Color.Black);

            _spriteBatch.DrawString(font, "Press enter to reset or backspace to go to Main Menu", new Vector2(250, 50), Color.Black);
        }

        private void LoadPlayers(GameTime gameTime)
        {
            if(numAi == 0)
            {
                player = new Player(150);
                player.hand[0] = deck.DrawCard();
                player.hand[1] = deck.DrawCard();

                if (player.hand[0] != null)
                    player.hand[0].Position = new Vector2(100, 450);

                if (player.hand[1] != null)
                    player.hand[1].Position = new Vector2(300, 450);
            }

            else
            {
                for(int i = 0; i < numAi; i++)
                {
                    Ais.Add(new AIPlayer(150));

                    do
                    {
                        index = rnd.Next(0, Names.Length);
                    }while(TakenNames.Contains(index));

                    Ais[i].Name = Names[index];
                    TakenNames.Add(index);
                }
                player = new Player(150);
                
                for(int i = 0; i < 2; i++)
                {
                    player.hand[i] = deck.DrawCard();
                    for(int j = 0; j < numAi; j++)
                    {
                        Ais[j].Hand[i] = deck.DrawCard();
                    }
                }

                player.SetHighCard();

                for(int i = 0; i < numAi; i++)
                {
                    Ais[i].SetHighCard();
                }

                if (player.hand[0] != null)
                    player.hand[0].Position = new Vector2(100, 450);

                if (player.hand[1] != null)
                    player.hand[1].Position = new Vector2(300, 450);

                /*if (Ais[0].Hand[0] != null)
                    Ais[0].Hand[0].Position = new Vector2(700, 450);

                if (Ais[0].Hand[1] != null)
                    Ais[0].Hand[1].Position = new Vector2(900, 450);*/
            }
        }
    }
}
