
#pragma once
#include "ChildView.h"

class CMainFrame : public CFrameWnd
{
private:
public:
	CMainFrame();
protected:
	DECLARE_DYNAMIC(CMainFrame)

	// Attributes
public:

	// Operations
public:

	// Overrides
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);

	// Implementation
public:
	virtual ~CMainFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:  // control bar embedded members
	CToolBar          m_wndToolBar;
	CStatusBar        m_wndStatusBar;
	CChildView    m_wndView;

	// Generated message map functions
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSetFocus(CWnd *pOldWnd);

	//My Implementation
	//void common(CWnd * wptr);
	//afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnPaint();
	afx_msg void OnMove(int x, int y);
	//afx_msg void OnSizing(UINT nSide, LPRECT lpRect);

	DECLARE_MESSAGE_MAP()
};


