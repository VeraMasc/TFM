using System.Collections.Generic;
using UnityEngine;

namespace SimpleMan
{
    public static class Methematics
    {
        public static T GetClosest<T>(Vector3 point, IEnumerable<T> collection) where T : Component
        {
            return GetClosest(point, collection, out float distance);
        }

        public static T GetClosest<T>(Vector3 point, IEnumerable<T> collection, out float distanceToClosestObject) where T : Component
        {
            if (collection == null)
                throw new System.ArgumentNullException("Collection");

            T closestObject = null;
            distanceToClosestObject = float.MaxValue;
            float distance;
            foreach (var item in collection)
            {
                distance = Vector3.Distance(point, item.transform.position);
                if (distance < distanceToClosestObject)
                {
                    distanceToClosestObject = distance;
                    closestObject = item;
                }
            }

            return closestObject;
        }

        public static Vector3 ToXZPlane(this Vector2 target)
        {
            return new Vector3(target.x, 0, target.y);
        }

        public static Vector2[] GetPointsOnCircle(float radius, int pointsCount)
        {
            return GetPointsOnCircle(Vector3.zero, radius, pointsCount);
        }

        public static Vector2[] GetPointsOnCircle(Vector2 center, float radius, int pointsCount)
        {
            Vector2[] points = new Vector2[pointsCount];


            float degrees = 360;
            float angle = 360 / pointsCount;


            for (int i = 0; i < pointsCount; i++)
            {
                points[i] = center + new Vector2((float)Mathf.Cos((degrees) * Mathf.Deg2Rad) * radius, (float)Mathf.Sin((degrees) * Mathf.Deg2Rad) * radius);
                degrees -= angle;
            }


            return points;
        }

        public static Vector2 GetPointOnCircle(float radius, float angle)
        {
            return GetPointOnCircle(Vector2.zero, radius, angle);
        }

        public static Vector2 GetPointOnCircle(Vector2 center, float radius, float angle)
        {
            return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, Mathf.Cos(angle * Mathf.Deg2Rad) * radius) + center;
        }
    }

    public static class CollectionsExtensions
    {
        public static void AddUnique<T>(this List<T> target, T item)
        {
            if (target.Contains(item))
                return;
            else
                target.Add(item);
        }

        public static void AddUnique<TKey, TValue>(this Dictionary<TKey, TValue> target, TKey key, TValue value)
        {
            if (target.ContainsKey(key))
                return;
            else
                target.Add(key, value);
        }
    }

    public static class TransformExtensions
    {
        public static Transform[] GetChildren(this Transform target)
        {
            if (!target)
                throw new System.ArgumentNullException("Target");

            Transform[] children = new Transform[target.childCount];
            for (int i = 0; i < target.childCount; i++)
                children[i] = target.GetChild(i);

            return children;
        }

        public static T[] GetChildrenOfType<T>(this Transform target)
        {
            if (!target)
                throw new System.ArgumentNullException("Target");

            List<T> children = new List<T>();
            for (int i = 0; i < target.childCount; i++)
            {
                if (target.GetChild(i).TryGetComponent(out T component))
                    children.Add(component);
            }

            return children.ToArray();
        }

        public static void DestroyChildren(this Transform target, int from = 0)
        {
            if (from >= target.childCount)
                throw new System.ArgumentOutOfRangeException($"From", "Can not destroy not existing children");

            for (int i = from; i < target.childCount; i++)
                Object.Destroy(target.GetChild(i).gameObject);
        }

        public static void DestroyChildrenImmediate(this Transform target, int from = 0)
        {
            if (from >= target.childCount)
                throw new System.ArgumentOutOfRangeException($"From", "Can not destroy not existing children");

            GameObject[] children = new GameObject[target.childCount - from];
            for (int i = from; i < target.childCount; i++)
                children[i - from] = target.GetChild(i).gameObject;

            for (int i = 0; i < children.Length; i++)
                Object.DestroyImmediate(children[i]);
        }
    }

    public static class ObjectExtensions
    {
        public static void ThrowNullReferenceException(this Object target, string message)
        {
            throw new System.NullReferenceException($"<b>{target.name}:</b> {message}");
        }

        public static void ThrowArgumentNullException(this Object target, string argumentName)
        {
            throw new System.ArgumentNullException($"<b>{argumentName}</b>", $" <b>{target.name}:</b> Argument is null");
        }

        public static void ThrowArgumentNullException(this Object target, string argumentName, string message)
        {
            throw new System.ArgumentNullException(argumentName, $"<b>{target.name}:</b> {message}");
        }

        public static void ThrowInvalidOperationException(this Object target, string message)
        {
            throw new System.NullReferenceException($"<b>{target.name}:</b> {message}");
        }

        public static void ThrowIndexOutOfRangeException(this Object target, string indexName)
        {
            throw new System.IndexOutOfRangeException($"<b>{target.name}:</b> <b>'{indexName}'</b> is out of range");
        }

        public static void ThrowEventNullException(this Object target, string eventName)
        {
            throw new System.NullReferenceException($"<b>{target.name}:</b> Game event/order/vote named <b>'{eventName}'</b> can not be found. Check the <b>'Event library'</b> object on scene");
        }

        public static void ThrowSceneObjectNullException(this Object target, string objectName)
        {
            throw new System.NullReferenceException($"<b>{target.name}:</b> Game object <b>'{objectName}'</b> not exist. You need to add <b>'{objectName}'</b> prefab on scene");
        }

        public static void PrintLog(this Object target, string message)
        {
            Debug.Log($"<b>{target.name}:</b> {message}");
        }

        public static void PrintWarning(this Object target, string message)
        {
            Debug.LogWarning($"<b>{target.name}</b>: {message}");
        }
    }

    public static class BaseTypesExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new System.ArgumentNullException(nameof(input));


            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}