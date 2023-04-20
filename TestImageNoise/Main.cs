using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestImageNoise
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static int seed = -11258;
        public static RenderTarget2D noiseTarget;
        public static int noiseSizeX = 800;
        public static int noiseSizeY = 480;
        public static FastNoiseLite noise;
        public static float noiseFreq = 1f;
        public static bool redrawNoise = false;
        public static Texture2D NoiseTexture;
        public static bool noiseInGen = false;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1202;
            graphics.PreferredBackBufferHeight = 802;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            noiseTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            NoiseTexture = Content.Load<Texture2D>("npc_0");
        }

        protected override void Update(GameTime gameTime)
        {
            Keys[] keysPresed = Keyboard.GetState().GetPressedKeys();
            if(keysPresed.Length > 0)
            {
                switch (keysPresed[0])
                {
                    case Keys.P:
                        GenNoise(0);
                        break;
                    case Keys.C:
                        GenNoise(1);
                        break;
                    case Keys.F:
                        GenNoise(2);
                        break;
                    case Keys.V:
                        GenNoise(3);
                        break;
                    case Keys.S:
                        GenNoise(4);
                        break;
                    case Keys.D2:
                        GenNoise(5);
                        break;
                }
            }

            base.Update(gameTime);
        }

        public void GenNoise(int noiseType)
        {
            if (noiseInGen)
                return;
            noiseInGen = true;
            noise = new FastNoiseLite(seed);
            switch (noiseType)
            {
                case 0:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
                    break;
                case 1:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
                    break;
                case 2:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.ValueCubic);
                    break;
                case 3:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.Value);
                    break;
                case 4:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
                    break;
                case 5:
                    noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
                    break;
            }
            noise.SetSeed(seed);
            noise.SetFrequency(0.01f);
            redrawNoise = true;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(noiseTarget);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
            for (int x = 0; x < graphics.PreferredBackBufferWidth; x++)
            {
                for (int y = 0; y < graphics.PreferredBackBufferHeight; y++)
                {
                    float u = noise.GetNoise(x, y);
                    if (u >= 0.5f)
                    {
                        float n = ((noise.GetNoise(x, y) + 1f) / 2f);
                        Color c = Color.White * n;
                        c.A = 255;
                        //spriteBatch.Draw(NoiseTexture, new Vector2(x, y), c);
                        spriteBatch.Draw(NoiseTexture, new Vector2(x, y), Color.White);
                    }
                }
            }
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            noiseInGen = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(noiseTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //if(noise != null)
            //{
            //    GraphicsDevice.SetRenderTarget(noiseTarget);
            //    spriteBatch.Begin();
            //    for (int x = 0; x < 800; x++)
            //    {
            //        for (int y = 0; y < 480; y++)
            //        {
            //            float u = noise.GetNoise(x, y);
            //            float n = ((noise.GetNoise(x, y) + 1f) / 2f);
            //            Color c = Color.White * n;
            //            c.A = 255;
            //            spriteBatch.Draw(NoiseTexture, new Vector2(x, y), c);
            //        }
            //    }
            //    spriteBatch.End();
            //    GraphicsDevice.SetRenderTarget(null);
            //}

            base.Draw(gameTime);
        }
    }
}