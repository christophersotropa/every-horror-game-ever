using UnityEngine;

namespace RetroStyleGames.LiminalSpaces
{
    /// <summary>
    /// Heavy "bodycam" first-person walker: the camera is strapped to the chest, so it carries the body's inertia,
    /// every footfall and the sway of the torso. Slow acceleration / braking, lagging turns, step dips with a roll,
    /// lean on strafe and turn, idle breathing, limited pitch (a chest camera cannot look far up or down).
    /// WASD move, Shift walk faster, Ctrl crouch, mouse look, F flashlight, E open / close a door,
    /// [ / ] (PageUp / PageDown) previous / next room, H help, Esc release the cursor.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class LSBodycamPlayer : MonoBehaviour
    {
        [Header("Rig")]
        public Transform cameraMount;           // chest pivot (child), the camera sits under it
        public Camera bodycam;
        public Light flashlight;

        [Header("Movement")]
        public float walkSpeed = 1.35f;
        public float runSpeed = 2.5f;
        public float crouchSpeed = 0.75f;
        public float accelerationTime = 0.55f;  // seconds to reach speed: weight
        public float brakingTime = 0.42f;
        public float gravity = 18f;
        public float chestHeight = 1.38f;
        public float crouchChestHeight = 0.95f;

        [Header("Look")]
        public float mouseSensitivity = 2.2f;
        public float yawLag = 0.09f;            // body turns after the mouse
        public float pitchLag = 0.16f;
        public float pitchLimit = 38f;
        public float restPitch = 4f;            // chest cameras sit tilted slightly down

        [Header("Body motion")]
        public float stepLength = 0.72f;
        public float bobHeight = 0.034f;
        public float bobSide = 0.022f;
        public float stepRoll = 1.1f;
        public float stepPitch = 0.9f;
        public float strafeLean = 2.4f;
        public float turnLean = 0.025f;
        public float breathe = 0.006f;
        public float shake = 0.22f;

        [Header("Flashlight")]
        public float flashlightLag = 9f;
        public bool flashlightOn = false;

        CharacterController cc;
        Vector3 velocity, accelRef;
        float vy, yaw, pitch, yawTarget, pitchTarget, yawVel, pitchVel, stepPhase, chestY, chestVel, lean, leanVel;
        float onTimer = 1f, lookBlock = 0.35f;   // ignore the cursor-lock jump of the first frames
        Quaternion lightRot;
        Transform cam;

        public float Yaw { get { return yaw; } }

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            cam = bodycam != null ? bodycam.transform : GetComponentInChildren<Camera>().transform;
            yaw = yawTarget = transform.eulerAngles.y;
            chestY = chestHeight;
            if (flashlight != null) { flashlight.enabled = flashlightOn; lightRot = cam.rotation; }
            // drop-in use: one player rig drives the scene. A second rig (prefab dragged into a scene that already has one)
            // switches itself off; a leftover default Main Camera is switched off so only this rig renders.
            foreach (var other in FindObjectsByType<LSBodycamPlayer>(FindObjectsSortMode.None))
            {
                if (other == this || !other.driving) continue;
                Debug.LogWarning("[LiminalSpaces] More than one " + nameof(LSBodycamPlayer) + " in the scene: '" + name + "' disabled, '" + other.name + "' is used.", this);
                gameObject.SetActive(false);
                return;
            }
            driving = true;
            foreach (var c in FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (c.transform.IsChildOf(transform) || !c.CompareTag("MainCamera") || c.GetComponentInParent<LSBodycamPlayer>() != null) continue;
                c.gameObject.SetActive(false);
            }
        }

        bool driving;   // this rig won the drop-in check (no statics: they survive Play with domain reload off)

        void OnEnable() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }

        public void Teleport(Vector3 pos, float yawDeg)
        {
            if (cc == null) cc = GetComponent<CharacterController>();
            cc.enabled = false;
            transform.position = pos; yaw = yawTarget = yawDeg; pitch = pitchTarget = 0f;
            velocity = Vector3.zero; vy = 0f; lookBlock = 0.2f;
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            cc.enabled = true;
            if (flashlight != null) lightRot = cam.rotation;
        }

        void Update()
        {
            float dt = Mathf.Min(Time.deltaTime, 0.05f);
            if (LSInput.Escape()) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            if (LSInput.Click() && Cursor.lockState != CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }

            // look: the body follows the mouse with a lag, pitch is heavier and limited
            lookBlock -= dt;
            if (Cursor.lockState == CursorLockMode.Locked && lookBlock <= 0f)
            {
                var look = LSInput.Look() * mouseSensitivity;
                yawTarget += look.x;
                pitchTarget = Mathf.Clamp(pitchTarget - look.y, -pitchLimit, pitchLimit);
            }
            float prevYaw = yaw;
            yaw = Mathf.SmoothDampAngle(yaw, yawTarget, ref yawVel, yawLag, Mathf.Infinity, dt);
            pitch = Mathf.SmoothDamp(pitch, pitchTarget, ref pitchVel, pitchLag, Mathf.Infinity, dt);
            float turnRate = Mathf.DeltaAngle(prevYaw, yaw) / Mathf.Max(dt, 1e-4f);
            transform.rotation = Quaternion.Euler(0, yaw, 0);

            // move: weighty acceleration and braking
            var input = Vector2.ClampMagnitude(LSInput.Move(), 1f);
            bool crouch = LSInput.Crouch();
            float speed = crouch ? crouchSpeed : (LSInput.Run() ? runSpeed : walkSpeed);
            if (input.y < 0) speed *= 0.7f;                                   // walking backwards is slower
            var wish = (transform.forward * input.y + transform.right * input.x * 0.85f) * speed;
            velocity = Vector3.SmoothDamp(velocity, wish, ref accelRef, wish.sqrMagnitude > velocity.sqrMagnitude ? accelerationTime : brakingTime, Mathf.Infinity, dt);
            if (cc.isGrounded) vy = -2f; else vy -= gravity * dt;
            cc.Move((velocity + Vector3.up * vy) * dt);

            // chest height (crouch) settles slowly
            chestY = Mathf.SmoothDamp(chestY, crouch ? crouchChestHeight : chestHeight, ref chestVel, 0.25f, Mathf.Infinity, dt);

            // footsteps: phase advances with distance walked, 2 steps per cycle
            float planar = new Vector3(velocity.x, 0, velocity.z).magnitude;
            float moving = Mathf.Clamp01(planar / walkSpeed);
            if (cc.isGrounded) stepPhase += planar * dt / stepLength * Mathf.PI;
            float s = Mathf.Sin(stepPhase), c = Mathf.Cos(stepPhase);
            float impact = Mathf.Pow(Mathf.Abs(s), 6f);                       // sharp dip on each foot plant
            float runK = Mathf.Lerp(1f, 1.45f, Mathf.InverseLerp(walkSpeed, runSpeed, planar));
            float t = Time.time;

            Vector3 pos = new Vector3(c * bobSide * moving * runK, chestY - (Mathf.Abs(s) * 0.6f + impact * 0.4f) * bobHeight * moving * runK, 0f);
            pos.y += Mathf.Sin(t * 1.6f) * breathe * (1f - moving * 0.6f);
            float localStrafe = Vector3.Dot(velocity, transform.right);
            lean = Mathf.SmoothDamp(lean, -localStrafe * strafeLean - turnRate * turnLean, ref leanVel, 0.22f, Mathf.Infinity, dt);
            float n1 = Mathf.PerlinNoise(t * 0.9f, 3.1f) - 0.5f, n2 = Mathf.PerlinNoise(7.3f, t * 0.8f) - 0.5f;
            float sh = shake * (0.35f + moving * runK);
            Vector3 rot = new Vector3(restPitch + pitch + impact * stepPitch * moving * runK + n1 * sh + Mathf.Sin(t * 1.6f) * 0.25f,
                                      n2 * sh * 1.3f,
                                      lean + c * stepRoll * moving * runK);
            cameraMount.localPosition = pos;
            cameraMount.localRotation = Quaternion.Euler(rot);

            // flashlight: hand-held feel, trails the camera slightly
            if (flashlight != null)
            {
                if (LSInput.Flashlight()) { flashlightOn = !flashlightOn; onTimer = 0f; }
                onTimer += dt;
                flashlight.enabled = flashlightOn && !(onTimer < 0.12f && Mathf.PerlinNoise(t * 60f, 0f) > 0.55f);
                lightRot = Quaternion.Slerp(lightRot, cam.rotation, 1f - Mathf.Exp(-flashlightLag * dt));
                flashlight.transform.rotation = lightRot;
            }

            if (LSInput.Use()) TryUseDoor();
        }

        void TryUseDoor()
        {
            // open leaves are triggers: look for the nearest door among solid and trigger hits (volumes are triggers too)
            var hits = Physics.RaycastAll(cam.position, cam.forward, 2.6f, ~0, QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            Transform t = null;
            foreach (var h in hits)
            {
                var p = h.collider.transform;
                while (p != null && p.name != "Door") p = p.parent;
                if (p != null) { t = p; break; }
                if (!h.collider.isTrigger) break;                            // a wall in front of the door
            }
            if (t != null) UseDoor(t, cam.position);
        }

        /// <summary>Opens / closes the door leaf root <paramref name="leaf"/> (a transform named "Door") away from <paramref name="from"/>.</summary>
        public static void UseDoor(Transform leaf, Vector3 from)
        {
            // TryGetComponent: in the editor GetComponent returns a fake null that `??` treats as a component
            LSDoor door;
            if (!leaf.TryGetComponent(out door)) door = leaf.gameObject.AddComponent<LSDoor>();
            door.Toggle(from);
        }
    }
}
