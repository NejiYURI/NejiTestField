using PixelMapMaker;
using UnityEngine;

namespace Editor
{
    public class PixelMap : ScriptableObject
    {
        public SerializedDictionary<Color32, Vector2Int> lookupMap = new();
        public Color32[] data;
    }
}

