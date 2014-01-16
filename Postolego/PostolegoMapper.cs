using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Postolego {
    public class PostolegoMapper : UriMapperBase {
        public override Uri MapUri(Uri uri) {
            var tempUri = HttpUtility.UrlDecode(uri.ToString());
            System.Diagnostics.Debug.WriteLine(tempUri);

            if(tempUri.Contains("postolego:")) {
                System.Diagnostics.Debug.WriteLine("has postolego:");
                if(tempUri.Contains("authorize")) {
                    System.Diagnostics.Debug.WriteLine("Has authorized");
                    return new Uri("/Pages/SignInPage.xaml", UriKind.Relative);
                }
                System.Diagnostics.Debug.WriteLine("Has not authorze");
            }
            if((App.Current.Resources["PostolegoData"] as PostolegoData).PocketSession == null || !(App.Current.Resources["PostolegoData"] as PostolegoData).PocketSession.IsAuthenticated) {
                return new Uri("/Pages/SignInPage.xaml", UriKind.Relative);
            } else {
                return uri;
            }
        }
    }
}
