using System;
using System.Collections.Generic;

namespace Game.AotHelper
{
    public static class AotEnsureList<T>
    {
        [Preserve]
        public static void Preserve() => Newtonsoft.Json.Utilities.AotHelper.EnsureList<T>();
    }

    public static class AotEnsureDictionary<TKey, TValue>
    {
        [Preserve]
        public static void Preserve()
        {
            _ = new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(default(IDictionary<TKey, TValue>));
            Newtonsoft.Json.Utilities.AotHelper.EnsureDictionary<TKey, TValue>();
        }
    }

    public class PreserveAttribute : Attribute
    {
        public PreserveAttribute() { }

        public PreserveAttribute(Type preserveType) { }
    }

}
