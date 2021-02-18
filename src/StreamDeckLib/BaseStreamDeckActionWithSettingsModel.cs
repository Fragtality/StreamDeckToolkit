using StreamDeckLib.Messages;
using System;
using System.Threading.Tasks;

namespace StreamDeckLib
{
  public abstract class BaseStreamDeckActionWithSettingsModel<T> : BaseStreamDeckAction
  {
	public T SettingsModel { get; } = Activator.CreateInstance<T>();

	public override Task OnDidReceiveSettings(StreamDeckEventPayload args)
	{
	  SetModelProperties(args);
	  return Task.CompletedTask;
	}

	public override Task OnWillAppear(StreamDeckEventPayload args)
	{
	  SetModelProperties(args);
	  return Task.CompletedTask;
	}

	//CHANGED
	protected void SetModelProperties(StreamDeckEventPayload args)
	{
			StreamDeckEventPayload.SetModelProperties(args, SettingsModel);
	}

	

	}
}
