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

using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace pluginHost
{
    //[ExecuteInEditMode]
    public class VSTEffect : MonoBehaviour
    {
        //important stuff 
        [Header("Alpha build: Windows")]
        [Header("Only supports 64bit VST2 effects (not VSTi)")]
        [Space]
        public string pluginPath = "";
        public bool MonoOutput = false;
        private int thisVSTIndex = 0;
        [Space]

        //////////////////////  params  //////////////////////
        [Header("Parameters")]
        public int numParams;
        [Range(0.0f, 1.0f)]
        public float[] parameters;
        public string[] paramNames;
        private float[] previousParams;

        //////////////////////  audio io  //////////////////////
        private float[][] audioThroughArray;
        private int numPluginInputs;
        private int numPluginOutputs;

        ////////////////////// interop //////////////////////
        private int audioPtrSize;
        private IntPtr inputArrayAsVoidPtr;
        private int messagePtrSize;
        private IntPtr messageAsVoidPtr;
        private bool ready = false;
        private bool pluginFailedToLoad = false;

        void Awake()
        {
            if (ready)
                return;

            if (pluginPath == "")
                pluginPath = Application.dataPath + "//PluginHost//VSTPlugins//StaticDelay.dll";

            thisVSTIndex = loadEffect(pluginPath);
            if(thisVSTIndex == -1)
            {
                Debug.Log("Error, VST has failed to load. Unsupported file path or format");
                pluginFailedToLoad = true;
                return;
            }
            setupParams();
            setupIO();
            ready = true;
        }

        void Update()
        {
            if (pluginFailedToLoad) return;
                
            for (int i = 0; i < numParams; i++)
            {
                if (previousParams[i] != parameters[i])
                {
                    HostDllCpp.setParam(thisVSTIndex, i, parameters[i]);
                    previousParams[i] = parameters[i];
                }
            }
        }
        
        void OnAudioFilterRead(float[] data, int channels)
        {
            if (!ready) return;
            if (pluginFailedToLoad) return;

            HostDllCpp.processBuffer(thisVSTIndex, data, pluggoHost.blockSize, channels);

            if (MonoOutput && channels == 2)
            {
                for(int i = 0; i < data.Length; i+= 2)
                {
                    data[i + 1] = data[i];
                }
            }
        }

        void setupIO()
        {
            ////////////////////// alloc space for audio io //////////////////////
            numPluginInputs = HostDllCpp.getNumPluginInputs(thisVSTIndex);
            numPluginOutputs = HostDllCpp.getNumPluginOutputs(thisVSTIndex);
            audioThroughArray = new float[numPluginInputs][];
            for (int i = 0; i < numPluginInputs; i++)
            {
                audioThroughArray[i] = new float[pluggoHost.blockSize];
            }

            //alloc unmanaged memory
            audioPtrSize = Marshal.SizeOf(audioThroughArray[0][0]) * audioThroughArray[0].Length * audioThroughArray.Length;
            inputArrayAsVoidPtr = Marshal.AllocHGlobal(audioPtrSize);
        }

        void setupParams()
        {
            if (pluginFailedToLoad) return;

            numParams = HostDllCpp.getNumParams(thisVSTIndex);
            parameters = new float[numParams];
            previousParams = new float[numParams];
            paramNames = new string[numParams];
            for (int i = 0; i < numParams; i++)
            {
                parameters[i] = HostDllCpp.getParam(thisVSTIndex, i);
                previousParams[i] = parameters[i];
                paramNames[i] = getParameterName(i);
            }
            //alloc unmanaged memory
            messagePtrSize = 8 * 256;
            messageAsVoidPtr = Marshal.AllocHGlobal(messagePtrSize);
        }

        public string getParameterName(int paramIndex)
        {
            if (pluginFailedToLoad) return "";

            IntPtr p_paramName = HostDllCpp.getParamName(thisVSTIndex, paramIndex);
            return Marshal.PtrToStringAnsi(p_paramName);
        }

        public int loadEffect(string path)
        {
            if (pluginFailedToLoad) return 0;

            IntPtr intPtr_aux = Marshal.StringToHGlobalAnsi(path);
            int effectIndex = HostDllCpp.loadEffect(intPtr_aux);
            Marshal.FreeHGlobal(intPtr_aux);
            return effectIndex;
        }

        public void OnApplicationQuit()
        {
            if (pluginFailedToLoad) return;

            Marshal.FreeHGlobal(inputArrayAsVoidPtr);
            Marshal.FreeHGlobal(messageAsVoidPtr);
            ready = false;
        }
    }
}
