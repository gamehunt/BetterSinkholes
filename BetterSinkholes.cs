/*
* +==================================================================================+
* |  _  ___                   ____  _                                             _  |
* | | |/  (_) _ __ __ _ ___  |  _ \| | __ _ _   _  __ _ _ __ ___  _   _ _ __   __| | |
* | | ' /| | '_ \ / _` / __| | |_) | |/ _` | | | |/ _` | '__/ _ \| | | | '_ \ / _` | |
* | | . \| | | | | (_| \__ \ |  __/| | (_| | |_| | (_| | | | (_) | |_| | | | | (_| | |
* | |_|\_\_|_| |_|\__, |___/ |_|   |_|\__,_|\__, |\__, |_|  \___/ \__,_|_| |_|\__,_| |
* |               |___/                     |___/ |___/                              |
* |                                                                                  |
* +==================================================================================+
* | Better Sinkholes Redux                                                          |
* | Maintained by Thomasjosif for King's Playground, written by Blackruby            |
* |                                                                                  |
* | https://kingsplayground.fun                                                      |
* +==================================================================================+
* | MIT License                                                                      |
* |                                                                                  |
* | Permission is hereby granted, free of charge, to any person obtaining a copy     |
* | of this software and associated documentation files (the "Software"), to deal    |
* | in the Software without restriction, including without limitation the rights     |
* | to use, copy, modify, merge, publish, distribute, sublicense, and/or sell        |
* | copies of the Software, and to permit persons to whom the Software is            |
* | furnished to do so, subject to the following conditions:                         |
* |                                                                                  |
* | The above copyright notice and this permission notice shall be included in all   |
* | copies or substantial portions of the Software.                                  |
* |                                                                                  |
* | THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR       |
* | IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,         |
* | FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE      |
* | AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER           |
* | LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,    |
* | OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE    |
* | SOFTWARE.                                                                        |
* +==================================================================================+
*/
using System;
using Exiled.API.Features;
using HarmonyLib;

namespace BetterSinkholes
{
    public class BetterSinkholes : Plugin<Config>
    {
        public override string Author { get; } = "Thomasjosif, origially written by Blackruby";
        public override string Name { get; } = "BetterSinkholes Redux";
        public override string Prefix { get; } = "BSH Redux";
        public override Version Version { get; } = new Version(3, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 3);
        public static int PatchCount = 0;
        public static Harmony Harmony { set; get; }
        public static Config config;

        public override void OnEnabled()
        {
            config = Config;
            if (config.IsEnabled)
            {
                Log.Info($"{Name} plugin loaded! Maintained by {Author}.");
                DoPatching();
            }   
        }

        public override void OnDisabled()
        {
            Harmony.UnpatchAll();
        }

        // Enables the modifications made to the sinkholes
        public static void DoPatching()
        {
            Harmony = new Harmony($"exiled.bettersinkholes{++PatchCount}");
            Harmony.PatchAll();
        }
    }
}
