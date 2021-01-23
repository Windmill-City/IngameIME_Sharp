# IngameIME_Sharp
A C# wrapper for Windows IME APIs. Its goal is to support both IMM32 and TSF.
using https://github.com/Windmill-City/IngameIME
## Introduction
### Using IngameIME_Sharp
```c#
//get a control first
//For win7 or below, use IMM
//For Win8 above should use TSF
private BaseIME_Sharp api;
//api = new IMM();
api = new TSF();
```
*initialize*
```c#
api.Initialize(Handle);
//Composition
api.m_compositionHandler.eventComposition += M_compositionHandler_eventComposition; ;
api.m_compositionHandler.eventGetTextExt += M_compositionHandler_eventGetTextExt; ;

//CandidateList
api.m_candidateListWrapper.eventCandidateList += M_candidateListWrapper_eventCandidateList;

//AlphaMode
api.eventAlphaMode += Api_eventAlphaMode;

```
*Set IME State and FullScreen Mode
```c#
api.setState(true);
api.setFullScreen(true);
```
## MS Docs
- [TSF Application](https://docs.microsoft.com/en-us/windows/win32/tsf/applications)
- [TSF UILess Mode](https://docs.microsoft.com/en-us/windows/win32/tsf/uiless-mode-overview)

