#pragma once
#include "../IngameIME//IngameIME/BaseIME.h"

public enum class refCompositionState
{
	StartComposition,
	Composing,
	Commit,
	EndComposition
};

public ref struct refRECT {
	int top;
	int left;
	int bottom;
	int right;
};

public ref struct refCompositionEventArgs
{
public:
	refCompositionState				m_state;
	LONG                            m_lCaretPos;
	System::String^					m_strComposition;
	System::String^					m_strCommit;
};

using namespace libtf;
public ref class CompositionHandler
{
#pragma region EventHandler Def&Var
	//CPP
	typedef VOID(*CompositionSink_nativeType)(CompositionEventArgs* comp);
	delegate VOID CompositionSink_native(CompositionEventArgs* comp);

	typedef VOID(*CompositionExtSink_nativeType)(RECT* rect);
	delegate VOID CompositionExtSink_native(RECT* rect);

	CompositionSink_native^ sink_comp;
	CompositionExtSink_native^ sink_ext;
#pragma endregion
public:
	delegate VOID CompositionSink_cli(refCompositionEventArgs^ comp);
	delegate VOID CompositionExtSink_cli(refRECT^ rect);
	event CompositionSink_cli^ eventComposition;
	event CompositionExtSink_cli^ eventGetTextExt;

	CompositionHandler(IngameIME::BaseIME* api);
	VOID onComposition(CompositionEventArgs* comp);
	VOID onCompositionExt(RECT* rect);
};
