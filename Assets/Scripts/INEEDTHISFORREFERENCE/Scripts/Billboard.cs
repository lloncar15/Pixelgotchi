using UnityEngine;

public class SpriteBillboardCPU : MonoBehaviour
{
    public enum Mode { Spherical, CylindricalY }
    public Mode mode = Mode.CylindricalY;
    public bool matchCameraRoll = false; // only for Spherical

    void LateUpdate()
    {
        var cam = Camera.main;
        if (!cam) return;

        if (mode == Mode.Spherical)
        {
            if (matchCameraRoll)
            {
                // Face camera fully (including roll)
                transform.rotation = cam.transform.rotation;
            }
            else
            {
                // Face camera, ignore roll
                Vector3 toCam = cam.transform.position - transform.position;
                if (toCam.sqrMagnitude < 1e-8f) return;
                transform.rotation = Quaternion.LookRotation(toCam, Vector3.up);
            }
        }
        else // CylindricalY: keep upright, rotate around Y only
        {
            Vector3 toCam = cam.transform.position - transform.position;
            toCam.y = 0f;
            if (toCam.sqrMagnitude < 1e-8f) return;
            transform.rotation = Quaternion.LookRotation(toCam, Vector3.up);
        }
    }
}