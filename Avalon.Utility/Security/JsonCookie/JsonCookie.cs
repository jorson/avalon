namespace Avalon.Security.Cryptography
{
    using System;
    using System.Web;

    /// <summary>
    /// Base class for storing data to cookies as an object
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class JsonCookie<TObject> where TObject : JsonCookie<TObject>, new()
    {
        /// <summary>
        /// Descendant class object used to get Cookie name if null or empty in method calls
        /// </summary>
        private static readonly TObject Descendant = new TObject();

        /// <summary>
        /// Gets the name of the cookie.
        /// </summary>
        /// <code lang="C#" title="Example">
        /// <![CDATA[
        /// public override string GetCookieName()
        /// {
        ///     return "HelloWorld";
        /// }
        /// ]]>
        /// </code>
        /// <remarks>
        /// Prevents the need to use Reflection and Attributes to get custom cookie names
        /// </remarks>
        /// <returns>The cookie name</returns>
        [NotNull]
        public static string ResolveCookieName()
        {
            var name = Descendant.GetCookieName();
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return typeof(TObject).Name;
        }

        /// <summary>
        /// Reads the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        /// <returns>
        /// The Object
        /// </returns>
        [CanBeNull]
        public static TObject ReadCookie([NotNull] HttpContext context, string prefix = "")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookieName = prefix + ResolveCookieName();
                var cookie = context.Request.Cookies[cookieName];

                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    JsonCookieEncryptionType encryptionType = Descendant.ResolveEncryptionType();
                    TObject instance;
                    switch (encryptionType)
                    {
                        case JsonCookieEncryptionType.EncryptDES:
                            instance = cookie.Value.DecryptAsDES().JsonDecode<TObject>();
                            break;

                        case JsonCookieEncryptionType.EncryptBase64:
                            instance = cookie.Value.DecodeAsBase64().JsonDecode<TObject>();
                            break;
                        default:
                            instance = cookie.Value.JsonDecode<TObject>();
                            break;
                    }

                    return instance;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        /// <returns>
        /// The Object
        /// </returns>
        [CanBeNull]
        public static TObject ReadCookie([NotNull] HttpContextBase context, string prefix = "")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookieName = prefix+ResolveCookieName();
                var cookie = context.Request.Cookies[cookieName];

                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    JsonCookieEncryptionType encryptionType = Descendant.ResolveEncryptionType();
                    var cookieValue = cookie.Value;
                    TObject instance;
                    switch (encryptionType)
                    {
                        case JsonCookieEncryptionType.EncryptDES:
                            instance = cookieValue.DecryptAsDES().JsonDecode<TObject>();
                            break;

                        case JsonCookieEncryptionType.EncryptBase64:
                            instance = cookieValue.DecodeAsBase64().JsonDecode<TObject>();
                            break;
                        default:
                            instance = cookieValue.JsonDecode<TObject>();
                            break;
                    }

                    return instance;
                }
            }

            return null;
        }

        /// <summary>
        /// Deletes the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        public static void DeleteCookie([NotNull] HttpContext context, string prefix = "")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookieName = prefix+ResolveCookieName();
                var cookie = context.Request.Cookies[cookieName];

                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    context.Response.Cookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Deletes the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        /// <param name="path"></param>
        public static void DeleteCookie([NotNull] HttpContextBase context, string prefix = "",string path="/")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookieName = prefix+ResolveCookieName();
                var cookie = context.Request.Cookies[cookieName];

                if (cookie != null)
                {
                    cookie.Path = path;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    context.Response.Cookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Writes the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        /// <returns>
        /// The created cookie
        /// </returns>
        [CanBeNull]
        public virtual HttpCookie WriteCookie([NotNull] HttpContext context, string prefix = "")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookie = this.ResolveNewHttpCookie(prefix);
                cookie.Value = this.GetCookieValue();

                context.Response.SetCookie(cookie);

                return cookie;
            }

            return null;
        }


        /// <summary>
        /// Writes the cookie.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prefix">前缀</param>
        /// <returns>
        /// The created cookie
        /// </returns>
        [CanBeNull]
        public virtual HttpCookie WriteCookie([NotNull] HttpContextBase context, string prefix = "")
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (context != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                var cookie = this.ResolveNewHttpCookie(prefix);
                cookie.Value = this.GetCookieValue();

                context.Response.SetCookie(cookie);

                return cookie;
            }

            return null;
        }

        /// <summary>
        /// Gets the name of the cookie. If not implemented, will use the Descendant class name.
        /// </summary>
        /// <returns>
        /// The cookie name to use
        /// </returns>
        [NotNull]
        public virtual string GetCookieName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Uses the encryption.
        /// </summary>
        /// <returns>
        /// True or false to use encryption
        /// </returns>
        public virtual JsonCookieEncryptionType ResolveEncryptionType()
        {
            return JsonCookieEncryptionType.EncryptDES;
        }

        /// <summary>
        /// Resolves the new HTTP cookie.
        /// </summary>
        /// <returns>The new Http Cookie</returns>
        /// <remarks>This method allow to override or change cookie parameters</remarks>
        public virtual HttpCookie ResolveNewHttpCookie(string prefix = "")
        {
            var cookie = new HttpCookie(prefix+this.GetCookieName());
            /* 
             * cookie.Path  = "/"
             */

            return cookie;
        }

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <returns>The cookie value</returns>
        protected string GetCookieValue()
        {
            JsonCookieEncryptionType encryptionType = this.ResolveEncryptionType();

            string cookieValue;
            switch (encryptionType)
            {
                case JsonCookieEncryptionType.EncryptDES:
                    cookieValue = this.JsonEncode().EncryptAsDES();
                    break;
                case JsonCookieEncryptionType.EncryptBase64:
                    cookieValue = this.JsonEncode().EncodeAsBase64();
                    break;

                default:
                    cookieValue = this.JsonEncode();
                    break;
            }

            return cookieValue;
        }
    }
}
