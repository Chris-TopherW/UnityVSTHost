# UnityVSTHost
VST plugin host for Unity engine. 

This is a very limited implementation of the standard:
- It supports 64bit VST2 plugins on Windows only. 
- Does not support MIDI input.
- Has no GUI support- it simply populates the inspector with all of the paramaters from the plugin.

Basically it uses OnAudioFilterRead to call the DSP callback on loaded plugins and can send them parameters.

Note that I have removed the C++ dll source code repo as Steinberg do not allow the vst2 library to be shared and they also no longer distribute it.

Warning: this code was part of my grad research in 2016 and is a dumpster fire, enter at your own peril.
