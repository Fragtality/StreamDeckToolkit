using System;
using Newtonsoft.Json;
using Serilog;

namespace StreamDeckLib.Messages
{
	public class StreamDeckEventPayload
	{
		public string action { get; set; }

		[JsonProperty(PropertyName = "event")]
		public string Event { get; set; }
		public string context { get; set; }
		public string device { get; set; }

		public Deviceinfo deviceInfo { get; set; }

		public Payload payload { get; set; }

		public class Payload
		{
			public dynamic settings { get; set; }
			public Coordinates coordinates { get; set; }
			public int state { get; set; }
			public int userDesiredState { get; set; }
			public bool isInMultiAction { get; set; }
			public string title { get; set; }
			public TitleParameters titleParameters { get; set; }
			public string application { get; set; }
		}

		public class TitleParameters
		{
			public string fontFamily { get; set; }
			public int fontSize { get; set; }
			public string fontStyle { get; set; }
			public bool fontUnderline { get; set; }
			public bool showTitle { get; set; }
			public string titleAlignment { get; set; }
			public string titleColor { get; set; }
		}

		public class Deviceinfo
		{
			public string name { get; set; } //CHANGED
			public int type { get; set; }
			public Size size { get; set; }
		}

		//CHANGED
		public static void SetModelProperties<T>(StreamDeckEventPayload args, T SettingsModel)
		{
			try
			{
				var properties = typeof(T).GetProperties();
				foreach (var prop in properties)
				{
					if (args.payload != null && args.payload.settings != null && args.payload.settings.settingsModel != null)
					{
						if (args.PayloadSettingsHasProperty(prop.Name))
						{
							try
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
							catch
							{
								Log.Logger.Error($"StreamDeckEventPayload:SetModelProperties - Exception during setting Property {prop?.Name}!");
							}
						}
					}
				}
			}
			catch
			{
				Log.Logger.Error($"StreamDeckEventPayload:SetModelProperties - Unknown Exception!");
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
