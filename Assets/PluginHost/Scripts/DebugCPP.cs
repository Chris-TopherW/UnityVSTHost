//Copyright 2018 Chris Wratt and Victoria University of Wellington
//Permission is hereby granted, free of charge, to any person obtaining
//a copy of this software and associated documentation files, 
//to deal in the Software without restriction, including without 
//limitation the rights to use, copy, modify, merge, publish, distribute, 
//sublicense, and/or sell copies of the Software, and to permit persons 
//to whom the Software is furnished to do so, subject to the following 
//conditions:

//The above copyright notice and this permission notice shall be included
//in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
//OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
//CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
//SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_EDITOR

namespace CppDebug
{
    public class DebugCPP : MonoBehaviour
    {
        private bool showNativeDebug = false;
        private static bool staticAllowDebug = true;
        void OnEnable()
        {
            RegisterDebugCallback(OnDebugCallback);
            staticAllowDebug = showNativeDebug;
        }

        private void Update()
        {
            staticAllowDebug = showNativeDebug;
        }

        [DllImport("VSTHostUnity", CallingConvention = CallingConvention.Cdecl)]
        static extern void RegisterDebugCallback(debugCallback cb);
        delegate void debugCallback(IntPtr request, int size);
        [MonoPInvokeCallback(typeof(debugCallback))]
        static void OnDebugCallback(IntPtr request, int size)
        {
            if (!staticAllowDebug)
                return;
            string debug_string = Marshal.PtrToStringAnsi(request, size);

            UnityEngine.Debug.Log(debug_string);
        }
    }
}
#endif
