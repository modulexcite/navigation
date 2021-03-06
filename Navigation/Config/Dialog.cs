using System;
#if !NET40Plus
using System.Threading;
using System.Web;
#endif

namespace Navigation
{
	/// <summary>
	/// Configures dialog information. Represents a logical grouping of child <see cref="Navigation.State"/> elements.
	/// Navigating across different dialogs will initialise the crumb trail
	/// </summary>
	[Serializable]
	public partial class Dialog
	{
		private StateInfoCollection<State> _States = new StateInfoCollection<State>();
		private int _Index;
		private State _Initial;
		private string _Key;
		private string _Title;
#if !NET40Plus
		private string _ResourceType;
		private string _ResourceKey;
#endif
		private StateInfoCollection<string> _Attributes;

		/// <summary>
		/// Gets the <see cref="Navigation.State"/> children
		/// </summary>
		public StateInfoCollection<State> States
		{
			get
			{
				return _States;
			}
		}

		/// <summary>
		/// Gets the number of the dialog
		/// </summary>
		public int Index
		{
			get
			{
				return _Index;
			}
			internal set
			{
				_Index = value;
			}
		}

		/// <summary>
		/// Gets the state to navigate to if the <see cref="Key"/> is passed as an action parameter
		/// to the <see cref="Navigation.StateController"/>
		/// </summary>
		public State Initial
		{
			get
			{
				return _Initial;
			}
			internal set
			{
				_Initial = value;
			}
		}

		/// <summary>
		/// Gets the key, unique across dialogs, which is passed as the action
		/// parameter to the <see cref="Navigation.StateController"/> when navigating
		/// </summary>
		public string Key
		{
			get
			{
				return _Key;
			}
			internal set
			{
				_Key = value;
			}
		}

		/// <summary>
		/// Gets the textual description of the dialog. The resourceType and resourceKey attributes can be 
		/// used for localization
		/// </summary>
		public string Title
		{
			get
			{
#if NET40Plus
				if (TitleFunc != null)
					return TitleFunc();
#else
				if (ResourceKey.Length != 0)
				{
					return (string)HttpContext.GetGlobalResourceObject(ResourceType, ResourceKey, Thread.CurrentThread.CurrentUICulture);
				}
#endif
				return _Title;
			}
			internal set
			{
				_Title = value;
			}
		}

#if NET40Plus
		internal Func<string> TitleFunc
		{
			get;
			set;
		}
#else
		internal string ResourceType
		{
			get
			{
				return _ResourceType;
			}
			set
			{
				_ResourceType = value;
			}
		}

		internal string ResourceKey
		{
			get
			{
				return _ResourceKey;
			}
			set
			{
				_ResourceKey = value;
			}
		}
#endif

		/// <summary>
		/// Gets the list of attributes
		/// </summary>
		public StateInfoCollection<string> Attributes
		{
			get
			{
				return _Attributes;
			}
			internal set
			{
				_Attributes = value;
			}
		}
	}
}
