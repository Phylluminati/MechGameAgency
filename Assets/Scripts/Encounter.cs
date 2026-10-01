using UnityEngine;
using System;
using System.Numerics;
using Vector2 = System.Numerics.Vector2;
using System.Collections.Generic;

public class Encounter : ScriptableObject
{
    public enum Type {NOT_ASSIGNED, ENEMY, CARDS, SHOP, BOSS, ENCOUNTER}
    public Type type;
    public int row;
    public int column;
    public Vector2 position;
    public List<Encounter> next_rooms;
    public bool selected = false;
}
