# WindowsPhoneBluetoothSPP
Simple class to help WindowsPhone 8.1 devices send data to bluetooth devices.

This is a simple class comming from a project I am happy to be part of: Streamoli.
It is a singleton, with two public working methods, designed to help some Windows Phone 8.1 code:
+ ShowBluetoothSettings() which launches bluetooth settings window (allowing to pair phone with desired device)
+ SendData(string), which sends the argument string to the device (serial-port emulation.)

When getting an instance, you should pass the name of the device (as seen in the bluetooth settings window) and port (it is often 1.) An attempt to connect with the device will be made when creating the instance. The code also assumes there is just one device with given name available around.

This is working code. However, it is meant to be a reference for using Windows Phone with bluetooth devices, rather than ready-to-go code. Before using it in your project, make sure you understand what is happening and whether this is what you want to be happening.

3yakuya, Stream Team, 2015