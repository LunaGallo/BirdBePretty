using UnityEngine;

namespace LunaLib {
    public static partial class CameraExtension {

        public static float OrthographicWidth(this Camera cam) => cam.aspect * cam.orthographicSize * 2f;
        public static float OrthographicHeight(this Camera cam) => cam.orthographicSize * 2f;
        public static Vector2 OrthographicSize(this Camera cam) => new (cam.OrthographicWidth(), cam.OrthographicHeight());
        public static Rect OrthographicLocalRect(this Camera cam) => new (-cam.aspect * cam.orthographicSize, -cam.orthographicSize, cam.aspect * cam.orthographicSize * 2f, cam.orthographicSize * 2f);

    }

}
