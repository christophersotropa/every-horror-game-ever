using UnityEngine;
using UnityEngine.Rendering;

namespace RetroStyleGames.LiminalSpaces
{
    /// <summary>
    /// Scene ambient ("sky") light for the unbaked demo scenes: a three-colour gradient whose ground colour is the
    /// brightest, standing in for light bounced off the floor. Without a lighting bake Unity does not keep the ambient
    /// probe, so this component rebuilds it whenever the scene is enabled (editor and play mode).
    /// </summary>
    [ExecuteAlways]
    public class LSAmbient : MonoBehaviour
    {
        public Color sky = new Color(0.045f, 0.046f, 0.05f);
        public Color equator = new Color(0.06f, 0.06f, 0.062f);
        public Color ground = new Color(0.085f, 0.082f, 0.078f);
        [Range(0f, 4f)] public float intensity = 1f;

        void OnEnable() { Apply(); }
        void OnValidate() { Apply(); }

        public void Apply()
        {
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = sky * intensity;
            RenderSettings.ambientEquatorColor = equator * intensity;
            RenderSettings.ambientGroundColor = ground * intensity;
            var sh = new SphericalHarmonicsL2();
            sh.AddAmbientLight(equator * intensity);
            sh.AddDirectionalLight(Vector3.up, (sky - equator) * intensity * 1.2f, 1f);
            sh.AddDirectionalLight(Vector3.down, (ground - equator) * intensity * 1.2f, 1f);
            RenderSettings.ambientProbe = sh;
        }
    }
}
