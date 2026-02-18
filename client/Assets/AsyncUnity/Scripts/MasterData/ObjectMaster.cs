﻿using System;
using MasterMemory;
using MessagePack;

namespace AsyncUnity.Scripts.MasterData
{
    public enum ObjectType
    {
        Rock,
        Mushroom,
    }
    
    [MemoryTable("Object"), MessagePackObject(false)]
    public record ObjectMaster
    {
        [PrimaryKey]
        [Key(0)]
        public long Code { get; init; }
        [Key(1)]
        public ObjectType ObjectType { get; init; }
        [Key(2)]
        public string ModelPrefabPath { get; init; }
        [Key(3)]
        public UnityEngine.Vector3 InitialPosition { get; init; }

        public static ObjectMaster FromDictionary(System.Collections.Generic.IReadOnlyDictionary<string, string> dict)
        {
            return new ObjectMaster()
            {
                Code = long.Parse(dict["Code"]),
                ObjectType = Enum.Parse<ObjectType>(dict["ObjectType"]),
                ModelPrefabPath = dict["ModelPrefabPath"],
                InitialPosition = new UnityEngine.Vector3(float.Parse(dict["InitialPosition.x"]), float.Parse(dict["InitialPosition.y"]), float.Parse(dict["InitialPosition.z"])),
            };
        }
    }
}