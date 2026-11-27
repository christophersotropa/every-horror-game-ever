using UnityEngine;
#if LS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace RetroStyleGames.LiminalSpaces
{
    /// <summary>Keyboard / mouse reads that work with the Input System package or the legacy Input Manager.</summary>
    public static class LSInput
    {
#if LS_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
        public static Vector2 Move()
        {
            var k = Keyboard.current; if (k == null) return Vector2.zero;
            return new Vector2((k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0), (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0));
        }
        public static Vector2 Look() { var m = Mouse.current; return m == null ? Vector2.zero : m.delta.ReadValue() * 0.05f; }
        public static bool Run() { var k = Keyboard.current; return k != null && k.leftShiftKey.isPressed; }
        public static bool Crouch() { var k = Keyboard.current; return k != null && k.leftCtrlKey.isPressed; }
        public static bool Flashlight() { var k = Keyboard.current; return k != null && k.fKey.wasPressedThisFrame; }
        public static bool Use() { var k = Keyboard.current; return k != null && k.eKey.wasPressedThisFrame; }
        public static bool NextRoom() { var k = Keyboard.current; return k != null && (k.rightBracketKey.wasPressedThisFrame || k.pageDownKey.wasPressedThisFrame); }
        public static bool PrevRoom() { var k = Keyboard.current; return k != null && (k.leftBracketKey.wasPressedThisFrame || k.pageUpKey.wasPressedThisFrame); }
        public static bool Help() { var k = Keyboard.current; return k != null && k.hKey.wasPressedThisFrame; }
        public static bool Escape() { var k = Keyboard.current; return k != null && k.escapeKey.wasPressedThisFrame; }
        public static bool Click() { var m = Mouse.current; return m != null && m.leftButton.wasPressedThisFrame; }
#else
        public static Vector2 Move() { return new Vector2((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0), (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)); }
        public static Vector2 Look() { return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); }
        public static bool Run() { return Input.GetKey(KeyCode.LeftShift); }
        public static bool Crouch() { return Input.GetKey(KeyCode.LeftControl); }
        public static bool Flashlight() { return Input.GetKeyDown(KeyCode.F); }
        public static bool Use() { return Input.GetKeyDown(KeyCode.E); }
        public static bool NextRoom() { return Input.GetKeyDown(KeyCode.RightBracket) || Input.GetKeyDown(KeyCode.PageDown); }
        public static bool PrevRoom() { return Input.GetKeyDown(KeyCode.LeftBracket) || Input.GetKeyDown(KeyCode.PageUp); }
        public static bool Help() { return Input.GetKeyDown(KeyCode.H); }
        public static bool Escape() { return Input.GetKeyDown(KeyCode.Escape); }
        public static bool Click() { return Input.GetMouseButtonDown(0); }
#endif
    }
}
