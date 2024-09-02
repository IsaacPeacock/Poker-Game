using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Violet;

public static class InputManager
{
    private static MouseState LastMs;
    public static KeyboardState CurrentKeyState { get; private set; }
    public static KeyboardState PrevKeyState { get; private set; }
    public static bool CPressed { get; private set; }
    public static bool RPressed { get; private set; }
    public static bool FPressed { get; private set; }
    public static bool UpPressed { get; private set; }
    public static bool DownPressed { get; private set; }
    public static bool RightPressed { get; private set; }
    public static bool LeftPressed { get; private set; }
    public static bool TwoPressed { get; private set; }
    public static bool SpacePressed { get; private set; }
    public static bool LeftClick { get; private set; }
    public static Point MousePosition { get; private set; }

    public static void Update()
    {
        MouseState mouseState = Mouse.GetState();
        CurrentKeyState = Keyboard.GetState();

        LeftClick = mouseState.LeftButton == ButtonState.Pressed && LastMs.LeftButton == ButtonState.Released;
        CPressed = CurrentKeyState.IsKeyDown(Keys.C) && PrevKeyState.IsKeyUp(Keys.C);
        RPressed = CurrentKeyState.IsKeyDown(Keys.R) && PrevKeyState.IsKeyUp(Keys.R);
        FPressed = CurrentKeyState.IsKeyDown(Keys.F) && PrevKeyState.IsKeyUp(Keys.F);
        UpPressed = CurrentKeyState.IsKeyDown(Keys.Up) && PrevKeyState.IsKeyUp(Keys.Up);
        DownPressed = CurrentKeyState.IsKeyDown(Keys.Down) && PrevKeyState.IsKeyUp(Keys.Down);
        RightPressed = CurrentKeyState.IsKeyDown(Keys.Right) && PrevKeyState.IsKeyUp(Keys.Right);
        LeftPressed = CurrentKeyState.IsKeyDown(Keys.Left) && PrevKeyState.IsKeyUp(Keys.Left);
        SpacePressed = CurrentKeyState.IsKeyDown(Keys.Space) && PrevKeyState.IsKeyUp(Keys.Space);
        TwoPressed = CurrentKeyState.IsKeyDown(Keys.NumPad2) && PrevKeyState.IsKeyUp(Keys.NumPad2);
        MousePosition = mouseState.Position;

        PrevKeyState = CurrentKeyState;
        LastMs = mouseState;
    }

    public static bool HasBeenPressed(Keys key)
    {
        return CurrentKeyState.IsKeyDown(key) && PrevKeyState.IsKeyUp(key);
    }
}
