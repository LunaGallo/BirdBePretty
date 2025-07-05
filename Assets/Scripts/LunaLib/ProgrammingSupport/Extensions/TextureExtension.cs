using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public static partial class TextureExtension {

        public static Texture2D Duplicate(this Texture2D tex) {
            Texture2D result = tex.CreateSimilar(tex.width, tex.height);
            result.SetPixels(tex.GetPixels());
            result.Apply();
            return result;
        }
        public static Texture2D CreateSimilar(this Texture2D tex, int width, int height) {
            return new Texture2D(width, height, tex.format, true) {
                filterMode = tex.filterMode,
                wrapMode = tex.wrapMode
            };
        }

        public static Texture2D Rotated90(this Texture2D tex) {
            Texture2D result = tex.Duplicate();
            for (int i = 0; i < result.width; i++) {
                for (int j = 0; j < result.height; j++) {
                    int k = tex.width - (j + 1);
                    result.SetPixel(i, j, tex.GetPixel(k, i));
                }
            }
            result.Apply();
            return result;
        }
        public static Texture2D Rotated90(this Texture2D tex, int times) {
            times %= 4;
            for (int i = 0; i < times; i++) {
                tex = tex.Rotated90();
            }
            return tex;
        }

        public static Texture2D FlippedX(this Texture2D tex) {
            Texture2D result = tex.Duplicate();
            for (int i = 0; i < result.width; i++) {
                for (int j = 0; j < result.height; j++) {
                    int k = tex.width - (i + 1);
                    result.SetPixel(i, j, tex.GetPixel(k, j));
                }
            }
            result.Apply();
            return result;
        }
        public static Texture2D FlippedY(this Texture2D tex) {
            Texture2D result = tex.Duplicate();
            for (int i = 0; i < result.width; i++) {
                for (int j = 0; j < result.height; j++) {
                    int k = tex.height - (j + 1);
                    result.SetPixel(i, j, tex.GetPixel(i, k));
                }
            }
            result.Apply();
            return result;
        }

        public static Texture2D Cropped(this Texture2D tex, int x, int y, int width, int height) {
            Texture2D result = tex.CreateSimilar(width, height);
            result.SetPixels(tex.GetPixels(x, y, width, height));
            result.Apply();
            return result;
        }
        public static Texture2D Cropped(this Texture2D tex, Vector2Int pos, Vector2Int size) {
            Texture2D result = tex.CreateSimilar(size.x, size.y);
            result.SetPixels(tex.GetPixels(pos.x, pos.y, size.x, size.y));
            result.Apply();
            return result;
        }


        public static Color GetPixel(this Texture2D tex, Vector2Int pos) {
            return tex.GetPixel(pos.x, pos.y);
        }
        public static void SetPixel(this Texture2D tex, Vector2Int pos, Color color) {
            tex.SetPixel(pos.x, pos.y, color);
        }


        public static void DrawLine(this Texture2D tex, Vector2Int p1, Vector2Int p2, Color color, int thickness) {
            RasterizationSolver.RasterizeLine(p1, p2, thickness).ForEach(p => tex.SetPixel(p, color));
        }
        public static void DrawLine(this Texture2D tex, Vector2Int p1, Vector2Int p2, Color color) {
            RasterizationSolver.RasterizeLine(p1, p2).ForEach(p => tex.SetPixel(p, color));
        }

        public static void DrawCircumference(this Texture2D tex, int radius, Vector2Int center, float startAngle, float finishAngle, Color color) {
            RasterizationSolver.RasterizeCircumference(radius, center, startAngle, finishAngle).ForEach(p => tex.SetPixel(p, color));
        }
        public static void DrawCircumference(this Texture2D tex, int radius, Vector2Int center, Color color) {
            RasterizationSolver.RasterizeCircumference(radius, center).ForEach(p => tex.SetPixel(p, color));
        }

        public static void DrawCircle(this Texture2D tex, int radius, Vector2Int center, Color color) {
            RasterizationSolver.RasterizeCircumference(radius, center).ForEach(p => tex.SetPixel(p, color));
            float sqrRadius = radius * radius;
            foreach (Vector2Int p in (new RectInt(center - Vector2Int.one * radius, Vector2Int.one * radius * 2)).allPositionsWithin) {
                if (p.sqrMagnitude <= sqrRadius) {
                    tex.SetPixel(p, color);
                }
            }
        }


        public static void SetPixelIsland(this Texture2D tex, Vector2Int origin, Color color, float colorDistanceThreshold) {
            tex.SelectPixelIsland(origin, colorDistanceThreshold).ForEach(p => tex.SetPixel(p, color));
        }
        public static List<Vector2Int> SelectPixelIsland(this Texture2D tex, Vector2Int origin, float colorDistanceThreshold) {
            Color originColor = tex.GetPixel(origin);
            return RasterizationSolver.GetPointIsland(origin, p => originColor.DistanceTo(tex.GetPixel(p), ColorSystem.HCL) <= colorDistanceThreshold);
        }

    }

}
