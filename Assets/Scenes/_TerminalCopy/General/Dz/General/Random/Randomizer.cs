using System;
using System.Collections.Generic;

//NOTE: Pisahin class-classnya
namespace Dz.Random {
    public static class Randomizer {
        /// <summary>
        /// Return one of the object from list. (With weight)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static T Randomize<T>(List<Randomizable> randomizables) {
            float totalRandomValue = 0f;
            for (int i = 0; i < randomizables.Count; i++) {
                totalRandomValue += randomizables[i].Weight;
            }

            float getRandomObjectAtFloat = Range(0f, totalRandomValue);
            totalRandomValue = 0f;
            for (int i = 0; i < randomizables.Count; i++) {
                totalRandomValue += randomizables[i].Weight;
                if (totalRandomValue > getRandomObjectAtFloat) {
                    return (T)randomizables[i].Obj;
                }
            }
            return (T)randomizables[^1].Obj;
        }

        /// <summary>
        /// Return one of the object from list. (No weight)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static T Randomize<T>(List<T> objects) {
            List<Randomizable> randomizables = new();
            for (int i = 0; i < objects.Count; i++) {
                randomizables.Add(new(objects[i], 1f));
            }
            return Randomize<T>(randomizables);
        }

        public static float Range(float minInclusive, float maxInclusive) {
            return minInclusive + (float)new System.Random().NextDouble() * (maxInclusive - minInclusive);
        }

        public static int Range(int minInclusive, int maxExclusive) {
            float nextFloat;
            //prevent max inclusive
            do {
                nextFloat = (float)new System.Random().NextDouble();
            }
            while (nextFloat == 1f);
            return minInclusive + (int)MathF.Floor(nextFloat * (maxExclusive - minInclusive));
        }
    }
}