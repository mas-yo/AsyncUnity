using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Lua;
using Lua.IO;
using Lua.Platforms;
using Lua.Standard;
using Lua.Unity;
using UnityEngine;

namespace AsyncUnity.Scenes.Game1
{
    public static class Scripting
    {
        private static readonly string ScriptDirectory = Path.Combine(Application.persistentDataPath, Constants.StaticDataDirName, Constants.ScriptDirName);
        private const string StartScriptSuffix = "_Start";
        private const string UpdateScriptSuffix = "_Update";

        private const string ScriptExtension = ".lua";

        public static LuaState CreateLuaState()
        {
            var platform = new LuaPlatform(
                FileSystem: new FileSystem(),
                OsEnvironment: new UnityApplicationOsEnvironment(),
                StandardIO: new UnityStandardIO(),
                TimeProvider: TimeProvider.System);
            var state = LuaState.Create(platform);
            state.OpenStandardLibraries();
            return state;
        }


        


        public static string CalcScriptDirectory(string stageName)
        {
            return Path.Combine(ScriptDirectory, stageName);
        }
        public static string CalcScriptFilePath(string stageName, string scriptName)
        {
            var dir = CalcScriptDirectory(stageName);
            return Path.Combine(dir, $"{scriptName}{ScriptExtension}");
        }

        public static string CalcScriptFilePath(string stageName, string entityName, string suffix)
        {
            return CalcScriptFilePath(stageName, $"{entityName}{suffix}");
        }
        public static string CalcStartScriptFilePath(string stageName, string entityName)
        {
            return CalcScriptFilePath(stageName, entityName, StartScriptSuffix);
        }
        public static IEnumerable<string> CalcUpdateScriptFilePaths(string stageName, string entityName)
        {
            string baseName = $"{entityName}{UpdateScriptSuffix}";
            return Directory.EnumerateFiles(CalcScriptDirectory(stageName), $"{baseName}*{ScriptExtension}")
                .Select(path => (path, Path.GetFileNameWithoutExtension(path).Substring(baseName.Length)))
                .Where(t => int.TryParse(t.Item2, out _))
                .OrderBy(t => int.Parse(t.Item2))
                .Select(t => t.path);
        }

        public static async UniTask OpenScriptEditorAsync(string scriptFilePath, CancellationToken token)
        {
            if (!File.Exists(scriptFilePath))
            {
                await File.WriteAllTextAsync(scriptFilePath, string.Empty, token);
            }
            System.Diagnostics.Process.Start(scriptFilePath);
        }
        public static void OpenScriptDirectory(string stageName)
        {
            var dir = CalcScriptDirectory(stageName);
            string path = "file://" + dir.Replace("\\", "/");
            Application.OpenURL(path);
        }

    }
}