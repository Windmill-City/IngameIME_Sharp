#include "pch.h"
#include "CompositionHandler.h"

CompositionHandler::CompositionHandler(IngameIME::BaseIME* api)
{
	sink_comp = gcnew CompositionSink_native(this, &CompositionHandler::onComposition);
	api->m_sigComposition = static_cast<CompositionSink_nativeType>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(sink_comp).ToPointer());

	sink_ext = gcnew CompositionExtSink_native(this, &CompositionHandler::onCompositionExt);
	api->m_sigGetTextExt = static_cast<CompositionExtSink_nativeType>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(sink_ext).ToPointer());
}
#include <msclr\marshal_cppstd.h>
using namespace msclr::interop;
using namespace System;
VOID CompositionHandler::onComposition(CompositionEventArgs* comp)
{
	auto refComp = gcnew refCompositionEventArgs();
	refComp->m_state = (refCompositionState)comp->m_state;
	refComp->m_lCaretPos = comp->m_lCaretPos;
	refComp->m_strComposition = marshal_as<String^>(comp->m_strComposition);
	refComp->m_strCommit = marshal_as<String^>(comp->m_strCommit);
	eventComposition(refComp);
}

VOID CompositionHandler::onCompositionExt(RECT* rect)
{
	auto refRect = gcnew refRECT();
	refRect->top = rect->top;
	refRect->left = rect->left;
	refRect->bottom = rect->bottom;
	refRect->right = rect->right;
	eventGetTextExt(refRect);
	rect->top = refRect->top;
	rect->left = refRect->left;
	rect->bottom = refRect->bottom;
	rect->right = refRect->right;
}