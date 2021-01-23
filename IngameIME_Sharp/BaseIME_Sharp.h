#pragma once
#include "../IngameIME/IngameIME/BaseIME.h"
#include "CandidateListWrapper.h"
#include "CompositionHandler.h"
using namespace IngameIME;
namespace IngameIME_Sharp {
	public ref class BaseIME_Sharp
	{
	private:
		typedef VOID(*AlphaModeSink_nativeType)(BOOL);
		delegate VOID AlphaModeSink_native(BOOL);

		AlphaModeSink_native^ sink_alphaMode;
	public:
		BaseIME* m_api;
		CompositionHandler^ m_compositionHandler;
		CandidateListWrapper^ m_candidateListWrapper;

		delegate VOID AlphaModeSink_cli(System::Boolean);
		event AlphaModeSink_cli^ eventAlphaMode;

		void Initialize(System::IntPtr handle);
		System::IntPtr Uninitialize();
		void setState(System::Boolean);
		System::Boolean State();
		void setFullScreen(System::Boolean);
		System::Boolean FullScreen();

		void onAlphaMode(BOOL);
	};
	public ref class IMM : BaseIME_Sharp {
	public:
		IMM();
	};
	public ref class TSF : BaseIME_Sharp {
	public:
		TSF();
	};
}
