using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LunaLib {
    public static partial class Geometry {

        public static readonly float sqrt3 = Mathf.Sqrt(3f);

        #region 2D

        #region Directions
        public static bool IsBetweenDirections(Vector2 v, Vector2 dir1, Vector2 dir2, bool inclusive = true) {
            float angleV = Vector2.SignedAngle(dir1, v);
            float angleD2 = Vector2.SignedAngle(dir1, dir2);
            if (inclusive) {
                return angleV == 0f || (angleV.Sign() == angleD2.Sign() && angleV.Abs() <= angleD2.Abs());
            }
            else {
                return angleV.Sign() == angleD2.Sign() && angleV.Abs() < angleD2.Abs();
            }
        }
        public static bool SameDirection(Vector2 dir1, Vector2 dir2, bool allowOpposite = false, float angleTreshold = 0f) {
            float angle = Vector2.Angle(dir1, dir2);
            if (allowOpposite && angle > 90f) {
                angle = Vector2.Angle(dir1, -dir2);
            }
            return angle <= angleTreshold;
        }
        #endregion

        #region Lines
        public static bool AreLinesParallel(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float angleTreshold = 0f) {
            return ((v1.x - v2.x) * (v3.y - v4.y) - (v1.y - v2.y) * (v3.x - v4.x)) == angleTreshold;
        }
        public static Vector2? LineLineIntersection(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) {
            float denominator = (v1.x - v2.x) * (v3.y - v4.y) - (v1.y - v2.y) * (v3.x - v4.x);
            if (denominator == 0f) {
                return null;
            }
            else {
                float t = ((v1.x - v3.x) * (v3.y - v4.y) - (v1.y - v3.y) * (v3.x - v4.x)) / denominator;
                return new Vector2(v1.x + t*(v2.x - v1.x), v1.y + t * (v2.y - v1.y));
            }
        }

        internal static float SegmentPointDist((Vector2 p0, Vector2 p1) segment, Vector2 point) {
            Vector2 dir = (segment.p1 - segment.p0).normalized;
            Vector2 relativePoint = point - segment.p0;
            return relativePoint.DistanceTo(dir * Mathf.Clamp(Vector2.Dot(relativePoint, dir), 0f, (segment.p1 - segment.p0).magnitude));
        }
        internal static Vector2 ClosestPointOnLineSegment((Vector2 p0, Vector2 p1) segment, Vector2 point) {
            Vector2 dir = (segment.p1 - segment.p0).normalized;
            Vector2 relativePoint = point - segment.p0;
            return segment.p0 + dir * Mathf.Clamp(Vector2.Dot(relativePoint, dir), 0f, (segment.p1 - segment.p0).magnitude);
        }

        public static List<Vector2> HorizontalLine(float y, float x0, float x1) => new() { new Vector2(x0, y), new Vector2(x1, y) };
        public static List<Vector2> VerticalLine(float x, float y0, float y1) => new() { new Vector2(x, y0), new Vector2(x, y1) };

        public static List<Vector2Int> HorizontalLine(int y, int x0, int x1) => new() { new Vector2Int(x0, y), new Vector2Int(x1, y) };
        public static List<Vector2Int> VerticalLine(int x, int y0, int y1) => new() { new Vector2Int(x, y0), new Vector2Int(x, y1) };

        public static Vector2 Midpoint(Vector2 v1, Vector2 v2) => (v1 + v2) / 2f;

        public static Vector2 ProjectPointOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd) {
            Vector2 lineDir = (lineEnd - lineStart).normalized;
            return lineStart + (lineDir * Vector2.Dot(point - lineStart, lineDir));
        }
        public static float ProjectionDistFromLineStart(Vector2 point, Vector2 lineStart, Vector2 lineEnd) => Vector2.Dot(point - lineStart, (lineEnd - lineStart).normalized);
        #endregion

        #region Polygons
        #region Generic
        public static float PolygonRadiusGivenSide(float side, int sideCount) {
            return side / (2f * Mathf.Sin(Mathf.Deg2Rad * 180f / sideCount));
        }
        public static float PolygonRadiusGivenApothem(float apothem, int sideCount) {
            return apothem / Mathf.Cos(Mathf.Deg2Rad * 180f / sideCount);
        }
        public static float PolygonSideGivenRadius(float radius, int sideCount) {
            return radius * 2f * Mathf.Sin(Mathf.Deg2Rad * 180f / sideCount);
        }
        public static float PolygonSideGivenApothem(float apothem, int sideCount) {
            float inRadians = Mathf.Deg2Rad * 180f / sideCount;
            return apothem * 2f * Mathf.Sin(inRadians) / Mathf.Cos(inRadians);
        }
        public static float PolygonApothemGivenRadius(float radius, int sideCount) {
            return radius * Mathf.Cos(Mathf.Deg2Rad * 180f / sideCount);
        }
        public static float PolygonApothemGivenSide(float side, int sideCount) {
            float inRadians = Mathf.Deg2Rad * 180f / sideCount;
            return side * Mathf.Cos(inRadians) / (2f * Mathf.Sin(inRadians));
        }
        #endregion

        #region Triangle
        public static partial class Triangle {

            #region Grid
            public enum Orientation {
                PointyTop = 0,   //00
                PointyBottom = 1,//01
                PointyRight = 2, //10
                PointyLeft = 3   //11
            }
            /*
            public static Vector2Int OrthonormalToRoundTri(Vector2 orthonormal, float size, Orientation orientation) {
                Vector2 result = orthonormal / size;
                switch (orientation) {
                    case Orientation.PointyTop:
                        result.y /= 1.5f;
                        result.x -= result.y / sqrt3;
                        result.x *= sqrt3 / 2f;
                        Vector2Int intResult = result.RoundedToInt(RoundMethod.Floor);
                        result -= intResult;
                        intResult *= 2;
                        if (result.x + result.y > 0.5f) {
                            intResult.x++;
                            result.x -= (1f - result.y) / 2f;
                        }
                        else {
                            result.x += result.y / 2f;
                        }
                        result += intResult;
                        break;
                    case Orientation.PointyBottom:
                        return orthonormal.MultipliedBy(bothMatrix);
                    case Orientation.PointyRight:
                        return orthonormal.MultipliedBy(sothMatrix);
                    case Orientation.PointyLeft:
                        return orthonormal.MultipliedBy(sothMatrix);
                    default:
                        return default;
                }
                return result;
            }
            */
            public static Vector2 OrthonormalToTri(Vector2 orthonormal, float size, Orientation orientation) {
                Vector2 result = orthonormal / size;
                //WIP
                return result;
            }
            public static Vector2 TriToOrthonormal(Vector2 tri, float size, Orientation orientation) => orientation switch {
                Orientation.PointyTop => new Vector2((tri.x + tri.y) / sqrt3, tri.y * 1.5f + (int)tri.x % 2 * 0.5f) * size,
                Orientation.PointyBottom => new Vector2((tri.x + tri.y) / sqrt3, tri.y * 1.5f - (int)tri.x % 2 * 0.5f) * size,
                Orientation.PointyRight => new Vector2(tri.x * 1.5f + (int)tri.y % 2 * 0.5f, (tri.x + tri.y) / sqrt3) * size,
                Orientation.PointyLeft => new Vector2(tri.x * 1.5f - (int)tri.y % 2 * 0.5f, (tri.x + tri.y) / sqrt3) * size,
                _ => default,
            };

            private static readonly float[,] bothMatrix = new float[2, 2] {
                { 1f, -1f / sqrt3 },
                { 0f,  2f / sqrt3 }
            };
            private static readonly float[,] sothMatrix = new float[2, 2] {
                {  2f / sqrt3, 0f         },
                { -1f / sqrt3, 1f / sqrt3 }
            };
            private static readonly float[,] bhtoMatrix = new float[2, 2] {
                { 1f, 0.5f         },
                { 0f, 0.5f * sqrt3 }
            };
            private static readonly float[,] shtoMatrix = new float[2, 2] {
                { 0.5f * sqrt3, 0f },
                { 0.5f,         1f }
            };

            #endregion

        }
        #endregion

        public static partial class Quadrilateral {

            #region Square
            public static partial class Square {
                public static List<Vector2> Vertices(float scale) {
                    return new List<Vector2>() {
                new Vector2(-scale / 2f,  scale / 2f),
                new Vector2( scale / 2f,  scale / 2f),
                new Vector2( scale / 2f, -scale / 2f),
                new Vector2(-scale / 2f, -scale / 2f)
            };
                }

                public static List<Vector2> BoundVerticalLines(Rect rect) {
                    List<Vector2> result = new List<Vector2>();
                    result.AddRange(VerticalLine(rect.xMax, rect.yMin, rect.yMax));
                    result.AddRange(VerticalLine(rect.xMin, rect.yMin, rect.yMax));
                    return result;
                }
                public static List<Vector2> BoundHorizontalLines(Rect rect) {
                    List<Vector2> result = new();
                    result.AddRange(HorizontalLine(rect.yMax, rect.xMin, rect.xMax));
                    result.AddRange(HorizontalLine(rect.yMin, rect.xMin, rect.xMax));
                    return result;
                }
                public static List<Vector2Int> BoundVerticalLines(RectInt rect) {
                    List<Vector2Int> result = new List<Vector2Int>();
                    result.AddRange(VerticalLine(rect.xMax - 1, rect.yMin, rect.yMax - 1));
                    result.AddRange(VerticalLine(rect.xMin, rect.yMin, rect.yMax - 1));
                    return result;
                }
                public static List<Vector2Int> BoundHorizontalLines(RectInt rect) {
                    List<Vector2Int> result = new List<Vector2Int>();
                    result.AddRange(HorizontalLine(rect.yMax - 1, rect.xMin, rect.xMax - 1));
                    result.AddRange(HorizontalLine(rect.yMin, rect.xMin, rect.xMax - 1));
                    return result;
                }

            }
            #endregion

            public static float Area(Vector2 size) => size.x * size.y;
            public static int Area(Vector2Int size) => size.x * size.y;
        }

        #region Hexagon
        public static partial class Hexagon {

            public static readonly float sqrt3 = Mathf.Sqrt(3f);

            public enum Orientation {
                PointyTop,
                FlatTop
            }

            public static float WidthFromSide(float side, Orientation orientation) => orientation switch {
                Orientation.PointyTop => sqrt3 * side,
                Orientation.FlatTop => 2f * side,
                _ => 0f,
            };
            public static float HeightFromSide(float side, Orientation orientation) => orientation switch {
                Orientation.PointyTop => 2f * side,
                Orientation.FlatTop => sqrt3 * side,
                _ => 0f,
            };
            public static float SideFromWidth(float width, Orientation orientation) => orientation switch {
                Orientation.PointyTop => width / sqrt3,
                Orientation.FlatTop => width / 2f,
                _ => 0f,
            };
            public static float SideFromHeight(float height, Orientation orientation) => orientation switch {
                Orientation.PointyTop => height / 2f,
                Orientation.FlatTop => height / sqrt3,
                _ => 0f,
            };
            public static List<Vector2> VertexPositions(float scale, Orientation orientation) {
                float width;
                float height;
                switch (orientation) {
                    case Orientation.PointyTop:
                        width = scale;
                        height = HeightFromSide(SideFromWidth(width, orientation), orientation);
                        return new List<Vector2>() {
                            new Vector2( width / 2f,  height / 4f),
                            new Vector2( width / 2f, -height / 4f),
                            new Vector2(         0f, -height / 2f),
                            new Vector2(-width / 2f, -height / 4f),
                            new Vector2(-width / 2f,  height / 4f),
                            new Vector2(         0f,  height / 2f)
                        };
                    case Orientation.FlatTop:
                        height = scale;
                        width = WidthFromSide(SideFromHeight(height, orientation), orientation);
                        return new List<Vector2>() {
                            new Vector2( width / 4f,  height / 2f),
                            new Vector2( width / 2f,           0f),
                            new Vector2( width / 4f, -height / 2f),
                            new Vector2(-width / 4f, -height / 2f),
                            new Vector2(-width / 2f,           0f),
                            new Vector2(-width / 4f,  height / 2f)
                        };
                    default:
                        return null;
                }
            }

            #region Grid
            private static readonly float[,] pothMatrix = new float[2, 2] {
                { 1f, -1f / sqrt3 },
                { 0f,  2f / sqrt3 }
            };
            private static readonly float[,] fothMatrix = new float[2, 2] {
                {  2f / sqrt3, 0f         },
                { -1f / sqrt3, 1f / sqrt3 }
            };
            private static readonly float[,] phtoMatrix = new float[2, 2] {
                { 1f, 0.5f         },
                { 0f, 0.5f * sqrt3 }
            };
            private static readonly float[,] fhtoMatrix = new float[2, 2] {
                { 0.5f * sqrt3, 0f },
                { 0.5f,         1f }
            };

            public static Vector2 OrthonormalToHex(Vector2 orthonormal, float size, Orientation orientation) => orientation switch {
                Orientation.PointyTop => orthonormal.MultipliedBy(pothMatrix) / size,
                Orientation.FlatTop => orthonormal.MultipliedBy(fothMatrix) / size,
                _ => default,
            };
            public static Vector2 HexToOrthonormal(Vector2 hex, float size, Orientation orientation) => orientation switch {
                Orientation.PointyTop => hex.MultipliedBy(phtoMatrix) * size,
                Orientation.FlatTop => hex.MultipliedBy(fhtoMatrix) * size,
                _ => default,
            };

            public static Vector2Int HexRound(Vector2 hex) => CubeToHex(CubeRound(HexToCube(hex)));
            public static Vector3Int CubeRound(Vector3 cube) {
                int rx = Mathf.RoundToInt(cube.x);
                int ry = Mathf.RoundToInt(cube.y);
                int rz = Mathf.RoundToInt(cube.z);

                float x_diff = Mathf.Abs(rx - cube.x);
                float y_diff = Mathf.Abs(ry - cube.y);
                float z_diff = Mathf.Abs(rz - cube.z);

                if (x_diff > y_diff && x_diff > z_diff) {
                    rx = -ry - rz;
                }
                else if (y_diff > z_diff) {
                    ry = -rx - rz;
                }
                else {
                    rz = -rx - ry;
                }

                return new Vector3Int(rx, ry, rz);
            }

            public static Vector3 HexToCube(Vector2 hex) {
                var x = hex.x;
                var z = hex.y;
                var y = -x - z;
                return new Vector3(x, y, z);
            }
            public static Vector2 CubeToHex(Vector3 cube) => new Vector2(cube.x, cube.z);
            public static Vector3Int HexToCube(Vector2Int hex) {
                var x = hex.x;
                var z = hex.y;
                var y = -x - z;
                return new Vector3Int(x, y, z);
            }
            public static Vector2Int CubeToHex(Vector3Int cube) => new Vector2Int(cube.x, cube.z);

            public static List<Vector2Int> HexPointList(RectInt limits) => limits.PointList();
            public static List<Vector3Int> CubePointList(Rect3Int limits) => HexPointList(new RectInt() {
                xMin = limits.Min.x,
                xMax = limits.Max.x,
                yMin = limits.Min.z,
                yMax = limits.Max.z,
            }).ConvertAll(HexToCube).FindAll(c => c.y >= limits.Min.y && c.y < limits.Max.y);

            public static List<Vector2Int> HexBoundPoints(RectInt limits) => limits.BoundPoints();
            public static List<Vector3Int> CubeBoundPoints(Rect3Int limits) => CubePointList(limits).FindAll(
                c => c.x == limits.Min.x || c.x == limits.Max.x
                 || c.y == limits.Min.y || c.y == limits.Max.y
                 || c.z == limits.Min.z || c.z == limits.Max.z
            );

            public static Vector3Int CubeClamped(Vector3Int cube, Rect3Int limits) => CubeBoundPoints(limits).MinElement(c => c.DistanceTo(cube));

            public static Vector3 CubeFromXY(float x, float y) => new(x, y, -x - y);
            public static Vector3 CubeFromXZ(float x, float z) => new(x, -x - z, z);
            public static Vector3 CubeFromYZ(float y, float z) => new(-y - z, y, z);
            public static Vector3Int CubeFromXY(int x, int y) => new(x, y, -x - y);
            public static Vector3Int CubeFromXZ(int x, int z) => new(x, -x - z, z);
            public static Vector3Int CubeFromYZ(int y, int z) => new(-y - z, y, z);

            public static List<Vector3> CubeXLineFromY(float x, float yMin, float yMax) => new List<Vector3>() { CubeFromXY(x, yMin), CubeFromXY(x, yMax) };
            public static List<Vector3> CubeXLineFromZ(float x, float zMin, float zMax) => new List<Vector3>() { CubeFromXZ(x, zMin), CubeFromXZ(x, zMax) };
            public static List<Vector3> CubeYLineFromX(float y, float xMin, float xMax) => new List<Vector3>() { CubeFromXY(xMin, y), CubeFromXY(xMax, y) };
            public static List<Vector3> CubeYLineFromZ(float y, float zMin, float zMax) => new List<Vector3>() { CubeFromYZ(y, zMin), CubeFromYZ(y, zMax) };
            public static List<Vector3> CubeZLineFromX(float z, float xMin, float xMax) => new List<Vector3>() { CubeFromXZ(xMin, z), CubeFromXZ(xMax, z) };
            public static List<Vector3> CubeZLineFromY(float z, float yMin, float yMax) => new List<Vector3>() { CubeFromYZ(yMin, z), CubeFromYZ(yMax, z) };
            public static List<Vector3Int> CubeXLineFromY(int x, int yMin, int yMax) => new List<Vector3Int>() { CubeFromXY(x, yMin), CubeFromXY(x, yMax) };
            public static List<Vector3Int> CubeXLineFromZ(int x, int zMin, int zMax) => new List<Vector3Int>() { CubeFromXZ(x, zMin), CubeFromXZ(x, zMax) };
            public static List<Vector3Int> CubeYLineFromX(int y, int xMin, int xMax) => new List<Vector3Int>() { CubeFromXY(xMin, y), CubeFromXY(xMax, y) };
            public static List<Vector3Int> CubeYLineFromZ(int y, int zMin, int zMax) => new List<Vector3Int>() { CubeFromYZ(y, zMin), CubeFromYZ(y, zMax) };
            public static List<Vector3Int> CubeZLineFromX(int z, int xMin, int xMax) => new List<Vector3Int>() { CubeFromXZ(xMin, z), CubeFromXZ(xMax, z) };
            public static List<Vector3Int> CubeZLineFromY(int z, int yMin, int yMax) => new List<Vector3Int>() { CubeFromYZ(yMin, z), CubeFromYZ(yMax, z) };

            public static List<Vector3> CubeXLine(float x, float yMin, float yMax, float zMin, float zMax) {
                List<Vector3> fromY = CubeXLineFromY(x, yMin, yMax);
                List<Vector3> fromZ = CubeXLineFromZ(x, zMin, zMax);
                if (fromY[1].z > fromZ[1].z || fromY[0].z < fromZ[0].z) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromY[0].z < fromZ[1].z) ? fromY[0] : fromZ[1],
                    (fromY[1].z > fromZ[0].z) ? fromY[1] : fromZ[0]
                };
            }
            public static List<Vector3> CubeYLine(float y, float xMin, float xMax, float zMin, float zMax) {
                List<Vector3> fromX = CubeYLineFromX(y, xMin, xMax);
                List<Vector3> fromZ = CubeYLineFromZ(y, zMin, zMax);
                if (fromX[1].z > fromZ[1].z || fromX[0].z < fromZ[0].z) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromX[0].z < fromZ[1].z) ? fromX[0] : fromZ[1],
                    (fromX[1].z > fromZ[0].z) ? fromX[1] : fromZ[0]
                };
            }
            public static List<Vector3> CubeZLine(float z, float xMin, float xMax, float yMin, float yMax) {
                List<Vector3> fromX = CubeZLineFromX(z, xMin, xMax);
                List<Vector3> fromY = CubeZLineFromY(z, yMin, yMax);
                if (fromX[1].y > fromY[1].y || fromX[0].y < fromY[0].y) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromX[0].y < fromY[1].y) ? fromX[0] : fromY[1],
                    (fromX[1].y > fromY[0].y) ? fromX[1] : fromY[0]
                };
            }
            public static List<Vector3Int> CubeXLine(int x, int yMin, int yMax, int zMin, int zMax) {
                List<Vector3Int> fromY = CubeXLineFromY(x, yMin, yMax);
                List<Vector3Int> fromZ = CubeXLineFromZ(x, zMin, zMax);
                if (fromY[1].z > fromZ[1].z || fromY[0].z < fromZ[0].z) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromY[0].z < fromZ[1].z) ? fromY[0] : fromZ[1],
                    (fromY[1].z > fromZ[0].z) ? fromY[1] : fromZ[0]
                };
            }
            public static List<Vector3Int> CubeYLine(int y, int xMin, int xMax, int zMin, int zMax) {
                List<Vector3Int> fromX = CubeYLineFromX(y, xMin, xMax);
                List<Vector3Int> fromZ = CubeYLineFromZ(y, zMin, zMax);
                if (fromX[1].z > fromZ[1].z || fromX[0].z < fromZ[0].z) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromX[0].z < fromZ[1].z) ? fromX[0] : fromZ[1],
                    (fromX[1].z > fromZ[0].z) ? fromX[1] : fromZ[0]
                };
            }
            public static List<Vector3Int> CubeZLine(int z, int xMin, int xMax, int yMin, int yMax) {
                List<Vector3Int> fromX = CubeZLineFromX(z, xMin, xMax);
                List<Vector3Int> fromY = CubeZLineFromY(z, yMin, yMax);
                if (fromX[1].y > fromY[1].y || fromX[0].y < fromY[0].y) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromX[0].y < fromY[1].y) ? fromX[0] : fromY[1],
                    (fromX[1].y > fromY[0].y) ? fromX[1] : fromY[0]
                };
            }

            public static List<Vector3> CubeBoundXLines(Rect3 rect) {
                List<Vector3> result = new();
                result.AddRange(CubeXLine(rect.Max.x, rect.Min.y, rect.Max.y, rect.Min.z, rect.Max.z));
                result.AddRange(CubeXLine(rect.Min.x, rect.Min.y, rect.Max.y, rect.Min.z, rect.Max.z));
                return result;
            }
            public static List<Vector3> CubeBoundYLines(Rect3 rect) {
                List<Vector3> result = new();
                result.AddRange(CubeYLine(rect.Max.y, rect.Min.x, rect.Max.x, rect.Min.z, rect.Max.z));
                result.AddRange(CubeYLine(rect.Min.y, rect.Min.x, rect.Max.x, rect.Min.z, rect.Max.z));
                return result;
            }
            public static List<Vector3> CubeBoundZLines(Rect3 rect) {
                List<Vector3> result = new();
                result.AddRange(CubeZLine(rect.Max.z, rect.Min.x, rect.Max.x, rect.Min.y, rect.Max.y));
                result.AddRange(CubeZLine(rect.Min.z, rect.Min.x, rect.Max.x, rect.Min.y, rect.Max.y));
                return result;
            }
            public static List<Vector3Int> CubeBoundXLines(Rect3Int rect) {
                List<Vector3Int> result = new();
                result.AddRange(CubeXLine(rect.Max.x - 1, rect.Min.y, rect.Max.y - 1, rect.Min.z, rect.Max.z - 1));
                result.AddRange(CubeXLine(rect.Min.x, rect.Min.y, rect.Max.y - 1, rect.Min.z, rect.Max.z - 1));
                return result;
            }
            public static List<Vector3Int> CubeBoundYLines(Rect3Int rect) {
                List<Vector3Int> result = new();
                result.AddRange(CubeYLine(rect.Max.y - 1, rect.Min.x, rect.Max.x - 1, rect.Min.z, rect.Max.z - 1));
                result.AddRange(CubeYLine(rect.Min.y, rect.Min.x, rect.Max.x - 1, rect.Min.z, rect.Max.z - 1));
                return result;
            }
            public static List<Vector3Int> CubeBoundZLines(Rect3Int rect) {
                List<Vector3Int> result = new();
                result.AddRange(CubeZLine(rect.Max.z - 1, rect.Min.x, rect.Max.x - 1, rect.Min.y, rect.Max.y - 1));
                result.AddRange(CubeZLine(rect.Min.z, rect.Min.x, rect.Max.x - 1, rect.Min.y, rect.Max.y - 1));
                return result;
            }

            public static List<Vector2Int> hexDirections = new() {
                new Vector2Int(1, -1),
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1)
            };

            public static float HexDist(Vector2 h1, Vector2 h2) => CubeDist(HexToCube(h1), HexToCube(h2));
            public static float CubeDist(Vector3 a, Vector3 b) => Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
            public static int HexDist(Vector2Int h1, Vector2Int h2) => CubeDist(HexToCube(h1), HexToCube(h2));
            public static int CubeDist(Vector3Int a, Vector3Int b) => Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));

            public static List<Vector2Int> RasterizeLine(Vector2Int start, Vector2Int end) {
                int n = HexDist(start, end);
                return new (Enumerable.Range(0, n + 1).Select(i => HexRound(Vector2.Lerp(start, end, (float)i / n))));
            }


            public enum HexCostEvaluation {
                OnLeave,
                OnEnter
            }
            public static bool CanReach(Predicate<Vector2Int> isHexValid, Vector2Int fromHex, Vector2Int toHex) {
                List<Vector2Int> visitedNodes = new() { fromHex };
                List<int> borderNodes = new() { 0 };
                while (borderNodes.Count > 0) {
                    int index = borderNodes[0];
                    if (visitedNodes[index] == toHex) {
                        return true;
                    }
                    foreach (Vector2Int neighbor in hexDirections) {
                        if (isHexValid.Invoke(visitedNodes[index]) || !visitedNodes.Contains(neighbor + visitedNodes[index])) {
                            borderNodes.Add(visitedNodes.Count);
                            visitedNodes.Add(neighbor + visitedNodes[index]);
                        }
                    }
                    borderNodes.RemoveAt(0);
                }
                return false;
            }
            public static bool CanReach(Predicate<Vector2Int> isHexValid, Vector2Int fromHex, Vector2Int toHex, int maxHops) {
                return GraphSolver.CanReach(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), fromHex, toHex, maxHops);
            }
            public static bool CanReach(Predicate<Vector2Int> isHexValid, Func<Vector2Int, float> cost, Vector2Int fromHex, Vector2Int toHex, float maxCost, HexCostEvaluation hexCostEvaluation) {
                return GraphSolver.CanReach(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), (f, t) => {
                    return hexCostEvaluation switch {
                        HexCostEvaluation.OnLeave => cost.Invoke(f),
                        HexCostEvaluation.OnEnter => cost.Invoke(t),
                        _ => default,
                    };
                }, fromHex, toHex, maxCost);
            }
            public static List<Vector2Int> ReachableNodes(Predicate<Vector2Int> isHexValid, Vector2Int start) {
                return GraphSolver.ReachableNodes(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), start);
            }
            public static List<Vector2Int> ReachableNodes(Predicate<Vector2Int> isHexValid, Vector2Int start, int maxHops) {
                return GraphSolver.ReachableNodes(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), start, maxHops);
            }
            public static List<Vector2Int> ReachableNodes(Predicate<Vector2Int> isHexValid, Func<Vector2Int, float> cost, Vector2Int start, float maxCost, HexCostEvaluation hexCostEvaluation) {
                return GraphSolver.ReachableNodes(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), (f, t) => {
                    return hexCostEvaluation switch {
                        HexCostEvaluation.OnLeave => cost.Invoke(f),
                        HexCostEvaluation.OnEnter => cost.Invoke(t),
                        _ => default,
                    };
                }, start, maxCost);
            }
            public static List<Vector2Int> BFS_PathFinding(Predicate<Vector2Int> isHexValid, Vector2Int fromHex, Predicate<Vector2Int> validFinal) {
                List<Vector2Int> visitedNodes = new() { fromHex };
                List<int> parentIDs = new() { -1 };
                List<int> borderNodeIDs = new() { 0 };
                while (borderNodeIDs.Count > 0) {
                    int cur = borderNodeIDs[0];
                    if (validFinal.Invoke(visitedNodes[cur])) {
                        List<Vector2Int> result = new();
                        while (cur >= 0) {
                            result.Add(visitedNodes[cur]);
                            cur = parentIDs[cur];
                        }
                        result.Reverse();
                        return result;
                    }
                    foreach (Vector2Int neighbor in hexDirections) {
                        if (isHexValid.Invoke(visitedNodes[cur]) || !visitedNodes.Contains(neighbor + visitedNodes[cur])) {
                            borderNodeIDs.Add(visitedNodes.Count);
                            parentIDs.Add(cur);
                            visitedNodes.Add(neighbor + visitedNodes[cur]);
                        }
                    }
                    borderNodeIDs.RemoveAt(0);
                }
                return null;
            }
            public static List<Vector2Int> BFS_PathFinding(Predicate<Vector2Int> isHexValid, Vector2Int fromHex, Vector2Int toHex) {
                List<Vector2Int> visitedNodes = new() { fromHex };
                List<int> parentIDs = new() { -1 };
                List<int> borderNodeIDs = new() { 0 };
                while (borderNodeIDs.Count > 0) {
                    int cur = borderNodeIDs[0];
                    if (visitedNodes[cur].Equals(toHex)) {
                        List<Vector2Int> result = new();
                        while (cur >= 0) {
                            result.Add(visitedNodes[cur]);
                            cur = parentIDs[cur];
                        }
                        result.Reverse();
                        return result;
                    }
                    foreach (Vector2Int neighbor in hexDirections) {
                        if (isHexValid.Invoke(visitedNodes[cur]) || !visitedNodes.Contains(neighbor + visitedNodes[cur])) {
                            borderNodeIDs.Add(visitedNodes.Count);
                            parentIDs.Add(cur);
                            visitedNodes.Add(neighbor + visitedNodes[cur]);
                        }
                    }
                    borderNodeIDs.RemoveAt(0);
                }
                return null;
            }
            public static List<Vector2Int> Dijkstra_PathFinding(Predicate<Vector2Int> isHexValid, Func<Vector2Int, float> cost, Vector2Int fromHex, Predicate<Vector2Int> validFinal, HexCostEvaluation hexCostEvaluation) {
                return GraphSolver.Dijkstra_PathFinding(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), (f, t) => {
                    return hexCostEvaluation switch {
                        HexCostEvaluation.OnLeave => cost.Invoke(f),
                        HexCostEvaluation.OnEnter => cost.Invoke(t),
                        _ => default,
                    };
                }, fromHex, validFinal);
            }
            public static List<Vector2Int> Dijkstra_PathFinding(Predicate<Vector2Int> isHexValid, Func<Vector2Int, float> cost, Vector2Int fromHex, Vector2Int toHex, HexCostEvaluation hexCostEvaluation) {
                return GraphSolver.Dijkstra_PathFinding(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), (f, t) => {
                    return hexCostEvaluation switch {
                        HexCostEvaluation.OnLeave => cost.Invoke(f),
                        HexCostEvaluation.OnEnter => cost.Invoke(t),
                        _ => default,
                    };
                }, fromHex, toHex);
            }
            public static List<Vector2Int> AStar_PathFinding(Predicate<Vector2Int> isHexValid, Func<Vector2Int, float> cost, Vector2Int fromHex, Vector2Int toHex, HexCostEvaluation hexCostEvaluation) {
                return GraphSolver.AStar_PathFinding(h => hexDirections.ConvertAll(d => h + d).FindAll(isHexValid), (f, t) => {
                    return hexCostEvaluation switch {
                        HexCostEvaluation.OnLeave => cost.Invoke(f),
                        HexCostEvaluation.OnEnter => cost.Invoke(t),
                        _ => default,
                    };
                }, h => HexDist(h, toHex), fromHex, toHex);
            }

            #endregion

        }

        #endregion
        #endregion

        #endregion

        #region 3D
        #region Lines
        public static Vector3 ClosestPointOnLineSegment((Vector3 p0, Vector3 p1) segment, Vector3 point) {
            Vector3 dir = (segment.p1 - segment.p0).normalized;
            Vector3 relativePoint = point - segment.p0;
            return segment.p0 + dir * Mathf.Clamp(Vector3.Dot(relativePoint, dir), 0f, (segment.p1 - segment.p0).magnitude);
        }
        public static bool RayPlaneIntersection(Ray ray, Plane plane, out Vector3 intersection) {
            bool result = plane.Raycast(ray, out float enter);
            intersection = ray.origin + ray.direction * enter;
            return result;
        }
        #endregion

        #region Polyhedrons
        public static partial class RectangularCuboid {
            public static float Volume(Vector3 size) => size.x * size.y * size.z;
            public static int Volume(Vector3Int size) => size.x * size.y * size.z;
        }

        #endregion
        #endregion

    }
}
