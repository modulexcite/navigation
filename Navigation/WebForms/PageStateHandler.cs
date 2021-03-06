﻿using System;
#if !NET40Plus
using System.Collections.Generic;
#endif
using System.Collections.Specialized;
using System.Text;
using System.Web;
#if NET40Plus
using System.Web.Routing;
#endif
#if NET45Plus
using System.Web.WebPages;
#endif

namespace Navigation
{
	/// <summary>
	/// Implementation of <see cref="Navigation.IStateHandler"/> that builds and parses
	/// navigation links for a Web Forms <see cref="Navigation.State"/>
	/// </summary>
#if NET40Plus
	public class PageStateHandler : StateHandler
#else
	public class PageStateHandler : IStateHandler
#endif
	{
#if NET40Plus
		/// <summary>
		/// Gets a link that navigates to the <paramref name="state"/> passing the <paramref name="data"/>.
		/// </summary>
		/// <param name="state">The <see cref="Navigation.State"/> to navigate to</param>
		/// <param name="data">The data to pass when navigating</param>
		/// <param name="context">The current context</param>
		/// <returns>The navigation link</returns>
		public override string GetNavigationLink(State state, NameValueCollection data, HttpContextBase context)
		{
			if (GetRouteName(state, context) != null)
			{
				return base.GetNavigationLink(state, data, context);
			}
			else
			{
				return GetLink(state, data, GetMobile(context), context.Request.ApplicationPath);
			}
		}

		/// <summary>
		/// Returns the route of the <paramref name="state"/>
		/// </summary>
		/// <param name="state">The <see cref="Navigation.State"/> to navigate to</param>
		/// <param name="context">The current context</param>
		/// <returns>The route name</returns>
		protected override string GetRouteName(State state, HttpContextBase context)
		{
			bool mobile = GetMobile(context);
			return RouteTable.Routes[state.GetRouteName(mobile)] != null ? state.GetRouteName(mobile) : null;
		}

		/// <summary>
		/// Returns true to terminate the current process when navigating with a 
		/// <see cref="NavigationMode.Client"/> mode
		/// </summary>
		/// <param name="state">The <see cref="Navigation.State"/> to navigate to</param>
		/// <param name="context">The current context</param>
		/// <returns>True to terminate the current process</returns>
		protected override bool GetEndResponse(State state, HttpContextBase context)
		{
			return true;
		}
#else
		/// <summary>
		/// Gets a link that navigates to the <paramref name="state"/> passing the <paramref name="data"/>
		/// </summary>
		/// <param name="state">The <see cref="Navigation.State"/> to navigate to</param>
		/// <param name="data">The data to pass when navigating</param>
		/// <returns>The navigation link</returns>
		public virtual string GetNavigationLink(State state, NameValueCollection data)
		{
			string applicationPath = HttpContext.Current != null ? HttpContext.Current.Request.ApplicationPath : NavigationSettings.Config.ApplicationPath;
			return GetLink(state, data, GetMobile(HttpContext.Current), applicationPath);
		}

		/// <summary>
		/// Gets the data parsed from the <paramref name="data">context data</paramref>
		/// </summary>
		/// <param name="state">The <see cref="Navigation.State"/> navigated to</param>
		/// <param name="data">The current context data</param>
		/// <returns>The navigation data</returns>
		public virtual NameValueCollection GetNavigationData(State state, NameValueCollection data)
		{
			return new NameValueCollection(data);
		}

		/// <summary>
		/// Redirects or Transfers to the <paramref name="url"/> depending on the 
		/// <paramref name="mode"/> specified
		/// </summary>
		/// <param name="state">The <see cref="State"/> to navigate to</param>
		/// <param name="url">The target location</param>
		/// <param name="mode">Indicates whether to Redirect or Transfer</param>
		public virtual void NavigateLink(State state, string url, NavigationMode mode)
		{
			if (HttpContext.Current == null) mode = NavigationMode.Mock;
			switch (mode)
			{
				case (NavigationMode.Client):
					{
						HttpContext.Current.Response.Redirect(url, true);
						break;
					}
				case (NavigationMode.Server):
					{
						HttpContext.Current.Server.Transfer(url);
						break;
					}
				case (NavigationMode.Mock):
					{
						StateController.SetStateContext(state.Id, HttpUtility.ParseQueryString(url.Substring(url.IndexOf("?", StringComparison.Ordinal))));
						break;
					}
			}
		}

		/// <summary>
		/// Truncates the crumb trail whenever a repeated or initial <see cref="State"/> is encountered
		/// </summary>
		/// <param name="state">The <see cref="State"/> navigated to</param>
		/// <param name="crumbs">The <see cref="Crumb"/> collection representing the crumb trail</param>
		/// <returns>Truncated crumb trail</returns>
		public virtual List<Crumb> TruncateCrumbTrail(State state, List<Crumb> crumbs)
		{
			var newCrumbs = new List<Crumb>();
			if (state.Parent.Initial == state)
				return newCrumbs;
			foreach (var crumb in crumbs)
			{
				if (crumb.State == state)
					break;
				newCrumbs.Add(crumb);
			}
			return newCrumbs;
		}
#endif
#if NET40Plus
		private bool GetMobile(HttpContextBase context)
#else
		private bool GetMobile(HttpContext context)
#endif
		{
#if NET45Plus
			return context.GetOverriddenBrowser().IsMobileDevice;
#else
			return context != null && context.Request.Browser.IsMobileDevice;
#endif
		}

		private string GetLink(State state, NameValueCollection data, bool mobile, string applicationPath)
		{
			StringBuilder link = new StringBuilder();
			link.Append(VirtualPathUtility.ToAbsolute(state.GetPage(mobile), applicationPath));
			link.Append("?");
			link.Append(HttpUtility.UrlEncode(NavigationSettings.Config.StateIdKey));
			link.Append("=");
			link.Append(HttpUtility.UrlEncode(state.Id));
			foreach (string key in data)
			{
				if (key != NavigationSettings.Config.StateIdKey)
				{
					link.Append("&");
					link.Append(HttpUtility.UrlEncode(key));
					link.Append("=");
					link.Append(HttpUtility.UrlEncode(data[key]));
				}
			}
			return link.ToString();
		}
	}
}
