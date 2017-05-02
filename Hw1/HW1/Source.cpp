/*
Assignment 1 - Win32API – simple window with text & graphics
Ahmet Furkan TEKE
B00512703
*/

#include "Windows.h"
#include <tchar.h>
#include <time.h>
#include <stdlib.h>
#include <iostream>
#include <vector>

using namespace std;

#define _UNICODE
#define memclear(var) \
	memset(&var, 0x00, sizeof(var)); // macro will be used for memory cleaning

LPCTSTR fonts[] = { "Calibri", "Cambria", "Georgia", "Tahoma", "Verdana" };

vector<TCHAR*> poetry;

int space = 10; // space from upside (pixel)
int nextLine = 0; // keeps groving to count lines
int lineC; // number of lines of poetry

void WriteText(HDC hdc, int Xpos, int Ypos, char *szMessage) {

	HFONT hFont = CreateFont(18, 0, 0, 0, FW_DONTCARE, FALSE, FALSE, FALSE, DEFAULT_CHARSET, OUT_OUTLINE_PRECIS,
		CLIP_DEFAULT_PRECIS, CLEARTYPE_QUALITY, VARIABLE_PITCH, TEXT(fonts[rand() % 5]));

	COLORREF textColor = SetTextColor(hdc, RGB(rand() % 256, rand() % 256, rand() % 256));

	//SelectObject(hdc, hFont);
	TextOut(hdc, Xpos, Ypos, szMessage, strlen(szMessage));

	// restore text color and font
	SetTextColor(hdc, textColor);
	SelectObject(hdc, hFont);

	DeleteObject(hFont);
}

LRESULT CALLBACK MainWndProc(
	HWND   hwnd,
	UINT   uMsg,
	WPARAM wParam,
	LPARAM lParam
) {

	PAINTSTRUCT ps;
	HDC hDC;
	TCHAR greeting[] = _T("Ahmet F Teke says, 'Do you enjoy reading poetry?");

	// set up window proc for handling close operations
	switch (uMsg) {
	case WM_PAINT:
		hDC = BeginPaint(hwnd, &ps);
		WriteText(hDC, 10, 10, greeting);
		EndPaint(hwnd, &ps);
		break;
	case WM_LBUTTONDOWN:
		InvalidateRect(hwnd, NULL, FALSE);
		hDC = BeginPaint(hwnd, &ps);
		space += 20;
		WriteText(hDC, 10, space, poetry.at(nextLine++%lineC));
		EndPaint(hwnd, &ps);
		break;

	case WM_DESTROY:
		PostQuitMessage(0);
	default:
		return DefWindowProc(hwnd, uMsg, wParam, lParam);
	}
	return 0;
}

int CALLBACK WinMain(
	HINSTANCE hInstance,
	HINSTANCE hPrevInstance,
	LPSTR     lpCmdLine,
	int       nCmdShow)


{
	// writing poetry to vector // 
	poetry.push_back(_T("Khonak an dam ke neshinim dar eyvan, mano to"));
	poetry.push_back(_T("Be do naghsho be do soorat, be yeki jan, mano to"));
	poetry.push_back(_T("Khosh o faregh ze khorafat-e-parishan mano to"));
	poetry.push_back(_T("Mano to, bi man o to, jam' shavim az sar-e-zogh"));
	poetry.push_back(_T("Mewlana Jalaluddin Rumi"));
	poetry.push_back(_T("*** Can't you read? Search on Youtube: Nu - man o to ***"));
	lineC = poetry.size(); // total line of the poetry

	srand((unsigned)time(NULL));

	WNDCLASSEX wndClassData; // windows classex structure
	memclear(wndClassData); // clear

	wndClassData.cbSize = sizeof(WNDCLASSEX); // compability with RegisterClassEx
	wndClassData.lpfnWndProc = MainWndProc;
	wndClassData.hInstance = hInstance;
	wndClassData.hCursor = LoadCursor(NULL, MAKEINTRESOURCE(IDC_ARROW)); // System cursor
	wndClassData.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1); // default background
	wndClassData.lpszClassName = "HW1 - Ahmet F Teke";

	ATOM wndClass = RegisterClassEx(&wndClassData);

	HWND mainWnd = CreateWindow(
		(LPCTSTR)wndClass,
		"HW1 - Ahmet F Teke",
		WS_OVERLAPPEDWINDOW | WS_VISIBLE,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		480,
		720,
		NULL,
		NULL,
		hInstance,
		NULL
	);

	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0)) { // get messages from user
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return 0;
}

