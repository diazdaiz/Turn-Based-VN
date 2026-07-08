using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class Library : MonoBehaviour {
    // Dictionary<Type, Card> Cards;
    // Dictionary<Type, Card> Relics;
    // Dictionary<Type, Card> Enemies;

    static Dictionary<Type, string> ElementsPath {
        get {
            if (elementsPath == null) {
                elementsPath = new();
                List<Type> cardsType = GetDerivedTypes<Skill>();
                for (int i = 0; i < cardsType.Count; i++) {
                    elementsPath.Add(cardsType[i], $"res://Assets/Game/Exploration/Elements/Card/Cards/{cardsType[i].Name}.tscn");
                }
                List<Type> relicsType = GetDerivedTypes<Equipment>();
                for (int i = 0; i < relicsType.Count; i++) {
                    elementsPath.Add(relicsType[i], $"res://Assets/Game/Exploration/Elements/Relic/Relics/{relicsType[i].Name}.tscn");
                }
                List<Type> potionType = GetDerivedTypes<Consumable>();
                for (int i = 0; i < potionType.Count; i++) {
                    elementsPath.Add(potionType[i], $"res://Assets/Game/Exploration/Elements/Potion/Potions/{potionType[i].Name}.tscn");
                }
            }
            return elementsPath;
        }
    }
    static Dictionary<Type, string> elementsPath;

    public static List<Type> GetDerivedTypes<T>() {
        return Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(T)))
            .ToList();
    }

    /// <summary>
    /// T is either Card, Relic or Potion. While elementType is class that derived from one of it.
    /// Example: Instantiate<Card>(typeof(Shiv));
    /// Example: Instantiate<Relic>(typeof(Akabeko));
    /// </summary>
    /// <param name="card"></param>
    public static T Instantiate<T>(Type elementType) where T : MonoBehaviour {
        return Instantiate(Resources.Load<GameObject>(ElementsPath[elementType])) as T;
    }

    public static object Instantiate(Type elementType) {
        return Instantiate(Resources.Load<GameObject>(ElementsPath[elementType]));
    }
}
