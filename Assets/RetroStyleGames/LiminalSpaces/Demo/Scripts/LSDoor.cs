using UnityEngine;

namespace RetroStyleGames.LiminalSpaces
{
    /// <summary>Swings a door leaf about its hinge pivot (the leaf prefab root), away from whoever opens it.</summary>
    public class LSDoor : MonoBehaviour
    {
        public float openAngle = 95f;
        public float duration = 1.1f;
        Quaternion closed; float target, angle, vel; bool init;
        Collider[] cols;

        void Init() { if (init) return; closed = transform.localRotation; cols = GetComponentsInChildren<Collider>(); init = true; }

        public void Toggle(Vector3 from)
        {
            Init();
            if (Mathf.Abs(target) > 1f) { target = 0f; return; }
            // open away from the player: the leaf's local Z is the wall normal
            float side = Vector3.Dot(transform.forward, from - transform.position);
            target = side > 0 ? -openAngle : openAngle;
        }

        void Update()
        {
            if (!init) return;
            angle = Mathf.SmoothDamp(angle, target, ref vel, duration * 0.35f);
            transform.localRotation = closed * Quaternion.Euler(0, angle, 0);
            // an open leaf (with its handles) narrows the 0.8 m opening below the player's width: once it has swung
            // clear it becomes a trigger (no blocking, still hit by the use ray to close it) and solid again while
            // closing (artist 2026-10-23: could not pass a door)
            bool trigger = Mathf.Abs(angle) > 35f;
            foreach (var c in cols) if (c != null && c.isTrigger != trigger) c.isTrigger = trigger;
        }
    }
}
