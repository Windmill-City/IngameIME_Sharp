#include "pch.h"
#include "CandidateListWrapper.h"

CandidateListWrapper::CandidateListWrapper(IngameIME::BaseIME* api)
{
	sink_candidateList = gcnew CandidateSink_native(this, &CandidateListWrapper::onCandidateList);
	api->m_sigCandidateList = static_cast<CandidateSink_nativeType>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(sink_candidateList).ToPointer());
}

#include <msclr\marshal_cppstd.h>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
VOID CandidateListWrapper::onCandidateList(CandidateList* list)
{
	auto reflist = gcnew refCandidateList();
	reflist->Candidates = gcnew List<String^>();
	for (size_t i = 0; i < list->m_lPageSize; i++)
	{
		reflist->Candidates->Add(marshal_as<String^>(list->m_pCandidates[i]));
	}
	eventCandidateList(reflist);
}