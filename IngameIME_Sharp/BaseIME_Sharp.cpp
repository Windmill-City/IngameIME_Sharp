#include "pch.h"
#include "BaseIME_Sharp.h"
#include "../IngameIME/IngameIME/IMM.cpp"
#include "../IngameIME/IngameIME/TSF.cpp"

void IngameIME_Sharp::BaseIME_Sharp::Initialize(System::IntPtr handle)
{
	m_api->Initialize((HWND)handle.ToPointer());

	sink_alphaMode = gcnew AlphaModeSink_native(this, &BaseIME_Sharp::onAlphaMode);
	m_api->m_sigAlphaMode = static_cast<AlphaModeSink_nativeType>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(sink_alphaMode).ToPointer());

	m_compositionHandler = gcnew CompositionHandler(m_api);
	m_candidateListWrapper = gcnew CandidateListWrapper(m_api);
}

System::IntPtr IngameIME_Sharp::BaseIME_Sharp::Uninitialize()
{
	return (System::IntPtr)m_api->Uninitialize();
}

void IngameIME_Sharp::BaseIME_Sharp::setState(System::Boolean state)
{
	m_api->setState(state);
}

System::Boolean IngameIME_Sharp::BaseIME_Sharp::State()
{
	return m_api->State();
}

void IngameIME_Sharp::BaseIME_Sharp::setFullScreen(System::Boolean fullscreen)
{
	m_api->setFullScreen(fullscreen);
}

System::Boolean IngameIME_Sharp::BaseIME_Sharp::FullScreen()
{
	return m_api->FullScreen();
}

void IngameIME_Sharp::BaseIME_Sharp::onAlphaMode(BOOL alphaMode)
{
	eventAlphaMode(alphaMode);
}

IngameIME_Sharp::IMM::IMM()
{
	m_api = new IngameIME::IMM();
}

IngameIME_Sharp::TSF::TSF()
{
	m_api = new IngameIME::TSF();
}
