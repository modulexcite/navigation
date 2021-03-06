﻿#if NET40Plus
using System.Web;

namespace Navigation
{
	internal class MockNavigationResponse : HttpResponseBase
	{
		private HttpCookieCollection _Cookies;

		internal MockNavigationResponse(State state)
			: base()
		{
			State = state;
		}

		private State State
		{
			get;
			set;
		}

		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}

		public override HttpCookieCollection Cookies
		{
			get
			{
				if (_Cookies == null)
					_Cookies = new HttpCookieCollection();
				return _Cookies;
			}
		}

		public override void Redirect(string url)
		{
			Redirect(url, true);
		}

		public override void Redirect(string url, bool endResponse)
		{
			Redirect(url, endResponse, false);
		}

		public override void RedirectPermanent(string url)
		{
			Redirect(url, true, true);
		}

		public override void RedirectPermanent(string url, bool endResponse)
		{
			Redirect(url, endResponse, true);
		}

		private void Redirect(string url, bool endResponse, bool permanent)
		{
			StateController.SetStateContext(State.Id, new MockNavigationContext(url, State));
		}
	}
}
#endif