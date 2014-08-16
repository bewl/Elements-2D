using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Utilities
{
    public static class Extensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #region Vector3 Shizz
        public static void SetXPos(this Transform transform, float x)
        {
            Vector3 newPos = new Vector3(x, transform.position.y, transform.position.z);

            transform.position = newPos;
        }

        public static void SetYPos(this Transform transform, float y)
        {
            Vector3 newPos = new Vector3(transform.position.x, y, transform.position.z);

            transform.position = newPos;
        }

        public static void SetZPos(this Transform transform, float z)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, z);

            transform.position = newPos;
        }

        #endregion
    }
}
