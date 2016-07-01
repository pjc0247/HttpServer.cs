#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace HttpServ {
	public ref class WebSocketHeader
	{
	public:
		bool fin;
		bool mask;
		char opcode;
		int maskKey;
		unsigned short payloadLength;

		int payloadOffset;

		bool IsControlFrame();
		Byte GetMaskKeyAt(int idx);
	};

	public ref class WebSocketParser
	{
	public:
		static WebSocketHeader ^Parse(ArraySegment<Byte> ^data);

		static array<Byte> ^Build(ArraySegment<Byte> ^data, unsigned char opcode);
	};
}