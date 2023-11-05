using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothPointGenerator
{
    //convert tile list into vector2 list to smooth
    public static List<Vector2> ConvertTileListToVector(List<Tile> tileList)
    {
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0; i < tileList.Count; i++)
        {
            temp.Add(new Vector2(tileList[i].x, tileList[i].y));
        }
        return temp;
    }
    //rolling averages smooth the list of points, ignoring first and last point in list
    public static List<Vector2> CreateSmoothPath(List<Vector2> points, int windowSize = 3)
    {
        List<Vector2> smoothedPoints = new List<Vector2>();
        smoothedPoints.Add(points[0]);
        for (int i = 1; i < points.Count-1; i++)
        {
            float sumX = 0;
            float sumY = 0;

            for (int j = i - windowSize / 2; j <= i + windowSize / 2; j++)
            {
                if (j >= 0 && j < points.Count)
                {
                    sumX += points[j].x;
                    sumY += points[j].y;
                }
            }

            float averageX = sumX / windowSize;
            float averageY = sumY / windowSize;

            smoothedPoints.Add(new Vector2(averageX, averageY));
        }
        smoothedPoints.Add(points[points.Count - 1]);
        return smoothedPoints;
    }
}

