﻿using System;
using System.Configuration;

namespace Navigation
{
	/// <summary>
	/// Provides access to the Navigation Settings configuration section
	/// </summary>
	public class NavigationSettings : ConfigurationSection
	{
		private static NavigationSettings _Config;

		static NavigationSettings()
		{
			_Config = (NavigationSettings)ConfigurationManager.GetSection("Navigation/Settings");
			if (_Config == null)
			{
				_Config = new NavigationSettings();
				_Config.SetReadOnly();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static NavigationSettings Config
		{
			get
			{
				return _Config;
			}
		}

		/// <summary>
		/// Gets or sets whether to revert to using ! and _ as separators in the Url
		/// </summary>
		[ConfigurationProperty("originalUrlSeparators", DefaultValue = false)]
		public bool OriginalUrlSeparators
		{
			get
			{
				return (bool)this["originalUrlSeparators"];
			}
			set
			{
				this["originalUrlSeparators"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether to revert to using short State keys in the crumb trail
		/// </summary>
		[ConfigurationProperty("originalCrumbTrailKeys", DefaultValue = false)]
		public bool OriginalCrumbTrailKeys
		{
			get
			{
				return (bool)this["originalCrumbTrailKeys"];
			}
			set
			{
				this["originalCrumbTrailKeys"] = value;
			}
		}

#if NET45Plus
		/// <summary>
		/// Gets or sets the custom <see cref="Navigation.StateRouteHandler"/>
		/// </summary>
		[ConfigurationProperty("stateRouteHandler", DefaultValue = "")]
		public string StateRouteHandler
		{
			get
			{
				return (string)this["stateRouteHandler"];
			}
			set
			{
				this["stateRouteHandler"] = value;
			}
		}
#endif

		/// <summary>
		/// Gets or sets the application path to use outside of a web context
		/// </summary>
		[ConfigurationProperty("applicationPath", DefaultValue = "/")]
		public string ApplicationPath
		{
			get
			{
				return (string)this["applicationPath"];
			}
			set
			{
				this["applicationPath"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Url for the login page that the authentication provider will redirect to
		/// </summary>
		[ConfigurationProperty("loginPath", DefaultValue = "")]
		public string LoginPath
		{
			get
			{
				return (string)this["loginPath"];
			}
			set
			{
				this["loginPath"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the query string key that identifies the Url the authentication provider will
		/// redirect to after a successful login
		/// </summary>
		[ConfigurationProperty("returnUrlKey", DefaultValue = "ReturnUrl")]
		public string ReturnUrlKey
		{
			get
			{
				return (string)this["returnUrlKey"];
			}
			set
			{
				this["returnUrlKey"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that identifies the StateId
		/// </summary>
		[ConfigurationProperty("stateIdKey", DefaultValue = "c0")]
		public string StateIdKey
		{
			get
			{
				return (string)this["stateIdKey"];
			}
			set
			{
				this["stateIdKey"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that identifies the PreviousStateId
		/// </summary>
		[ConfigurationProperty("previousStateIdKey", DefaultValue = "c1")]
		public string PreviousStateIdKey
		{
			get
			{
				return (string)this["previousStateIdKey"];
			}
			set
			{
				this["previousStateIdKey"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that identifies the ReturnData
		/// </summary>
		[ConfigurationProperty("returnDataKey", DefaultValue = "c2")]
		public string ReturnDataKey
		{
			get
			{
				return (string)this["returnDataKey"];
			}
			set
			{
				this["returnDataKey"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that identifies the CrumbTrail
		/// </summary>
		[ConfigurationProperty("crumbTrailKey", DefaultValue = "c3")]
		public string CrumbTrailKey
		{
			get
			{
				return (string)this["crumbTrailKey"];
			}
			set
			{
				this["crumbTrailKey"] = value;
			}
		}
	}
}
