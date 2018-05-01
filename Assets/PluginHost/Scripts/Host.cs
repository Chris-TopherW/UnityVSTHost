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

namespace pluginHost
{
    public static class pluggoHost
    {
        public static int blockSize;
        public static long sampleRate;

        public static void init()
        {
            HostDllCpp.initHost();

            ////////////////////// setup io //////////////////////
            int _numBuff;
            AudioSettings.GetDSPBufferSize(out blockSize, out _numBuff);
            sampleRate = AudioSettings.outputSampleRate;
            HostDllCpp.setHostBlockSize(blockSize);
            HostDllCpp.setSampleRate(sampleRate);
        }
    }

    [ExecuteInEditMode]
    public class Host : MonoBehaviour
    {
        private void Awake()
        {
            pluggoHost.init();
        }

        private void OnApplicationQuit()
        {
            HostDllCpp.clearVSTs();
        }
    }
}
