using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public static class RasterizationSolver {

        #region 2D
        public static List<Vector2Int> RasterizeLine(Vector2Int from, Vector2Int to, int thickness) {
            List<Vector2Int> result = new();
            RasterizeLine(from, to).ForEach(v => result.AddRangeIfDoesntContain(RasterizeCircle(thickness, v)));
            return result;
        }
        public static List<Vector2Int> RasterizeLine(Vector2Int from, Vector2Int to, DiagonalRasterizationType diagonalRasterizationType = DiagonalRasterizationType.GoStraight) {
            return RasterizeLineToGroups(from, to, diagonalRasterizationType).ToSingleList();
        }
        public static List<List<Vector2Int>> RasterizeLineToGroups(Vector2Int from, Vector2Int to, DiagonalRasterizationType diagonalRasterizationType = DiagonalRasterizationType.GoStraight) {
            List<List<Vector2Int>> result = new List<List<Vector2Int>>();
            int yMin = Mathf.Min(from.y, to.y);
            int yMax = Mathf.Max(from.y, to.y);
            int xMin = Mathf.Min(from.x, to.x);
            int xMax = Mathf.Max(from.x, to.x);
            int xSignal = (from.x < to.x) ? 1 : -1;
            int ySignal = (from.y < to.y) ? 1 : -1;
            float yDelta = yMax - yMin;
            float xDelta = xMax - xMin;
            if (from == to) {
                result.Add(new List<Vector2Int>() { from });
            }
            else if (from.x == to.x) {
                for (int i = 0; i <= yDelta; i++) {
                    result.Add(new List<Vector2Int>() { new Vector2Int(from.x, yMin + i) });
                }
            }
            else if (from.y == to.y) {
                for (int i = 0; i <= xDelta; i++) {
                    result.Add(new List<Vector2Int>() { new Vector2Int(xMin + i, from.y) });
                }
            }
            else {
                if (xDelta == yDelta) {
                    for (int i = 0; i <= xDelta; i++) {
                        Vector2Int nextPoint = new Vector2Int(from.x + (i * xSignal), from.y + (i * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
                else if (xDelta > yDelta) {
                    for (int i = 0; i <= xDelta; i++) {
                        int curY = Mathf.RoundToInt(i * yDelta / xDelta);
                        Vector2Int nextPoint = new Vector2Int(from.x + (i * xSignal), from.y + (curY * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0 && nextPoint.x != result.Last().Last().x && nextPoint.y != result.Last().Last().y) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
                else {
                    for (int i = 0; i <= yDelta; i++) {
                        int curX = Mathf.RoundToInt(i * xDelta / yDelta);
                        Vector2Int nextPoint = new Vector2Int(from.x + (curX * xSignal), from.y + (i * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0 && nextPoint.x != result.Last().Last().x && nextPoint.y != result.Last().Last().y) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
            }
            return result;
        }

        public static List<Vector2Int> RasterizeFloatingLine(Vector2 from, Vector2 to, DiagonalRasterizationType diagonalRasterizationType) {
            List<Vector2Int> result = new();
            Vector2Int fromInt = from.RoundedToInt();
            Vector2Int toInt = to.RoundedToInt();
            Vector2Int intMin = Vector2Int.Min(fromInt, toInt);
            Vector2Int intMax = Vector2Int.Max(fromInt, toInt);
            Vector2Int sign = new((toInt.x - fromInt.x).Sign(), (toInt.y - fromInt.y).Sign());
            Vector2 delta = to - from;
            Vector2Int intSize = intMax - intMin;
            if (fromInt == toInt) {
                result.Add(fromInt);
            }
            else if (fromInt.x == toInt.x) {
                for (int i = 0; i <= intSize.y; i++) {
                    result.Add(new(fromInt.x, intMin.x + i));
                }
            }
            else if (fromInt.y == toInt.y) {
                for (int i = 0; i <= intSize.x; i++) {
                    result.Add(new(intMin.x + i, fromInt.y));
                }
            }
            else {
                List<float> verticalLineIntersectionDists = new(Enumerable.Range(0, intSize.x - 1).Select(index => Geometry.VerticalLine(from.x + (index + 0.5f) * sign.x, from.y, to.y)).ConvertAll(l => from.DistanceTo(Geometry.LineLineIntersection(l[0], l[1], from, to).Value)));
                List<float> horizontalLineIntersectionDists = new(Enumerable.Range(0, intSize.y - 1).Select(index => Geometry.HorizontalLine(from.y + (index + 0.5f) * sign.y, from.x, to.x)).ConvertAll(l => from.DistanceTo(Geometry.LineLineIntersection(l[0], l[1], from, to).Value)));
                Vector2Int i = Vector2Int.zero;
                result.Add(fromInt);
                while (i.x < intSize.x - 1 || i.y < intSize.y - 1) {
                    bool moveOnX = false;
                    bool moveOnY = false;
                    if (i.x == intSize.x - 1) {
                        moveOnY = true;
                    }
                    else if (i.y == intSize.y - 1) {
                        moveOnX = true;
                    }
                    else {
                        float nextVertical = verticalLineIntersectionDists[i.x];
                        float nextHorizontal = horizontalLineIntersectionDists[i.y];
                        if (nextVertical < nextHorizontal) {
                            moveOnX = true;
                        }
                        else if (nextVertical > nextHorizontal) {
                            moveOnY = true;
                        }
                        else {
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoStraight:
                                    moveOnX = true;
                                    moveOnY = true;
                                    break;
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    moveOnX = true;
                                    moveOnY = false;
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    result.Add(fromInt + new Vector2Int(i.x * sign.x, i.y * sign.y) + Vector2Int.up);
                                    moveOnX = true;
                                    moveOnY = false;
                                    break;
                            }
                        }
                    }
                    if (moveOnX) {
                        i += Vector2Int.right * sign.x;
                    }
                    if (moveOnY) {
                        i += Vector2Int.up * sign.y;
                    }
                    result.Add(fromInt + new Vector2Int(i.x * sign.x, i.y * sign.y));
                }
            }
            return result;
        }

        public static List<Vector2Int> RasterizeCircle(int radius, Vector2Int center) {
            return RasterizeCircle(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircle(int radius) {
            List<Vector2Int> result = new List<Vector2Int>(RasterizeCircumference(radius));
            result.AddRange(GetPointIsland(Vector2Int.zero, result));
            return result;
        }

        public static List<Vector2Int> RasterizeCircumference(int radius, Vector2Int center, float startAngle, float finishAngle) {
            return RasterizeCircumference(radius, startAngle, finishAngle).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumference(int radius, float startAngle, float finishAngle) {
            List<Vector2Int> result = new List<Vector2Int>();

            int startFullOctant = Mathf.CeilToInt(startAngle / 45f);
            int finishFullOctant = Mathf.FloorToInt(finishAngle / 45f) - 1;
            float leftoverStartAngle = startFullOctant * 45f - startAngle;
            float leftoverFinishAngle = finishAngle - finishFullOctant * 45f;

            if (startFullOctant - 1 <= finishFullOctant) {
                if (leftoverStartAngle > 0f) {
                    result.AddRange(RasterizeCircumferencePartialOctant(radius, startFullOctant - 1, startAngle, startFullOctant * 45f));
                }
                result.AddRangeIfDoesntContain(RasterizeCircumferenceOctants(radius, startFullOctant, finishFullOctant));
                if (leftoverFinishAngle > 0f) {
                    result.AddRangeIfDoesntContain(RasterizeCircumferencePartialOctant(radius, finishFullOctant + 1, finishFullOctant * 45f, finishAngle));
                }
            }
            else {
                result.AddRange(RasterizeCircumferencePartialOctant(radius, startFullOctant - 1, startAngle, finishAngle));
            }

            return result;
        }
        public static List<Vector2Int> RasterizeCircumference(int radius, Vector2Int center) {
            return RasterizeCircumference(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumference(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> semicircumference = RasterizeSemicircumference(radius);
            result.AddRange(semicircumference);
            semicircumference.SetEachElement(v => v.RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(semicircumference);
            return result;
        }

        public static List<Vector2Int> RasterizeSemicircumference(int radius, Vector2Int center) {
            return RasterizeSemicircumference(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeSemicircumference(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> quadrant = RasterizeCircumferenceQuadrant(radius);
            result.AddRange(quadrant);
            quadrant.SetEachElement(v => v.RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(quadrant);
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceQuadrant(int radius, Vector2Int center) {
            return RasterizeCircumferenceQuadrant(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumferenceQuadrant(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> octant = RasterizeCircumferenceOctant(radius);
            result.AddRange(octant);
            octant.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(octant);
            return result;
        }

        public static List<Vector2Int> RasterizeCircumferenceOctants(int radius, int startOctant, int finishOctant) {
            List<Vector2Int> result = RasterizeCircumferenceOctant(radius);
            for (int i = startOctant; i <= finishOctant; i++) {
                result.AddRangeIfDoesntContain(RasterizeCircumferenceOctant(radius, i));
            }
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius, int octantIndex) {
            List<Vector2Int> result = RasterizeCircumferenceOctant(radius);
            if (octantIndex % 2 == 1) {
                result.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            }
            result.SetEachElement(v => v.RotatedBy90Clockwise(octantIndex / 2));
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius, Vector2Int center) {
            return RasterizeCircumferenceOctant(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius) {
            return RasterizeCircumferencePartialOctant(radius, 0f, 45f);
        }

        public static List<Vector2Int> RasterizeCircumferencePartialOctant(int radius, int octantIndex, float startAngle, float finishAngle) {
            List<Vector2Int> result;
            float octantAngle = octantIndex * 45f;
            if (octantIndex % 2 == 1) {
                result = RasterizeCircumferencePartialOctant(radius, finishAngle - octantAngle, startAngle - octantAngle);
                result.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            }
            else {
                result = RasterizeCircumferencePartialOctant(radius, startAngle - octantAngle, finishAngle - octantAngle);
            }
            result.SetEachElement(v => v.RotatedBy90Clockwise(octantIndex / 2));
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferencePartialOctant(int radius, float startAngle, float finishAngle) {
            List<Vector2Int> result = new List<Vector2Int>();
            if (radius == 0) {
                result = new List<Vector2Int>() { Vector2Int.zero };
            }
            else {
                if (radius < 0) {
                    radius = -radius;
                }
                startAngle = Mathf.Clamp(startAngle, 0f, 45f);
                finishAngle = Mathf.Clamp(finishAngle, startAngle, 45f);
                Vector2Int startPoint = Vector2Int.RoundToInt((Vector2.up * radius).RotatedBy(startAngle));
                float curAngle = startAngle;
                int y = startPoint.y;
                int x = startPoint.x;
                while (curAngle <= finishAngle) {
                    result.Add(new Vector2Int(x, y));
                    x++;
                    float distToRadius = Mathf.Abs((new Vector2(x, y)).magnitude - radius);
                    float decDistToRadius = Mathf.Abs((new Vector2(x, y - 1)).magnitude - radius);
                    if (decDistToRadius < distToRadius) {
                        y--;
                    }
                    curAngle = Vector2.Angle(Vector2.up, new Vector2(x, y));
                }
            }
            return result;
        }

        public static List<Vector2Int> GetPointIsland(Vector2Int origin, List<Vector2Int> border) {
            return GetPointIsland(origin, v => border.Contains(v));
        }
        public static List<Vector2Int> GetPointIsland(Vector2Int origin, Predicate<Vector2Int> isBorder) {
            return GraphSolver.ReachableNodes(p => p.Neighbors(), origin, isBorder, true);
        }
        #endregion

    }

    public enum DiagonalRasterizationType {
        GoStraight,
        GoThruOneDirectNeighbor,
        GoThruAllDirectNeighbors
    }

}