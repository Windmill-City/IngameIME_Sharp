#pragma once
#include "../IngameIME/IngameIME/BaseIME.h"

using namespace System;
using namespace System::Collections::Generic;
public ref struct refCandidateList
{
public:
	List<String^>^ Candidates;//PageSize indicates its array length
};

using namespace libtf;
public ref class CandidateListWrapper
{
private:
	typedef VOID(*CandidateSink_nativeType)(CandidateList* list);
	delegate VOID CandidateSink_native(CandidateList* list);

	CandidateSink_native^ sink_candidateList;
public:
	delegate VOID CandidateSink_cli(refCandidateList^ list);
	event CandidateSink_cli^ eventCandidateList;

	CandidateListWrapper(IngameIME::BaseIME* api);
	VOID onCandidateList(CandidateList* list);
};
