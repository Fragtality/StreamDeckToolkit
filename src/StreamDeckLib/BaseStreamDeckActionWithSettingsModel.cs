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

	protected void SetModelProperties(StreamDeckEventPayload args)
	{
	  var properties = typeof(T).GetProperties();
	  foreach (var prop in properties)
	  {
			if (args.payload != null && args.payload.settings != null && args.payload.settings.settingsModel != null)
			{
				if (args.PayloadSettingsHasProperty(prop.Name))
				{
					var value = args.GetPayloadSettingsValue(prop.Name);
					dynamic value2;
					if (IsNumericOrBooleanType(prop.PropertyType) && (value is string) && (value as string) == "")
					{
						if (IsBooleanType(prop.PropertyType))
							value2 = false;
						else
							value2 = 0;
					}
					else
						value2 = Convert.ChangeType(value, prop.PropertyType);
					prop.SetValue(SettingsModel, value2);
				}
			}
	  }
	}

	public static bool IsNumericOrBooleanType(Type type)
	{
		switch (Type.GetTypeCode(type))
		{
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
			case TypeCode.Boolean:
				return true;
			default:
				return false;
		}
	}

	public static bool IsBooleanType(Type type)
	{
		return Type.GetTypeCode(type) == TypeCode.Boolean;
	}

	}
}
