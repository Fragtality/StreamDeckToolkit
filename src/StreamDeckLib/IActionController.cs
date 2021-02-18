using System;
using System.Threading;
using StreamDeckLib.Messages;

namespace StreamDeckLib
{
	public interface IActionController : IDisposable
	{
		ConnectionManager DeckManager { get; set; }
		int Timing { get; }

		void Run(CancellationToken token);
		void OnGlobalEvent(StreamDeckEventPayload args);
	}
}
