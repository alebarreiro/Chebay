using System;
using Facebook;
using Facebook.Client;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;

namespace WindowsPhoneAPP.Common
{

    public class FacebookHelper
    {
     /*   private readonly Uri callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

        private readonly Uri loginUri;

        private const string AppId = "INSERT FACEBOOK APP ID HERE";

        private const string AppPermissions = "user_about_me,read_stream,publish_stream,publish_actions";

        public FacebookHelper()
        {
            this.Client = new FacebookClient();

            this.loginUri =
                this.Client.GetLoginUrl(
                    new
                        {
                            client_id = AppId,
                            redirect_uri = this.callbackUri.AbsoluteUri,
                            scope = AppPermissions,
                            display = "popup",
                            response_type = "token"
                        });
        }

        //public FacebookClient Client { get; private set; }

        public string AccessToken
        {
            get
            {
                return this.Client.AccessToken;
            }
        }

        public void LoginContinuation()
        {
            WebAuthenticationBroker.AuthenticateAndContinue(this.loginUri);
        }

        public void ContinueAuth(WebAuthenticationBrokerContinuationEventArgs args)
        {
            this.Validate(args.WebAuthenticationResult);
        }

        private void Validate(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    {
                        var responseUri = new Uri(result.ResponseData.ToString());

                        var oauthResult = this.Client.ParseOAuthCallbackUrl(responseUri);

                        if (string.IsNullOrWhiteSpace(oauthResult.Error))
                        {
                            this.Client.AccessToken = oauthResult.AccessToken;
                        }
                        else
                        {
                            // ToDo: Handle error.
                        }
                    }

                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    break;
                default:
                    this.Client.AccessToken = null;
                    break;
            }
        }*/
    }
}
