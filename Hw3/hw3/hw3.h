
#pragma once

#ifndef __AFXWIN_H__
#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"       // main symbols


class Chw3 : public CWinApp
{
public:
	Chw3();


	// Overrides
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

	// Implementation

public:
	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern Chw3 theApp;
