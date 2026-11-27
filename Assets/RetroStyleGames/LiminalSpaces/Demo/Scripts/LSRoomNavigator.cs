using UnityEngine;

namespace RetroStyleGames.LiminalSpaces
{
    /// <summary>Jumps the player between the room spawn points ([ / ] or PageUp / PageDown) and shows the controls.</summary>
    public class LSRoomNavigator : MonoBehaviour
    {
        public LSBodycamPlayer player;
        public Transform[] spawns;
        public int current;
        public bool showHelp = true;
        float labelTime = 4f;

        void Start() { if (spawns != null && spawns.Length > 0) Go(Mathf.Clamp(current, 0, spawns.Length - 1)); }

        void Update()
        {
            if (spawns == null || spawns.Length == 0 || player == null) return;
            if (LSInput.NextRoom()) Go((current + 1) % spawns.Length);
            if (LSInput.PrevRoom()) Go((current + spawns.Length - 1) % spawns.Length);
            if (LSInput.Help()) showHelp = !showHelp;
            labelTime -= Time.deltaTime;
        }

        void Go(int i)
        {
            if (player == null) player = FindAnyObjectByType<LSBodycamPlayer>(FindObjectsInactive.Include);
            if (player == null) return;
            if (!player.gameObject.activeSelf) player.gameObject.SetActive(true);
            current = i;
            player.Teleport(spawns[i].position, spawns[i].eulerAngles.y);
            labelTime = 3f;
        }

        void OnGUI()
        {
            var st = new GUIStyle(GUI.skin.label) { fontSize = 15 };
            st.normal.textColor = new Color(1f, 1f, 1f, 0.85f);
            if (labelTime > 0 && spawns != null && spawns.Length > 0)
                GUI.Label(new Rect(20, 16, 600, 26), spawns[current].name.Replace("Spawn_", "") + "  (" + (current + 1) + "/" + spawns.Length + ")", st);
            if (showHelp)
                GUI.Label(new Rect(20, Screen.height - 44, 1200, 26), "WASD move · Shift faster · Ctrl crouch · F flashlight · E door · [ ] room · H hide help · Esc cursor", st);
        }
    }
}
