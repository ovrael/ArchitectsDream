using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssemblyCSharp.Assets.Tools
{
    public static class Extensions
    {
        private static System.Random rng = new System.Random();

        public static T RandomOne<T>(this T[] array)
        {
            return array[rng.Next(array.Length)];
        }

        public static (T Element, int Index) RandomOneWithIndex<T>(this T[] array)
        {
            int index = rng.Next(array.Length);
            return (array[index], index);
        }

        public static string PlayAnimation(this Animator animator, string nextAnimationName, string currentAnimationName)
        {
            if (currentAnimationName == nextAnimationName)
                return currentAnimationName;

            animator.Play(nextAnimationName);
            return nextAnimationName;
        }
    }
}
