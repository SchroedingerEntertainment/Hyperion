// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SE.Hyperion.Desktop.Win32
{
    public partial struct Window
    {
        const int ExtendedKeyMask = (1 << 24);
        const int ScanCodeMask = 0xFF0000;

        private readonly static Dictionary<int, Key> KeyTable = new Dictionary<int, Key>
        {
            { 0, Key.None },
            { 3, Key.Cancel },
            { 8, Key.Back },
            { 9, Key.Tab },
            { 12, Key.Clear },
            { 13, Key.Return },
            { 16, Key.LeftShift},
            { 17, Key.LeftCtrl},
            { 18, Key.LeftAlt },
            { 19, Key.Pause },
            { 20, Key.Capital },
            { 21, Key.KanaMode },
            { 23, Key.JunjaMode },
            { 24, Key.FinalMode },
            { 25, Key.HanjaMode },
            { 27, Key.Escape },
            { 28, Key.ImeConvert },
            { 29, Key.ImeNonConvert },
            { 30, Key.ImeAccept },
            { 31, Key.ImeModeChange },
            { 32, Key.Space },
            { 33, Key.PageUp },
            { 34, Key.PageDown },
            { 35, Key.End },
            { 36, Key.Home },
            { 37, Key.Left },
            { 38, Key.Up },
            { 39, Key.Right },
            { 40, Key.Down },
            { 41, Key.Select },
            { 42, Key.Print },
            { 43, Key.Execute },
            { 44, Key.Snapshot },
            { 45, Key.Insert },
            { 46, Key.Delete },
            { 47, Key.Help },
            { 48, Key.D0 },
            { 49, Key.D1 },
            { 50, Key.D2 },
            { 51, Key.D3 },
            { 52, Key.D4 },
            { 53, Key.D5 },
            { 54, Key.D6 },
            { 55, Key.D7 },
            { 56, Key.D8 },
            { 57, Key.D9 },
            { 65, Key.A },
            { 66, Key.B },
            { 67, Key.C },
            { 68, Key.D },
            { 69, Key.E },
            { 70, Key.F },
            { 71, Key.G },
            { 72, Key.H },
            { 73, Key.I },
            { 74, Key.J },
            { 75, Key.K },
            { 76, Key.L },
            { 77, Key.M },
            { 78, Key.N },
            { 79, Key.O },
            { 80, Key.P },
            { 81, Key.Q },
            { 82, Key.R },
            { 83, Key.S },
            { 84, Key.T },
            { 85, Key.U },
            { 86, Key.V },
            { 87, Key.W },
            { 88, Key.X },
            { 89, Key.Y },
            { 90, Key.Z },
            { 91, Key.LWin },
            { 92, Key.RWin },
            { 93, Key.Apps },
            { 95, Key.Sleep },
            { 96, Key.NumPad0 },
            { 97, Key.NumPad1 },
            { 98, Key.NumPad2 },
            { 99, Key.NumPad3 },
            { 100, Key.NumPad4 },
            { 101, Key.NumPad5 },
            { 102, Key.NumPad6 },
            { 103, Key.NumPad7 },
            { 104, Key.NumPad8 },
            { 105, Key.NumPad9 },
            { 106, Key.Multiply },
            { 107, Key.Add },
            { 108, Key.Separator },
            { 109, Key.Subtract },
            { 110, Key.Decimal },
            { 111, Key.Divide },
            { 112, Key.F1 },
            { 113, Key.F2 },
            { 114, Key.F3 },
            { 115, Key.F4 },
            { 116, Key.F5 },
            { 117, Key.F6 },
            { 118, Key.F7 },
            { 119, Key.F8 },
            { 120, Key.F9 },
            { 121, Key.F10 },
            { 122, Key.F11 },
            { 123, Key.F12 },
            { 124, Key.F13 },
            { 125, Key.F14 },
            { 126, Key.F15 },
            { 127, Key.F16 },
            { 128, Key.F17 },
            { 129, Key.F18 },
            { 130, Key.F19 },
            { 131, Key.F20 },
            { 132, Key.F21 },
            { 133, Key.F22 },
            { 134, Key.F23 },
            { 135, Key.F24 },
            { 144, Key.NumLock },
            { 145, Key.Scroll },
            { 160, Key.LeftShift },
            { 161, Key.RightShift },
            { 162, Key.LeftCtrl },
            { 163, Key.RightCtrl },
            { 164, Key.LeftAlt },
            { 165, Key.RightAlt },
            { 166, Key.BrowserBack },
            { 167, Key.BrowserForward },
            { 168, Key.BrowserRefresh },
            { 169, Key.BrowserStop },
            { 170, Key.BrowserSearch },
            { 171, Key.BrowserFavorites },
            { 172, Key.BrowserHome },
            { 173, Key.VolumeMute },
            { 174, Key.VolumeDown },
            { 175, Key.VolumeUp },
            { 176, Key.MediaNextTrack },
            { 177, Key.MediaPreviousTrack },
            { 178, Key.MediaStop },
            { 179, Key.MediaPlayPause },
            { 180, Key.LaunchMail },
            { 181, Key.SelectMedia },
            { 182, Key.LaunchApplication1 },
            { 183, Key.LaunchApplication2 },
            { 186, Key.Oem1 },
            { 187, Key.OemPlus },
            { 188, Key.OemComma },
            { 189, Key.OemMinus },
            { 190, Key.OemPeriod },
            { 191, Key.OemQuestion },
            { 192, Key.Oem3 },
            { 193, Key.AbntC1 },
            { 194, Key.AbntC2 },
            { 219, Key.OemOpenBrackets },
            { 220, Key.Oem5 },
            { 221, Key.Oem6 },
            { 222, Key.OemQuotes },
            { 223, Key.Oem8 },
            { 226, Key.OemBackslash },
            { 229, Key.ImeProcessed },
            { 240, Key.OemAttn },
            { 241, Key.OemFinish },
            { 242, Key.OemCopy },
            { 243, Key.DbeSbcsChar },
            { 244, Key.OemEnlw },
            { 245, Key.OemBackTab },
            { 246, Key.DbeNoRoman },
            { 247, Key.DbeEnterWordRegisterMode },
            { 248, Key.DbeEnterImeConfigureMode },
            { 249, Key.EraseEof },
            { 250, Key.Play },
            { 251, Key.DbeNoCodeInput },
            { 252, Key.NoName },
            { 253, Key.Pa1 },
            { 254, Key.OemClear },
        };

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static Key GetKey(int virtualKey, int keyData)
        {
            virtualKey = GetVirtualKey(virtualKey, keyData);
            Key key; KeyTable.TryGetValue(virtualKey, out key);

            return key;
        }

        private static int GetVirtualKey(int virtualKey, int keyData)
        {
            if (virtualKey == (int)VirtualKeys.VK_SHIFT)
            {
                virtualKey = Window.MapVirtualKey((keyData & ScanCodeMask) >> 16, VirtualKeyMap.MAPVK_VSC_TO_VK_EX);
                if (virtualKey == 0)
                {
                    return (int)VirtualKeys.VK_LSHIFT;
                }
                else return virtualKey;
            }
            else if (virtualKey == (int)VirtualKeys.VK_MENU)
            {
                if (IsExtended(keyData))
                {
                    return (int)VirtualKeys.VK_RMENU;
                }
                else return (int)VirtualKeys.VK_LMENU;
            }
            else if (virtualKey == (int)Win32.VirtualKeys.VK_CONTROL)
            {
                if (IsExtended(keyData))
                {
                    return (int)VirtualKeys.VK_RCONTROL;
                }
                else return (int)VirtualKeys.VK_LCONTROL;
            }
            else return virtualKey;
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        private static bool IsExtended(int keyData)
        {
            return ((keyData & ExtendedKeyMask) != 0);
        }
    }
}
