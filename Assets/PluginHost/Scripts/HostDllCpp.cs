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

using System.Runtime.InteropServices;
using System;

namespace pluginHost
{
    public static class HostDllCpp
    {
        ///////////////////////////////////////////Loading///////////////////////////////////////////

        const string cppDllName = "VSTHostUnity";
        [DllImport(cppDllName, EntryPoint = "initHost", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void initHost();

        [DllImport(cppDllName, EntryPoint = "loadEffect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int loadEffect(IntPtr path);

        [DllImport(cppDllName, EntryPoint = "loadInstrument", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int loadInstrument(IntPtr path);

        ///////////////////////////////////////////Parameters///////////////////////////////////////////

        [DllImport(cppDllName, EntryPoint = "setSampleRate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setSampleRate(long p_sampleRate);

        [DllImport(cppDllName, EntryPoint = "setHostBlockSize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setHostBlockSize(int p_blocksize);

        [DllImport(cppDllName, EntryPoint = "getNumParams", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNumParams(int vstIndex);

        [DllImport(cppDllName, EntryPoint = "getParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern float getParam(int vstIndex, int paramIndex);

        [DllImport(cppDllName, EntryPoint = "getParamName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getParamName(int vstIndex, int paramIndex);

        [DllImport(cppDllName, EntryPoint = "setParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern int setParam(int vstIndex, int paramIndex, float p_value);

        ///////////////////////////////////////////IO/////////////////////////////////////////

        [DllImport(cppDllName, EntryPoint = "processFxAudio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr processFxAudio(int vstIndex, IntPtr audioThrough, long numFrames, int numChannels);

        [DllImport(cppDllName, EntryPoint = "processInstAudio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr processInstAudio(int vstIndex, long numFrames, int numChannels);

        [DllImport(cppDllName, EntryPoint = "getNumPluginInputs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNumPluginInputs(int vstIndex);

        [DllImport(cppDllName, EntryPoint = "getNumPluginOutputs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNumPluginOutputs(int vstIndex);

        //////////////////////////////////////clean up///////////////////////////////////////

        [DllImport(cppDllName, EntryPoint = "clearVSTs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clearVSTs();
    }
}
