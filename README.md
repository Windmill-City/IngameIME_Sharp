# IME Sharp
A C# wrapper for Windows IME APIs. Its goal is to support both IMM32 and TSF.

Made my own version ImeSharp, using libtf https://github.com/Windmill-City/libtf
## Discord for Dev
https://discord.gg/BC4GKKr
## Introduction
### Using ImeSharp
```c#
//get a control first
//For win7 or below, will get IMM32
//For Win8 above will get TF
iMEControl = ImeSharp.GetDefaultControl();
```
**or FORCE a control**
```c#
iMEControl = ImeSharp.Get_IMM32Control();
iMEControl = ImeSharp.Get_TFControl();
```
*initialize*
```c#
if (UILess)
    iMEControl.Initialize(Handle, true);//UILess Mode, IME dont draw Candidate Window
else
    iMEControl.Initialize(Handle);//Normal mode, IME will draw a Candidate Window
//Composition
//Get Commit Text|Composition Text|Composition Caret
iMEControl.CompositionEvent += IMEControl_CompositionEvent;
//IME require this RECT to position its Candidate Window
iMEControl.GetCompExtEvent += IMEControl_GetCompExtEvent;

//CandidateList, if in UILess Mode, we need to draw CandidateList ourself

if (UILess)
  iMEControl.CandidateListEvent += IMEControl_CandidateListEvent;

```
42
## MS Docs
[TSF Application](https://docs.microsoft.com/en-us/windows/win32/tsf/applications)

[TSF UILess Mode](https://docs.microsoft.com/en-us/windows/win32/tsf/uiless-mode-overview)

[TSF msctf.h header](https://docs.microsoft.com/en-us/windows/win32/api/msctf/)

[IMM32 Use IME in a Game](https://docs.microsoft.com/en-us/windows/win32/dxtecharts/using-an-input-method-editor-in-a-game)

[IMM32 imm.h header](https://docs.microsoft.com/en-us/windows/win32/api/imm/)
## Other samples / implementations
[Chromium](https://github.com/chromium/chromium/tree/master/ui/base/ime/win)

[Windows Class Samples](https://github.com/microsoft/Windows-classic-samples/blob/master/Samples/IME/cpp/SampleIME)

[SDL2](https://github.com/spurious/SDL-mirror/blob/master/src/video/windows/SDL_windowskeyboard.c)

[WPF Core](https://github.com/dotnet/wpf/tree/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Input)

## Credits
[WPF Core](https://github.com/dotnet/wpf)

