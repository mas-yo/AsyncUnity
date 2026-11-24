
using System.Collections.Generic;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public class GridMap
    { 
         private readonly string[][] _gameEntityCodes;

         public GridMap(string[][] gameEntityCodes)
         {
             _gameEntityCodes = gameEntityCodes;
         }

         public string GetCell(int x, int y)
         {
             if (y < 0 || y >= _gameEntityCodes.Length || x < 0 || x >= _gameEntityCodes[y].Length)
             {
                 return string.Empty;
             }

             return _gameEntityCodes[y][x];
         }

         public IEnumerable<(int x, int y, string entityCode)> EnumerateCells()
         {
             for (int y = 0; y < _gameEntityCodes.Length; y++)
             {
                 for (int x = 0; x < _gameEntityCodes[y].Length; x++)
                 {
                     yield return (x, y, _gameEntityCodes[y][x]);
                 }
             }
         }

         public static Vector3 GridPositionToWorld(Vector3Int position)
         {
             return new Vector3(position.x, position.y, position.z);
         }
         public static Vector3Int WorldPositionToGrid(Vector3 position)
         {
             return new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
         }

    }
}