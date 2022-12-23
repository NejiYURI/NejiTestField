using PixelMapMaker;
using UnityEngine;

namespace PixelEditor
{
    public class PixelMap : ScriptableObject
    {
        public SerializedDictionary<Color32, Vector2Int> lookupMap = new();
        public Color32[] data;
    }
}

