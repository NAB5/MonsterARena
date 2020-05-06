# MonsterARena

## Prerequisites:
- Iphone SE or better
- macOS 10.13. 6 or later and Xcode 10 or later
- Apple Developer License
- Unity 2019.2.13f1 and Unity Hub.

## Build Instructions
Open Unity Hub and install Unity 2019.2.13f1 from the “Installs” tab on the left. Add the MonsterARena project folder from Github with the “Add” button at the top right.

Next, double click to open in Unity. Once open go to File->Build Settings->Build and Run. All the project settings should be set correctly and the build should succeed.

Once the build is finished on Unity’s end, it will create an Xcode project and redirect you to Xcode. It will try to run, but make sure to cancel as some settings need to be changed. First, make sure your Iphone is plugged into your computer via usb and is set as the target for the build:

After this, click on “Unity-Iphone” at the top of the file hierarchy located on the left. Go to “Signing & Capabilities” and make sure to check “Automatically manage signing” and select your “Team”. Add an account if need be.

Finally, click the play button at the top right to build and run on your targeted device. If you run into a shader error, follow this link to resolve the issue:

https://forum.unity.com/threads/unity-2019-2-7-build-ios-crashes-in-shader-compile.757754/?_ga=2.113408007.1857113669.1574585473-2145115517.1571751592

