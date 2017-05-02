#include "stdafx.h"
#include "hw3.h"
#include <iostream>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

int globalCounter = 0;
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CFrameWnd)

const int  iMaxUserToolbars = 10;
const UINT uiFirstUserToolBarId = AFX_IDW_CONTROLBAR_FIRST + 40;
const UINT uiLastUserToolBarId = uiFirstUserToolBarId + iMaxUserToolbars - 1;

/*Define messages we will respond to and crete the window*/

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	ON_WM_PAINT()
	ON_WM_MOVE()
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_INDICATOR_CAPS,
	ID_INDICATOR_NUM,
	ID_INDICATOR_SCRL,
};

// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	//Create(NULL, _T("Ex04a Text Output"), WS_OVERLAPPEDWINDOW, CRect(20, 30, 350, 140));

	//dc is device context.
	//GetClientToScreen
}


void CMainFrame::OnPaint()
{
	CDC * dc; dc = GetDC();   //CPaintDC is only used in OnPaint function.
	CPaintDC dc2(this);

	CRect clientR, windowR; //Client Rectangle and Window Rectangle.

	GetWindowRect(&windowR);
	GetClientRect(&clientR);

	CString message = _T("Hi! 'Oguzhan Duzen'");
	dc2.TextOut(0, 0, message);

	if (globalCounter > 1) {
		dc->SetTextColor(RGB(0, 0, 255));
	} globalCounter++;

	CString coordinates;
	coordinates.Format(_T("left = %d, right = %d, top = %d, bottom = %d")
		, windowR.left, windowR.right, windowR.top, windowR.bottom);
	dc->TextOut(0, 25, coordinates);

	CString coordinates2;
	coordinates2.Format(_T("left = %d right = %d top = %d bottom = %d")
		, clientR.left, clientR.right, clientR.top, clientR.bottom);
	dc->TextOut(0, 50, coordinates2);

}



void CMainFrame::OnMove(int x, int y)
{

	CFrameWnd::OnMove(x, y);
	CDC * dc; dc = GetDC();  //Get device context.
	CRect clientR, windowR; //Client Rectangle and Window Rectangle.
	CClientDC dc2(this);

	GetClientRect(&clientR);
	GetWindowRect(&windowR);

	int thicknessOfBorder = GetSystemMetrics(SM_CXPADDEDBORDER);
	int & t = thicknessOfBorder;


	CString message = _T("Hi! 'Oguzhan Duzen'");
	dc2.TextOut(0, 0, message);

	if (globalCounter > 1) {
		dc->SetTextColor(RGB(0, 0, 255));
	} globalCounter++;

	CString coordinates;
	coordinates.Format(_T("left = %d, right = %d, top = %d, bottom = %d")
		, windowR.left, windowR.right, windowR.top, windowR.bottom);
	dc->TextOut(0, 25, coordinates);

	CString coordinates2;
	coordinates2.Format(_T("left = %d right = %d top = %d bottom = %d")
		, clientR.left, clientR.right, clientR.top, clientR.bottom);
	dc->TextOut(0, 50, coordinates2);

}


CMainFrame::~CMainFrame()
{

}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	// create a view to occupy the client area of the frame
	if (!m_wndView.Create(NULL, NULL, AFX_WS_DEFAULT_VIEW, CRect(0, 0, 0, 0), this, AFX_IDW_PANE_FIRST, NULL))
	{
		TRACE0("Failed to create view window\n");
		return -1;
	}

	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP | CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}

	if (!m_wndStatusBar.Create(this))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}
	m_wndStatusBar.SetIndicators(indicators, sizeof(indicators) / sizeof(UINT));

	// TODO: Delete these three lines if you don't want the toolbar to be dockable
	m_wndToolBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndToolBar);


	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if (!CFrameWnd::PreCreateWindow(cs))
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs


	cs.dwExStyle &= ~WS_EX_CLIENTEDGE;
	cs.lpszClass = AfxRegisterWndClass(0);
	return TRUE;
}

// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWnd::Dump(dc);
}
#endif //_DEBUG


// CMainFrame message handlers

void CMainFrame::OnSetFocus(CWnd* /*pOldWnd*/)
{
	// forward focus to the view window
	m_wndView.SetFocus();
}

BOOL CMainFrame::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo)
{
	// let the view have first crack at the command
	if (m_wndView.OnCmdMsg(nID, nCode, pExtra, pHandlerInfo))
		return TRUE;

	// otherwise, do default handling
	return CFrameWnd::OnCmdMsg(nID, nCode, pExtra, pHandlerInfo);
}

