# ![Argus Framework Logo](https://raw.githubusercontent.com/blakepell/ArgusFramework/master/assets/Argus-Logo-Purple-64.png) Argus Framework

## Argus.NAudio

This library currently includes the only addition I've needed to NAudio which is a loopback recorder that is capable of recording 
what is currently being played through the soundcard.

### C# Example Recording from the Default Audio Device

This example is on a WPF window with a button named `ButtonRecord` and it places a file located at `C:\Temp\test.wav`.

```
private Argus.Audio.NAudio.LoopbackRecorder _recorder = new Argus.Audio.NAudio.LoopbackRecorder();

private void ButtonRecord_Click(object sender, RoutedEventArgs e)
{
    if (_recorder.IsRecording)
    {
        this.Title = $"The final recording was {_recorder.ElapsedSeconds()} seconds.";
        _recorder.StopRecording();
        ButtonRecord.Content = "Record";

    }
    else
    {
        _recorder.StartRecording(@"C:\Temp\test.wav");
        ButtonRecord.Content = "Stop";
    }
}
```

```
// Sets up a recorder on the default device that's active.
var recorder = new LoopbackRecorder();
```

By not specifying a parameter in the constructor of `LoopbackRecorder` it sets itself to use the default device on the computer.  In order to 
set your own device a specific device you'll want to turn off the auto setup via passing a false to the constructor:

### C# Example: Setting a Specific Audio Device


```
// Setup the recorder but DON'T wire up the default device
var recorder = new LoopbackRecorder(false);
```


To get a list of the devices `NAudio` provides an `MMDeviceEnumerator` which the `LoopbackRecorder` has put into the `Devices` property.  With
this you can specific a device by name or you can enumerate through the list it has and choose.

```
// Loop over each active audio device
foreach (var item in recorder.Devices.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
{
    if (item.AudioDevice.FriendlyName == "NVIDIA - High Definition Audio")
    {
        recorder.AudioDevice = item;
        break;
    }
}
```

### .Net Framework Support

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Core App 3.1
- .NET Core App 3.0
- .NET Framework 4.6.1
- .NET Framework 4.5
- .NET Framework 3.5

### Historical Note

Back in 2012 I had included an older version of this in a blog post and had noticed recently that it had propagated into 
25-30 respositories and I thought a cleaned up version and Nuget package might be useful.

https://www.blakepell.com/2013-07-26-naudio-loopback-record-what-you-hear-through-the-speaker