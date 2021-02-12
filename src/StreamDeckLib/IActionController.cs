using System;
using System.Threading;

namespace StreamDeckLib
{
	public interface IActionController : IDisposable
	{
		ConnectionManager DeckManager { get; set; }
		int Timing { get; }

		void Run(CancellationToken token);
	}
}
