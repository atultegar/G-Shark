﻿using GeometrySharp.Geometry;
using System;
using System.Collections.Generic;

namespace GeometrySharp.Core
{
    /// <summary>
    /// Trigonometry provides basic trigonometry methods.
    /// </summary>
    public class Trigonometry
    {
        /// <summary>
        /// Determines if the provide points are on the same plane.
        /// </summary>
        /// <param name="points">Provided points.</param>
        /// <returns>Whether the point are coplanar.</returns>
        public static bool ArePointsCoplanar(IList<Vector3> points)
        {
            // https://en.wikipedia.org/wiki/Triple_product
            if (points.Count < 3)
            {
                return true;
            }

            Vector3 vec1 = points[1] - points[0];
            Vector3 vec2 = points[2] - points[0];

            for (int i = 3; i < points.Count; i++)
            {
                Vector3 vec3 = points[i] - points[0];
                double tripleProduct = Vector3.Dot(Vector3.Cross(vec3, vec2), vec1);
                if (Math.Abs(tripleProduct) > GeoSharpMath.EPSILON)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if three points form a straight line (are collinear) within a given tolerance.
        /// </summary>
        /// <param name="pt1">First point.</param>
        /// <param name="pt2">Second point.</param>
        /// <param name="pt3">Third point.</param>
        /// <param name="tol">Tolerance ser per default as 1e-6</param>
        /// <returns>True if the three points are collinear.</returns>
        public static bool AreThreePointsCollinear(Vector3 pt1, Vector3 pt2, Vector3 pt3, double tol = 1e-6)
        {
            // Find the area of the triangle without using square root and multiply it for 0.5
            // http://www.stumblingrobot.com/2016/05/01/use-cross-product-compute-area-triangles-given-vertices/
            Vector3 pt1ToPt2 = pt2 - pt1;
            Vector3 pt1ToPt3 = pt3 - pt1;
            Vector3 norm = Vector3.Cross(pt1ToPt2, pt1ToPt3);
            double area = Vector3.Dot(norm, norm);

            return area < tol;
        }

        /// <summary>
        /// Finds the closest point on a segment.
        /// The segment is deconstruct in two points and two t values.
        /// </summary>
        /// <param name="point">Point to project.</param>
        /// <param name="segmentPt0">First point of the segment.</param>
        /// <param name="segmentPt1">Second point of the segment.</param>
        /// <param name="valueT0">First t value of the segment.</param>
        /// <param name="valueT1">Second t value of the segment.</param>
        /// <returns>Tuple with the point projected and its t value.</returns>
        public static (double tValue, Vector3 pt) ClosestPointToSegment(Vector3 point, Vector3 segmentPt0,
            Vector3 segmentPt1, double valueT0, double valueT1)
        {
            Vector3 direction = segmentPt1 - segmentPt0;
            double length = direction.Length();

            if (length < GeoSharpMath.EPSILON)
            {
                return (tValue: valueT0, pt: segmentPt0);
            }

            Vector3 vecUnitized = direction.Unitize();
            Vector3 ptToSegPt0 = point - segmentPt0;
            double dotResult = Vector3.Dot(ptToSegPt0, vecUnitized);

            if (dotResult < 0.0)
            {
                return (tValue: valueT0, pt: segmentPt0);
            }
            else if (dotResult > length)
            {
                return (tValue: valueT1, pt: segmentPt1);
            }
            else
            {
                Vector3 pointResult = segmentPt0 + (vecUnitized * dotResult);
                double tValueResult = valueT0 + (valueT1 - valueT0) * dotResult / length;
                return (tValue: tValueResult, pt: pointResult);
            }
        }
    }
}
