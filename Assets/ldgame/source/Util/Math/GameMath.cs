using System.Collections.Generic;
using UnityEngine;

namespace Engine.Math
{
    public enum Facing
    {
        Left,
        Right,
        Up,
        Down,
    }

    public enum CardinalDirection
    {
        North,
        East,
        South,
        West,
    }

    public static class GameMath
    {
        public static Vector2 DirectionToVector(CardinalDirection dir)
        {
            switch (dir)
            {
                case CardinalDirection.East:
                    return new Vector2(1f, 0f);
                case CardinalDirection.South:
                    return new Vector2(0f, -1f);
                case CardinalDirection.West:
                    return new Vector2(-1f, 0f);
                default:
                    return new Vector2(0, 1f);
            }
        }

        public static Vector2 VectorFromFacing(Facing facing)
        {
            switch (facing)
            {
                case Facing.Down:
                    return Vector2.down;
                case Facing.Left:
                    return Vector2.left;
                case Facing.Right:
                    return Vector2.right;
                default:
                    return Vector2.up;
            }
        }

        public static float GetAngleOnCircle(Vector2 center, Vector2 point)
        {
            float opposite = Vector2.Distance(point, new Vector2(center.x, point.y));
            float adjacent = Vector2.Distance((Vector2)center, new Vector2(center.x, point.y));

            int quadrant = GetQuadrant(center, point);
            float mouseAngle = (quadrant == 2 || quadrant == 4 ? Mathf.PI * 0.5f - Mathf.Atan2(adjacent, opposite) : Mathf.Atan2(adjacent, opposite)) + (quadrant - 1) * Mathf.PI * 0.5f;

            return mouseAngle;
        }

        // Converts a 2D direction vector to an angle in degrees
        public static float Vector2ToAngle(Vector2 direction)
        {
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            return angleDegrees;
        }

        public static int GetQuadrant(Vector2 circleCenter, Vector2 point)
        {
            if (point.x > circleCenter.x && point.y > circleCenter.y)
                return 1;
            else if (point.x > circleCenter.x && point.y < circleCenter.y)
                return 4;
            else if (point.x < circleCenter.x && point.y < circleCenter.y)
                return 3;
            else
                return 2;
        }

        public static float AngleDelta(float angle1, float angle2)
        {
            float w1 = angle1 > 180f ? angle1 - 360f : angle1;
            float w2 = angle2 > 180f ? angle2 - 360f : angle2;

            return Mathf.Abs(w1 - w2);
        }

        public static int DirectionLeftOrRight(Vector3 forward, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(forward, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0f)
            {
                return 1; // Right
            }
            else if (dir < 0f)
            {
                return -1; // Left
            }
            else
            {
                return 0;
            }
        }

        public static Vector2 XZ(this Vector3 vec3D)
        {
            return new Vector2(vec3D.x, vec3D.z);
        }

        public static Vector2 XY(this Vector3 vec3D)
        {
            return new Vector2(vec3D.x, vec3D.y);
        }

        public static Color SetAlpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static Color ParseColor(this string str)
        {
            if (ColorUtility.TryParseHtmlString(str, out var clr))
                return clr;
            return UnityEngine.Color.magenta;
        }

        public static string Color(this string str, Color c)
        {
            return str.Color("#" + ColorUtility.ToHtmlStringRGBA(c));
        }

        public static string Color(this string str, string c)
        {
            string coloredString = "<color=" + c + ">";
            coloredString += str;
            coloredString += "</color>";
            return coloredString;
        }

        public static Vector2 AngleToVector2(float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
        }

        public static Vector2 DirectionToMouse2DRaw(Vector2 from, Camera cam = null)
        {
            Vector2 mousePosition = MousePos2D(cam);
            return (mousePosition - from);
        }

        public static Vector2 DirectionToMouse2D(Vector2 from, Camera cam = null)
        {
            Vector2 mousePosition = MousePos2D(cam);
            return (mousePosition - from).normalized;
        }

        public static Vector3 MousePos2D(Camera cam = null)
        {
            cam ??= Camera.main;
            return cam.ScreenToWorldPoint(Input.mousePosition);
        }

        public static float AngleToMouse2D(Vector2 from, Camera cam = null)
        {
            return AngleToPos2D(from, MousePos2D(cam));
        }

        public static float AngleToPos2D(Vector2 from, Vector2 to)
        {
            var direction = Direction(from, to);
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            return (angleDegrees + 360) % 360;
        }

        public static Quaternion RandomRotation2D()
        {
            return Quaternion.Euler(0, 0, Random.Range(0, 360f));
        }

        public static Vector3 Direction(Vector3 origin, Vector3 target)
        {
            return (target - origin).normalized;
        }

        public static void MoveTowards(this Transform origin, Vector3 target, float speed)
        {
            origin.transform.position = Vector3.MoveTowards(origin.position, target, speed);
        }

        public static bool IsColliding(Vector3 transformPosition, Vector3 position, float f)
        {
            return Vector2.Distance(transformPosition, position) < f;
        }

        public static T IsColliding<T>(Vector3 transformPosition, List<T> any, float f) where T : Component
        {
            foreach (var e in any)
                if (Vector2.Distance(transformPosition, e.transform.position) < f)
                    return e;
            return default(T);
        }

        // Function to convert canvas position to world position
        public static Vector3 CanvasToWorld(RectTransform target)
        {
            // Convert canvas position to screen space
            var componentInParent = target.GetComponentInParent<Canvas>();
            var rectTransform = componentInParent.GetComponent<RectTransform>();

            Vector2 screenPosition = RectTransformUtility.PixelAdjustPoint(target.position, target, componentInParent);

            // Convert screen space to world space
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPosition, Camera.main, out worldPosition);
            return worldPosition;
        }

        public static float Inverse01(float damage)
        {
            return 1f - Mathf.Clamp01(damage);
        }


        // Method to find the closest transform to a given position and return both the transform and the distance
        public static (Transform, float) FindClosest<T>(List<T> transforms, Vector2 pos) where T : Component
        {
            Transform closestTransform = null;
            float minDistance = float.MaxValue; // Initialize with the maximum possible float value

            foreach (T t in transforms)
            {
                float distance = Vector2.Distance(t.transform.position, pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTransform = t.transform;
                }
            }

            return (closestTransform, minDistance);
        }

        public static Vector3 MouseCameraOffset2D()
        {
            // Get the mouse position
            Vector2 mousePosition = Input.mousePosition;

            // Calculate the offset from the center of the screen
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 offset = mousePosition - screenCenter;

            // Normalize the offset to the range of -0.5 to 0.5
            Vector2 normalizedOffset = new Vector2(offset.x / Screen.width, offset.y / Screen.height);

            return normalizedOffset;
        }

        public static Vector3 GetInputAxis2D()
        {
            var inputAxis2D = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            return inputAxis2D.normalized;
        }

        // Method to generate an arc based on a direction vector
        public static List<Vector2> Arc(Vector2 origin, float radius, Vector2 direction, int numberOfPoints)
        {
            List<Vector2> points = new List<Vector2>();

            // Normalize the direction vector
            direction.Normalize();

            // Compute the angle in radians from the direction vector
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Calculate the range of the arc based on the radius
            float verticalRange = radius / 2;

            // Determine the angles for the start and end of the arc
            float startAngle = angle - Mathf.Asin(verticalRange / radius);
            float endAngle = angle + Mathf.Asin(verticalRange / radius);

            // Angle increment per point
            float angleIncrement = (endAngle - startAngle) / (numberOfPoints - 1);

            for (int i = 0; i < numberOfPoints; i++)
            {
                // Current angle in radians
                float currentAngle = startAngle + angleIncrement * i;
                // Calculate the position of the point
                float x = origin.x + radius * Mathf.Cos(currentAngle);
                float y = origin.y + radius * Mathf.Sin(currentAngle);
                points.Add(new Vector2(x, y));
            }

            return points;
        }

        // Method to generate a full 360-degree arc (circle)
        public static List<Vector2> FullCircle(Vector2 origin, int numberOfPoints, List<Vector2> points = null, float time = 0, float r = 1)
        {
            points ??= new List<Vector2>();

            var radius = r;
            // Calculate the angle increment for each point in radians
            float angleIncrement = 2 * Mathf.PI / numberOfPoints;

            for (int i = 0; i < numberOfPoints; i++)
            {
                // Calculate the current angle
                float currentAngle = angleIncrement * i;

                // Compute x and y using the current angle
                float x = origin.x + radius * Mathf.Cos(time + currentAngle);
                float y = origin.y + radius * Mathf.Sin(time + currentAngle);

                // Add the point to the list
                points.Add(new Vector2(x, y));
            }

            return points;
        }

        // Extension method to enable or disable emission of a ParticleSystem
        public static void SetEmission(this ParticleSystem particleSystem, bool enabled)
        {
            // Access the emission module of the particle system
            var emission = particleSystem.emission;
            // Set the enabled state of the emission module
            emission.enabled = enabled;
        }

        public static bool Chance(double d)
        {
            if (Input.GetKey(KeyCode.Tab))
                return true;
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Tab))
                return true;
            return Random.Range(0f, 1f) < d;
        } // Method to check if the given position is on screen

    
        public static Vector2 MousePositionToRectTransform(this RectTransform rectTransform, Camera uiCamera)
        {
            Vector2 localPoint;

            // Convert mouse position to world point relative to the RectTransform's canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, 
                Input.mousePosition, 
                uiCamera, 
                out localPoint
            );

            return localPoint;
        }
        
        public static bool IsOnScreen(Vector3 position)
        {
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);

            // Check if the viewportPoint is within the screen bounds
            bool onScreen = viewportPoint.x >= 0 && viewportPoint.x <= 1
                                                 && viewportPoint.y >= 0 && viewportPoint.y <= 1
                                                 && viewportPoint.z > 0; // z > 0 indicates the point is in front of the camera

            return onScreen;
        }

        public static string FormatSeconds(int seconds)
        {
            // Calculate minutes and the remaining seconds
            int minutes = seconds / 60;
            int remainingSeconds = seconds % 60;

            // Return formatted time
            return string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        }

        public static int RandomDirection()
        {
            return Chance(0.5f) ? -1 : 1;
        }
        
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static Color Rainbow(float time)
        {
            // Calculate the red, green, and blue components
            float red = Mathf.Sin(time * 0.5f + 0) * 0.5f + 0.5f;
            float green = Mathf.Sin(time * 0.5f + 2 * Mathf.PI / 3) * 0.5f + 0.5f;
            float blue = Mathf.Sin(time * 0.5f + 4 * Mathf.PI / 3) * 0.5f + 0.5f;
        
            // Create and return a new Color with the calculated components
            return new Color(red, green, blue);
        }
    }
}