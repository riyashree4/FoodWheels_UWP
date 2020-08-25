using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace FWApp.Services
{
   public class NavigationService
    {
        //set the frame that we want to use when we do navigation
        public void SetFrame(Frame Frame)
        {
            _Frame = Frame;


            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                GoBack();
            };
        }

        Frame _Frame;

        public string CurrentPageName
        {
            get
            {
                return _Frame.Content?.GetType().Name ?? "";
            }
        }

        public void GoBack()
        {
            if (_Frame.CanGoBack)
                _Frame.GoBack();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public void GotoPage(string PageTypeName)
        {
            if (String.Compare(CurrentPageName, PageTypeName, true) == 0)
                return;
            var PageType = typeof(NavigationService).GetTypeInfo().Assembly.DefinedTypes.Where(t => t.FullName.EndsWith(PageTypeName, StringComparison.OrdinalIgnoreCase)).Select(t => t.AsType()).FirstOrDefault();
            _Frame.Navigate(PageType, "wibble");

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            if (NavigatedForward != null)
                NavigatedForward();
        }

        public event Action NavigatedForward;

        private static NavigationService _Instance;

        public static NavigationService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new NavigationService();
                return _Instance;
            }
        }
    }
}
