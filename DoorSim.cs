using System.ComponentModel;
using System.Numerics;
using Microsoft.VisualBasic;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Door
{
    class Program
    {
        
        const int screenWidth = 800;
        const int screenHeight = 450;
        static readonly float FRAMES = 1f;
        static int Score = 0;
        static void Main()
        {
            
            

            int btnState = 0;
            bool btnAction = false;

            bool IsFlipped = false;

            bool GameClosed = false;

            float FPS = 0;
            bool Drawit = true;

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            
            Raylib.InitWindow(screenWidth, screenHeight, "Door Simulator");

            Raylib.InitAudioDevice();

            Sound Door = Raylib.LoadSound("door-2-open.wav"); // Hi :3
            Sound Button = Raylib.LoadSound("fish.wav");

            Texture2D button = LoadTexture("button.png");

            float changed = 1.03f;

            float frameHeight = (float)button.Height/FRAMES;
            Rectangle sourceRec = new(0, 0, button.Width, button.Height);

            

            

            float Volume = 0f;
            Volume = Raylib.GetMasterVolume();
            
            RenderTexture2D Start = Raylib.LoadRenderTexture(480, 240);
            Raylib.SetTextureFilter(Start.Texture, TextureFilter.Point);

            Vector2 mousePoint = new( 0.0f, 0.0f );

            while (!Raylib.WindowShouldClose())
            {
                mousePoint = GetMousePosition();

                Rectangle btnBounds = new( screenWidth/changed - button.Width/2.0f, screenHeight/2.0f - button.Height/FRAMES/2.0f, button.Width, frameHeight );

                if (CheckCollisionPointRec(mousePoint, btnBounds))
                {
                    
                    if (IsMouseButtonDown(MouseButton.Left)) btnState = 2;
                    else btnState = 1;

                    if (IsMouseButtonReleased(MouseButton.Left)) btnAction = !btnAction;

                }
                else btnState = 0;

                if (btnAction)
                {   
                    IsFlipped = true;    
                    changed = 1.7f;        
                } else
                {
                    IsFlipped = false;
                    changed = 1.03f;
                }
                
                sourceRec.Y = btnState*frameHeight;

                Raylib.SetMasterVolume(Volume);

                if (!Raylib.IsWindowFocused())
                {
                    GameClosed = true;
                }
                else
                {
                    GameClosed = false;
                }
                int SizeH = Raylib.GetScreenHeight();
                int SizeW = Raylib.GetScreenWidth();
                Vector2 Center = Raylib.GetScreenCenter();
                FPS += Raylib.GetFrameTime();
                //Console.WriteLine(FPS);
                //Console.WriteLine(GameClosed);
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    Volume -= (float)0.05;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    Volume += (float)0.05;
                }
                if (Raylib.IsMouseButtonPressed(MouseButton.Left) || (Raylib.IsKeyDown(KeyboardKey.Space) && FPS >= 0.2))
                {
                    FPS = 0;
                    Drawit = !Drawit;
                    if (!Drawit)
                    {
                        Score ++ ;
                    }
                }
                Raylib.BeginTextureMode(Start);
                Raylib.ClearBackground(Color.Blue); 
                if (Drawit)
                {
                    Raylib.DrawRectangle(200,120-100/2,50,100,Color.Brown);
                    Raylib.DrawRectangle(240,120-5/2,5,5,Color.Green);
                    Raylib.PlaySound(Door);
                }

                if (!Drawit)
                {
                    Raylib.DrawRectangle(200,120-100/2,4,100,Color.Brown);
                    Raylib.DrawRectangle(200-5,120-5/2,5,5,Color.Green);
                }

                Raylib.DrawText(Score.ToString(), 10, 10, 32, Color.Gold);
                Raylib.DrawText(Volume.ToString(".00"), 80, 10, 32, Color.Gold);

                Raylib.EndTextureMode();

                Raylib.BeginDrawing();
                    DrawTexturePro(Start.Texture, new Rectangle(0,0,Start.Texture.Width,-Start.Texture.Height), new Rectangle(0,0,SizeW,SizeH), new Vector2(), 0, Color.White);
                    //DrawRectangleRec(btnBounds, Color.Red);
                if (IsFlipped)
                {
                    DrawTextureRec(button, new Rectangle(sourceRec.X, sourceRec.Y, -sourceRec.Width, sourceRec.Height), new Vector2(btnBounds.X, btnBounds.Y), Color.White);
                } else
                {
                    DrawTextureRec(button, new Rectangle(sourceRec.X, sourceRec.Y, sourceRec.Width, sourceRec.Height), new Vector2(btnBounds.X, btnBounds.Y), Color.White);
                }
                Raylib.EndDrawing();

                if (GameClosed == true)
                {
                    UnloadTexture(button);
                    UnloadSound(Door);
                    UnloadSound(Button);
                    CloseAudioDevice();
                    CloseWindow();
                }

            }

            Raylib.CloseWindow();
            //Console.WriteLine("col");       
            
        }

    }
}
