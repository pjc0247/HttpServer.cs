#include "stdafx.h"

#include <memory.h>
#include <stdio.h>
#include <stdlib.h>

#include "WebSocketParser.h"

namespace HttpServ {
#pragma pack (push, 1)
	struct sWebsocketHeaderBase {
		unsigned char dummy1;
		unsigned char dummy2;
	};

	struct sWebsocketHeaderCompact : sWebsocketHeaderBase{
		int maskKey;
	};
	struct sWebsocketHeader16 : sWebsocketHeaderBase{
		unsigned short payloadLength;
		int maskKey;
	};
	struct sWebsocketHeader64 : sWebsocketHeaderBase {
		unsigned long payloadLength;
		int maskKey;
	};
#pragma pack (pop)

	Byte WebSocketHeader::GetMaskKeyAt(int i) {
		int v = maskKey;
		Byte key = *((char*)&v + (i % 4));
		return key;
	}

	WebSocketHeader ^WebSocketParser::Parse(ArraySegment<Byte> ^data) {
		auto header = gcnew WebSocketHeader;

		if (data->Count < sizeof(sWebsocketHeaderCompact))
			return nullptr;

		Byte b1 = data->Array[data->Offset + 0];
		Byte b2 = data->Array[data->Offset + 1];

		header->fin = b1 & 0b10000000;
		header->opcode = b1 & 0b00001111;
		header->mask = b2 & 0b10000000;
		header->payloadLength = b2 & 0b01111111;

		// extended 16
		if (header->payloadLength == 127) {
			throw gcnew NotImplementedException("payload64 is not supported currently");
			/*
			if (data->Count <= sizeof(sWebsocketHeader64))
				return nullptr;

			sWebsocketHeader64 extended;
			for (int i = 2; i < sizeof(sWebsocketHeader64); i++) {
				auto tmp = data->Array[data->Offset + i];
				memcpy((char*)&extended + i, &tmp, sizeof(Byte));
			}

			header->payloadOffset = sizeof(sWebsocketHeader64);
			header->payloadLength = extended.payloadLength;
			header->maskKey = extended.maskKey;
			*/
		}
		else if (header->payloadLength == 126) {
			if (data->Count <= sizeof(sWebsocketHeader16))
				return nullptr;

			sWebsocketHeader16 extended;
			for (int i = 2; i < sizeof(sWebsocketHeader16); i++) {
				auto tmp = data->Array[data->Offset + i];
				memcpy((char*)&extended + i, &tmp, sizeof(Byte));
			}
			
			header->payloadOffset = sizeof(sWebsocketHeader16);
			header->payloadLength = extended.payloadLength;
			header->maskKey = extended.maskKey;
		}
		// compact
		else {
			sWebsocketHeaderCompact extended;
			for (int i = 2; i < sizeof(sWebsocketHeaderCompact); i++) {
				auto tmp = data->Array[data->Offset + i];
				memcpy((char*)&extended + i, &tmp, sizeof(Byte));
			}

			header->payloadOffset = sizeof(sWebsocketHeaderCompact);
			header->maskKey = extended.maskKey;
		}

		/*
		printf("FIN : %d\n", b1 & 0b10000000);
		printf("OPCODE : %d\n", b1 & 0b00001111);
		printf("MASK : %d\n", b2 & 0b10000000);
		printf("PAYLOAD : %d\n", b2 & 0b01111111);
		*/

		return header;
	}

	array<Byte> ^WebSocketParser::Build(ArraySegment<Byte> ^data, unsigned char opcode) {
		array<Byte> ^ary = nullptr;

		// extended
		if (data->Count >= 128) {
			throw gcnew NotImplementedException("payload16/64 is not supported currently");
		}
		// compact
		else {
			sWebsocketHeaderCompact header;

			memset(&header, 0, sizeof(sWebsocketHeaderCompact));

			header.dummy1 = 0b10000000;
			header.dummy1 |= opcode;

			header.dummy2 = 0b00000000;
			header.dummy2 |= data->Count;

			ary = gcnew array<Byte>(
				sizeof(sWebsocketHeaderCompact) - sizeof(int));

			System::Runtime::InteropServices::Marshal::Copy(
				(System::IntPtr)&header, ary, 0, (int)sizeof(sWebsocketHeaderCompact) - sizeof(int));
		}

		return ary;
	}
}