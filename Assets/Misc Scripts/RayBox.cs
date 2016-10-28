using UnityEngine;
using System.Collections;

public class RayBox{

    public Vector3 cornerOne;
    public Vector3 cornerTwo;
    public Vector3 cornerThree;
    public Vector3 cornerFour;

    public Vector3 midPointOneThreeX;
    public Vector3 midPointOneFourZ;
    public Vector3 center;

    public RayBox(Vector3 posOne, Vector3 posTwo)
    {
        //Generate all four corners
        cornerOne = posOne;
        cornerTwo = new Vector3(posTwo.x, cornerOne.y, posTwo.z);
        cornerThree = new Vector3(cornerTwo.x, cornerOne.y, cornerOne.z);
        cornerFour = new Vector3(cornerOne.x, cornerOne.y, cornerTwo.z);

        //Disect
        float halfDistOneThreeX = (Vector3.Distance(cornerOne, cornerThree) / 2) * (cornerOne.x < cornerTwo.x ? 1 : -1);
        float halfDistOneFourZ = (Vector3.Distance(cornerOne, cornerFour) / 2) * (cornerOne.z < cornerTwo.z ? 1 : -1);

        center = new Vector3(cornerOne.x + halfDistOneThreeX, cornerOne.y, cornerOne.z + halfDistOneFourZ);
        midPointOneThreeX = new Vector3(cornerOne.x + halfDistOneThreeX, cornerOne.y, cornerOne.z);
        midPointOneFourZ = new Vector3(cornerOne.x, cornerOne.y, cornerOne.z + halfDistOneFourZ);

    }

    public Collider[] drawOverlapBox()
    {
        //Vector3 halfExtents = new Vector3(Vector3.Distance(center, midPointOneThreeX), 1, Vector3.Distance(center, midPointOneFourZ));
        Vector3 halfExtents = new Vector3(Vector3.Distance(center, midPointOneFourZ), 1, Vector3.Distance(center, midPointOneThreeX));
        return Physics.OverlapBox(center, halfExtents);
    }

    public void drawBox()
    {
        //Draw Sides
        Debug.DrawLine(cornerOne, cornerThree);
        Debug.DrawLine(cornerOne, cornerFour);
        Debug.DrawLine(cornerTwo, cornerThree);
        Debug.DrawLine(cornerTwo, cornerFour);

        //Draw Dissection
        Debug.DrawLine(center, midPointOneThreeX);
        Debug.DrawLine(center, midPointOneFourZ);
    }

    public static class Utils
    {
        static Texture2D _whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        
        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            // Top
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }
}
